import { Client } from "pg";

type Semester = [number, string, Date, Date];
type Subject = [number, string, string, number];
type Student = [number, string, string, Date];
type Course = [number, string, number];
type Enrollment = [number, number, number, Date, string];

const DATABASE_URL = process.env.DATABASE_URL;

if (!DATABASE_URL) {
    throw new Error("DATABASE_URL is not set");
}

const FIRST_NAMES = [
    "An",
    "Binh",
    "Chi",
    "Dung",
    "Hanh",
    "Hieu",
    "Khang",
    "Linh",
    "Minh",
    "Nga",
    "Phong",
    "Quang",
    "Son",
    "Trang",
    "Tuan",
    "Vy",
    "Hoa",
    "Nam",
    "Huyen",
    "Tung",
];

const LAST_NAMES = [
    "Nguyen",
    "Tran",
    "Le",
    "Pham",
    "Hoang",
    "Vo",
    "Dang",
    "Bui",
    "Do",
    "Ngo",
];

const STUDENT_STATUSES = ["Enrolled", "Completed", "Dropped", "Withdrawn"];

const SEMESTERS_SOURCE = [
    [
        "Spring 2023",
        new Date("2023-01-15T00:00:00Z"),
        new Date("2023-05-31T00:00:00Z"),
    ],
    [
        "Fall 2023",
        new Date("2023-08-15T00:00:00Z"),
        new Date("2023-12-20T00:00:00Z"),
    ],
    [
        "Spring 2024",
        new Date("2024-01-15T00:00:00Z"),
        new Date("2024-05-31T00:00:00Z"),
    ],
    [
        "Fall 2024",
        new Date("2024-08-15T00:00:00Z"),
        new Date("2024-12-20T00:00:00Z"),
    ],
    [
        "Spring 2025",
        new Date("2025-01-15T00:00:00Z"),
        new Date("2025-05-31T00:00:00Z"),
    ],
] as const;

const SUBJECTS_SOURCE = [
    ["MATH101", "Calculus I", 3],
    ["MATH102", "Calculus II", 3],
    ["PHYS101", "Physics I", 4],
    ["CS101", "Introduction to Programming", 3],
    ["CS102", "Data Structures", 3],
    ["DB101", "Database Systems", 3],
    ["SE101", "Software Engineering", 3],
    ["ENG101", "Academic Writing", 2],
    ["STAT101", "Statistics", 3],
    ["AI101", "Introduction to AI", 3],
] as const;

const COURSE_TITLES = [
    "Morning Section",
    "Afternoon Section",
    "Evening Section",
    "Lab Section",
];

function seedRandom(seed: number) {
    let value = seed;

    return () => {
        value = (value * 1664525 + 1013904223) % 4294967296;
        return value / 4294967296;
    };
}

const rand = seedRandom(42);

function randomInt(min: number, max: number): number {
    return Math.floor(rand() * (max - min + 1)) + min;
}

function choice<T>(arr: readonly T[]): T {
    return arr[randomInt(0, arr.length - 1)];
}

function randomName(): string {
    return `${choice(LAST_NAMES)} ${choice(FIRST_NAMES)}`;
}

function randomEmail(fullName: string, studentId: number): string {
    return `${fullName.toLowerCase().replace(/\s+/g, ".")}${studentId}@example.com`;
}

function randomDob(): Date {
    const start = new Date("1998-01-01T00:00:00Z").getTime();
    const end = new Date("2007-12-31T00:00:00Z").getTime();

    return new Date(randomInt(start, end));
}

function randomDateBetween(start: Date, end: Date): Date {
    return new Date(randomInt(start.getTime(), end.getTime()));
}

function generateSemesters(): Semester[] {
    return SEMESTERS_SOURCE.map((s, index) => [index + 1, s[0], s[1], s[2]]);
}

function generateSubjects(): Subject[] {
    return SUBJECTS_SOURCE.map((s, index) => [index + 1, s[0], s[1], s[2]]);
}

