import { Client } from "pg";
import { faker } from "@faker-js/faker";

function toUtcIso(value: Date | string | number): string {
    return new Date(value).toISOString();
}

function pickRandom<T>(items: T[]): T {
    if (items.length === 0) throw new Error("Cannot pick from empty array");
    return items[Math.floor(Math.random() * items.length)];
}

async function main(): Promise<void> {
    const connectionString = process.env.DATABASE_URL;
    if (!connectionString) {
        throw new Error("DATABASE_URL is missing");
    }

    const client = new Client({ connectionString });
    await client.connect();

    try {
        await client.query("BEGIN");

        await client.query(`
      TRUNCATE TABLE
        "Enrollment",
        "Course",
        "Student",
        "Subject",
        "Semester"
      RESTART IDENTITY CASCADE
    `);

        const semesterIds: number[] = [];
        for (let i = 1; i <= 5; i++) {
            const start = faker.date.future({ years: 1 });
            const end = new Date(start);
            end.setMonth(end.getMonth() + 4);

            const result = await client.query<{ SemesterId: number }>(
                `
        INSERT INTO "Semester" ("SemesterName", "StartDate", "EndDate")
        VALUES ($1, $2, $3)
        RETURNING "SemesterId"
        `,
                [`Semester ${i}`, toUtcIso(start), toUtcIso(end)],
            );

            semesterIds.push(result.rows[0].SemesterId);
        }

        const subjectIds: number[] = [];
        for (let i = 1; i <= 10; i++) {
            const result = await client.query<{ SubjectId: number }>(
                `
        INSERT INTO "Subject" ("SubjectCode", "SubjectName", "Credit")
        VALUES ($1, $2, $3)
        RETURNING "SubjectId"
        `,
                [
                    `SUB${String(i).padStart(3, "0")}`,
                    faker.commerce.productName(),
                    faker.number.int({ min: 1, max: 4 }),
                ],
            );

            subjectIds.push(result.rows[0].SubjectId);
        }

        const courseIds: number[] = [];
        for (let i = 1; i <= 20; i++) {
            const result = await client.query<{ CourseId: number }>(
                `
        INSERT INTO "Course" ("CourseName", "SemesterId")
        VALUES ($1, $2)
        RETURNING "CourseId"
        `,
                [
                    `Course ${i} - ${faker.hacker.phrase()}`,
                    pickRandom(semesterIds),
                ],
            );

            courseIds.push(result.rows[0].CourseId);
        }

        const studentIds: number[] = [];
        for (let i = 1; i <= 50; i++) {
            const fullName = faker.person.fullName();
            const parts = fullName.split(" ");
            const firstName = parts[0] ?? "John";
            const lastName = parts.at(-1) ?? "Doe";

            const result = await client.query<{ StudentId: number }>(
                `
        INSERT INTO "Student" ("FullName", "Email", "DateOfBirth")
        VALUES ($1, $2, $3)
        RETURNING "StudentId"
        `,
                [
                    fullName,
                    faker.internet.email({ firstName, lastName }),
                    toUtcIso(
                        faker.date.birthdate({ min: 18, max: 30, mode: "age" }),
                    ),
                ],
            );

            studentIds.push(result.rows[0].StudentId);
        }

        const statuses = ["Active", "Completed", "Dropped"];

        for (let i = 0; i < 500; i++) {
            await client.query(
                `
        INSERT INTO "Enrollment" ("StudentId", "CourseId", "EnrollDate", "Status")
        VALUES ($1, $2, $3, $4)
        `,
                [
                    pickRandom(studentIds),
                    pickRandom(courseIds),
                    toUtcIso(faker.date.past({ years: 1 })),
                    pickRandom(statuses),
                ],
            );
        }

        await client.query("COMMIT");
        console.log("Seed completed successfully.");
    } catch (err) {
        await client.query("ROLLBACK");
        throw err;
    } finally {
        await client.end();
    }
}

main().catch((err) => {
    console.error(err);
    process.exit(1);
});
