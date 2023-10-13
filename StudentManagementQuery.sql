CREATE TABLE account(
	username VARCHAR(20),
	password VARCHAR(20),
	role INT,
	CONSTRAINT PK_account PRIMARY KEY(username)
);

CREATE TABLE student(
	student_id VARCHAR(10),
	name VARCHAR(20),
	birthday DATE,
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

CREATE OR REPLACE FUNCTION USP_Login(username VARCHAR(30), password VARCHAR(30))
RETURNS SETOF account
LANGUAGE SQL
AS 
$$
	SELECT * FROM account WHERE account.username = USP_Login.username AND account.password = USP_Login.password
$$

INSERT INTO account VALUES('22521161', 'quark724', 1);