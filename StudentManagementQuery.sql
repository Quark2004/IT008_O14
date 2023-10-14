CREATE TABLE account(
	username VARCHAR(20),
	password VARCHAR(20),
	role INT,
	CONSTRAINT PK_account PRIMARY KEY(username)
);

CREATE TABLE student(
	student_id VARCHAR(10),
	name VARCHAR(20),
	birthday TIMESTAMP,
	gender VARCHAR(3),
	class_id VARCHAR(10),
	CONSTRAINT PK_student PRIMARY KEY(student_id)
);

CREATE TABLE class(
	class_id VARCHAR(10),
	name VARCHAR(20),
	num_of_students INT,
	CONSTRAINT PK_class PRIMARY KEY(class_id)
);

CREATE TABLE lecturer(
	lecturer_id VARCHAR(10),
	name VARCHAR(20),
	gender VARCHAr(3),
	CONSTRAINT PK_lecturer PRIMARY KEY(lecturer_id)
);

CREATE TABLE subject(
	subject_id VARCHAR(10),
	name VARCHAR(20),
	num_of_credits INT,
	CONSTRAINT PK_subject PRIMARY KEY(subject_id)
);

CREATE TABLE teaching(
	class_id VARCHAR(10),
	subject_id VARCHAR(10),
	lecturer_id VARCHAR(10),
	start_date DATE,
	end_date DATE,
	CONSTRAINT PK_teaching PRIMARY KEY(class_id, subject_id)
);

CREATE OR REPLACE FUNCTION Account_Login(username VARCHAR(30), password VARCHAR(30))
RETURNS SETOF account
LANGUAGE SQL
AS 
$$
	SELECT * FROM account WHERE account.username = Account_Login.username AND account.password = Account_Login.password
$$

CREATE OR REPLACE FUNCTION Student_GetInfo(id VARCHAR(10))
RETURNS SETOF student
LANGUAGE SQL
AS
$$
	SELECT * FROM student WHERE student.student_id = Student_GetInfo.id
$$

INSERT INTO account VALUES('22521161', 'quark724', 1);
INSERT INTO account VALUES('22529507', 'mylove', 2);
INSERT INTO account VALUES('admin', 'admin', 3);
INSERT INTO student VALUES('22521161', 'Hồ Văn Phương', '07-02-2004', 'Nam', 'KTPM2022.2');