function generateStudents(count = 50): Student[] {
    const students: Student[] = [];

    for (let i = 1; i <= count; i++) {
        const fullName = randomName();

        students.push([i, fullName, randomEmail(fullName, i), randomDob()]);
    }

    return students;
}

function generateCourses(semesters: Semester[]): Course[] {
    const courses: Course[] = [];

    let courseId = 1;

    for (const semester of semesters) {
        const semesterId = semester[0];
        const semesterName = semester[1];

        for (const title of COURSE_TITLES) {
            courses.push([courseId, `${semesterName} - ${title}`, semesterId]);

            courseId++;
        }
    }

    return courses;
}

function generateEnrollments(
    students: Student[],
    courses: Course[],
    semesters: Semester[],
    targetCount = 600,
): Enrollment[] {
    const enrollments: Enrollment[] = [];

    const usedPairs = new Set<string>();

    const semesterMap = new Map<number, Semester>();
    const courseMap = new Map<number, Course>();

    for (const semester of semesters) {
        semesterMap.set(semester[0], semester);
    }

    for (const course of courses) {
        courseMap.set(course[0], course);
    }

    let enrollmentId = 1;

    while (enrollments.length < targetCount) {
        const student = choice(students);
        const course = choice(courses);

        const key = `${student[0]}-${course[0]}`;

        if (usedPairs.has(key)) {
            continue;
        }

        usedPairs.add(key);

        const semester = semesterMap.get(course[2]);

        if (!semester) {
            continue;
        }

        const enrollDate = randomDateBetween(semester[2], semester[3]);

        enrollments.push([
            enrollmentId,
            student[0],
            course[0],
            enrollDate,
            choice(STUDENT_STATUSES),
        ]);

        enrollmentId++;

        if (usedPairs.size >= students.length * courses.length) {
            break;
        }
    }

    return enrollments;
}

async function insertMany(client: Client, query: string, rows: any[][]) {
    for (const row of rows) {
        await client.query(query, row);
    }
}

async function main() {
    const semesters = generateSemesters();
    const subjects = generateSubjects();
    const students = generateStudents(50);
    const courses = generateCourses(semesters);
    const enrollments = generateEnrollments(students, courses, semesters, 600);

    const client = new Client({
        connectionString: DATABASE_URL,
    });

    await client.connect();

    try {
        await client.query("BEGIN");

        await insertMany(
            client,
            `
      INSERT INTO Semester (
        SemesterId,
        SemesterName,
        StartDate,
        EndDate
      )
      VALUES ($1, $2, $3, $4)
      `,
            semesters,
        );

        await insertMany(
            client,
            `
      INSERT INTO Subject (
        SubjectId,
        SubjectCode,
        SubjectName,
        Credit
      )
      VALUES ($1, $2, $3, $4)
      `,
            subjects,
        );

        await insertMany(
            client,
            `
      INSERT INTO Student (
        StudentId,
        FullName,
        Email,
        DateOfBirth
      )
      VALUES ($1, $2, $3, $4)
      `,
            students,
        );

        await insertMany(
            client,
            `
      INSERT INTO Course (
        CourseId,
        CourseName,
        SemesterId
      )
      VALUES ($1, $2, $3)
      `,
            courses,
        );

        await insertMany(
            client,
            `
      INSERT INTO Enrollment (
        EnrollmentId,
        StudentId,
        CourseId,
        EnrollDate,
        Status
      )
      VALUES ($1, $2, $3, $4, $5)
      `,
            enrollments,
        );

        await client.query("COMMIT");

        console.log("Seed completed");
        console.log(`Semesters: ${semesters.length}`);
        console.log(`Subjects: ${subjects.length}`);
        console.log(`Students: ${students.length}`);
        console.log(`Courses: ${courses.length}`);
        console.log(`Enrollments: ${enrollments.length}`);
    } catch (error) {
        await client.query("ROLLBACK");
        console.error(error);
    } finally {
        await client.end();
    }
}

main().catch(console.error);

