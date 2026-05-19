CREATE TABLE IF NOT EXISTS Semester(
    SemesterId int,
    SemesterName varchar(100),
    StartDate timestamptz,
    EndDate timestamptz
);

CREATE TABLE IF NOT EXISTS Course(
    CourseId int,
    CourseName varchar(100),
    SemesterId int
);

CREATE TABLE IF NOT EXISTS Subject(
    SubjectId int,
    SubjectCode varchar(20),
    SubjectName varchar(100),
    Credit int
);

CREATE TABLE IF NOT EXISTS Student(
    StudentId int,
    FullName varchar(100),
    Email varchar(100),
    DateOfBirth timestamptz
);

CREATE TABLE IF NOT EXISTS Enrollment(
    EnrollmentId int,
    StudentId int,
    CourseId int,
    EnrollDate timestamptz,
    Status varchar(20)
);
