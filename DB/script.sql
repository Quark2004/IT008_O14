CREATE TABLE Account (
    username VARCHAR(100) PRIMARY KEY,
    password VARCHAR(1000) NOT NULL,
    role VARCHAR(100) DEFAULT 'student'
);

CREATE TABLE Profile (
    id VARCHAR(100) PRIMARY KEY,
    name VARCHAR(100),
    birthday TIMESTAMP CHECK (birthday < CURRENT_TIMESTAMP),
    gender VARCHAR(100),
    level VARCHAR(100) DEFAULT 'Đại học',
    trainingSystem VARCHAR(100) DEFAULT 'Chính quy',
    avatar BYTEA  
);

CREATE TABLE UserAcc (
    id SERIAL PRIMARY KEY, 
    idAccount VARCHAR(100) REFERENCES Account(username),
    idProfile VARCHAR(100) REFERENCES Profile(id),
    UNIQUE (idAccount, idProfile)
);


CREATE TABLE Course (
    id VARCHAR(100) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    numberOfCredits INT NOT NULL,
    schoolDay VARCHAR(100) NOT NULL,
    lesson VARCHAR(100) NOT NULL,
    classroom VARCHAR(100) NOT NULL,
    semester VARCHAR(100) NOT NULL,
    schoolYear VARCHAR(100) NOT NULL,
    startDay DATE NOT NULL,
    endDay DATE NOT NULL,
    CHECK (endDay > startDay)
);

CREATE TABLE Score (
    id SERIAL PRIMARY KEY, 
    processScore FLOAT,
    midtermScore FLOAT,
    finalScore FLOAT,
    practiceScore FLOAT,
    ratioProcess FLOAT DEFAULT 0.1 CHECK (ratioProcess >= 0 AND ratioProcess <= 1),
    ratioMidterm FLOAT DEFAULT 0.2 CHECK (ratioMidterm >= 0 AND ratioMidterm <= 1),
    ratioPractice FLOAT DEFAULT 0.3 CHECK (ratioPractice >= 0 AND ratioPractice <= 1),
    ratioFinal FLOAT DEFAULT 0.4 CHECK (ratioFinal >= 0 AND ratioFinal <= 1)
);

CREATE TABLE Schedule (
    id SERIAL PRIMARY KEY, 
    idProfile VARCHAR(100) NOT NULL REFERENCES Profile (id),
    idCourse VARCHAR(100) NOT NULL REFERENCES Course (id),
    idScore INT REFERENCES Score (id),
    note VARCHAR(1000)
);

CREATE TABLE RegisterCourse (
    id SERIAL PRIMARY KEY,
    idCourse VARCHAR(100) REFERENCES Course(id),
    idProfile VARCHAR(100) REFERENCES Profile(id)
);

CREATE TABLE RegistrationPeriod (
    id SERIAL PRIMARY KEY,
    startTime TIMESTAMP,
    endTime TIMESTAMP,
    CHECK (endTime > startTime)
);

-- Store procedure 

CREATE OR REPLACE FUNCTION "Login"(_username VARCHAR(100))
RETURNS TABLE("id" VARCHAR(100), "password" VARCHAR(1000), "role" VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY 
    SELECT Profile.id as "MSSV" ,  Account.password as "Password", Account.role as "Role"
    FROM Account, Profile, UserAcc
    WHERE   Account.username = UserAcc.idAccount AND
            UserAcc.idProfile = Profile.id AND
            username = _username;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM "Login"('student1');

CREATE OR REPLACE FUNCTION LoadProfileById(_id VARCHAR(100))
RETURNS TABLE("MSSV" VARCHAR(100), "Tên" VARCHAR(100), "Ngày sinh" TIMESTAMP, "Giới tính" VARCHAR(100), "Bậc đào tạo" VARCHAR(100), "Hệ đào tạo" VARCHAR(100), "Ảnh đại diện" BYTEA) AS $$
BEGIN
    RETURN QUERY
    SELECT id as "MSSV",
           name as "Tên",
           birthday as "Ngày sinh",
           gender as "Giới tính",
           level as "Bậc đào tạo",
           trainingSystem as "Hệ đào tạo",
           avatar as "Ảnh đại diện"
    FROM Profile
    WHERE id = _id;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM LoadProfileById('21521601');

CREATE OR REPLACE FUNCTION GetScheduleByID(_id VARCHAR(100))
RETURNS TABLE("Mã môn học" VARCHAR(100), "Tên môn học" VARCHAR(100), "Phòng học" VARCHAR(100), "Ngày bắt đầu" DATE, "Ngày kết thúc" DATE, "Thứ" VARCHAR(100), "Tiết" VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY
    SELECT Course.id as "Mã môn học",
           Course.name as "Tên môn học",
           Course.classroom as "Phòng học",
           Course.startDay as "Ngày bắt đầu",
           Course.endDay as "Ngày kết thúc",
           Course.schoolDay as "Thứ",
           Course.lesson as "Tiết"
    FROM Schedule, Course
    WHERE Schedule.idProfile = _id
        AND Schedule.idCourse = Course.id

    -- Sắp xếp tăng dần để vẽ thời khóa biểu 

	ORDER BY Course.schoolDay ASC, 
            Course.lesson ASC; 
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetScheduleByID('21521601');

CREATE OR REPLACE FUNCTION GetListRegisterCourse()
RETURNS TABLE(
	"Mã lớp" VARCHAR(100),
    "Tên môn học" VARCHAR(100),
    "Mã giảng viên" VARCHAR(100),
    "Tên giảng viên" VARCHAR(100),
    "Số tín chỉ" INT,
    "Thứ" VARCHAR(100),
    "Tiết" VARCHAR(100),
    "Phòng" VARCHAR(100),
    "Học kì" VARCHAR(100),
    "Năm học" VARCHAR(100),
    "Ngày bắt đầu" DATE,
    "Ngày kết thúc" DATE
) AS $$
BEGIN

    -- Nếu quá thời hạn đăng kí học phần sẽ không load được kết quả
    IF CURRENT_TIMESTAMP > (SELECT endTime FROM RegistrationPeriod ORDER BY id DESC LIMIT 1) THEN
        RETURN;
    ELSE
        RETURN QUERY
        SELECT Course.id as "Mã lớp",
			   Course.name as "Tên môn học",
               Profile.id as "Mã giảng viên",
               Profile.name as "Tên giảng viên",
               Course.numberOfCredits as "Số tín chỉ",
               Course.schoolDay as "Thứ",
               Course.lesson as "Tiết",
               Course.classroom as "Phòng",
               Course.semester as "Học kì",
               Course.schoolYear as "Năm học",
               Course.startDay as "Ngày bắt đầu",
               Course.endDay as "Ngày kết thúc"
        FROM Schedule
        JOIN Course ON Schedule.idCourse = Course.id
        JOIN Profile ON Schedule.idProfile = Profile.id
        JOIN UserAcc ON Profile.id = UserAcc.idProfile
        JOIN Account ON UserAcc.idAccount = Account.username
        WHERE Account.role = 'teacher'
        ORDER BY Course.name, Course.id;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetListRegisterCourse();

CREATE OR REPLACE FUNCTION GetLearningOutcomes(_id VARCHAR(100))
RETURNS TABLE("Mã học phần" VARCHAR(100), "Tên học phần" VARCHAR(100), "Tín chỉ" INT, "Điểm quá trình" FLOAT, "Điểm giữa kì" FLOAT, "Điểm thực hành" FLOAT, "Điểm cuối kì" FLOAT, "Điểm học phần" FLOAT, "Học kì" VARCHAR(100), "Năm học" VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY
    SELECT Course.id as "Mã học phần",
           Course.name as "Tên học phần",
           Course.numberOfCredits as "Tín chỉ",
           Score.processScore as "Điểm quá trình",
           Score.midtermScore as "Điểm giữa kì",
           Score.practiceScore as "Điểm thực hành",
           Score.finalScore as "Điểm cuối kì",
		   cast((Score.processScore * Score.ratioProcess 
               + Score.midtermScore * Score.ratioMidterm 
               + Score.practiceScore * Score.ratioPractice
               + Score.finalScore * Score.ratioFinal) as numeric(10, 2))::float as "Điểm học phần",
           Course.semester as "Học kì",
           Course.schoolYear as "Năm học"
    FROM Schedule
    JOIN Course ON Schedule.idCourse = Course.id
    JOIN Score ON Schedule.idScore = Score.id
    WHERE Schedule.idProfile = _id;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetLearningOutcomes('21521601');

CREATE OR REPLACE FUNCTION GetClassInCharge(_id VARCHAR(100))
RETURNS TABLE("Mã môn học" VARCHAR(100), "Tên môn học" VARCHAR(100), "Phòng học" VARCHAR(100), "Ngày bắt đầu" DATE, "Ngày kết thúc" DATE, "Thứ" VARCHAR(100), "Tiết" VARCHAR(100), "SLSV" BIGINT, "Ghi chú" VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY
    SELECT Course.id as "Mã môn học",
           Course.name as "Tên môn học",
		   Course.classroom as "Phòng học",
           Course.startDay as "Ngày bắt đầu",
           Course.endDay as "Ngày kết thúc",
           Course.schoolDay as "Thứ",
           Course.lesson as "Tiết",
           COUNT(*) as "SLSV",
           Schedule.note as "Ghi chú"
    FROM Schedule
    JOIN Course ON Schedule.idCourse = Course.id
    WHERE NOT Schedule.idProfile = _id
      AND Schedule.idCourse IN (
        SELECT Course.id
        FROM Schedule
        JOIN Course ON Schedule.idCourse = Course.id
        WHERE Schedule.idProfile = _id
      )
    GROUP BY Course.id, Course.name, Course.schoolDay, Course.lesson, Schedule.note, Course.classroom, Course.startDay, Course.endDay;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetClassInCharge('GV1');

CREATE OR REPLACE FUNCTION GetListClass(_idCourse VARCHAR(100))
RETURNS TABLE("MSSV" VARCHAR(100), "Tên sinh viên" VARCHAR(100), "Điểm quá trình" FLOAT, "Điểm giữa kì" FLOAT, "Điểm thực hành" FLOAT, "Điểm cuối kì" FLOAT, "Điểm học phần" FLOAT) AS $$
BEGIN
    RETURN QUERY
    SELECT Profile.id as "MSSV",
           Profile.name as "Tên sinh viên",
           Score.processScore as "Điểm quá trình",
           Score.midtermScore as "Điểm giữa kì",
           Score.practiceScore as "Điểm thực hành",
           Score.finalScore as "Điểm cuối kì",
           cast((Score.processScore * Score.ratioProcess 
               + Score.midtermScore * Score.ratioMidterm 
               + Score.practiceScore * Score.ratioPractice
               + Score.finalScore * Score.ratioFinal) as numeric(10, 2))::float as "Điểm học phần"
    FROM Schedule
    JOIN Profile ON Schedule.idProfile = Profile.id
    JOIN Score ON Schedule.idScore = Score.id
    WHERE Schedule.idCourse = _idCourse
    ORDER BY Profile.id;
END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetListClass('IT004.O14');

CREATE OR REPLACE FUNCTION GetListRegisteredByID(
    IN v_id VARCHAR(100)
)
RETURNS TABLE("Tên môn học" VARCHAR(100), "Mã lớp" VARCHAR(100), "Tên giảng viên" VARCHAR(100), "Số tín chỉ" INT, "Thứ" VARCHAR(100), "Tiết" VARCHAR(100), "Phòng" VARCHAR(100), "Học kì" VARCHAR(100), "Năm học" VARCHAR(100), "Ngày bắt đầu" DATE, "Ngày kết thúc" DATE) AS $$
BEGIN
    RETURN QUERY
    SELECT 
		"GLRC"."Tên môn học",
		"GLRC"."Mã lớp",
		"GLRC"."Tên giảng viên",
		"GLRC"."Số tín chỉ",
		"GLRC"."Thứ",
		"GLRC"."Tiết",
		"GLRC"."Phòng",
		"GLRC"."Học kì",
		"GLRC"."Năm học",
		"GLRC"."Ngày bắt đầu",
		"GLRC"."Ngày kết thúc"
	FROM GetListRegisterCourse() AS "GLRC"
	JOIN RegisterCourse ON RegisterCourse.idCourse = "GLRC"."Mã lớp"
	WHERE
        RegisterCourse.idProfile = v_id
    ORDER BY
        "GLRC"."Thứ" ASC,
		"GLRC"."Tiết" ASC; 

END;
$$ LANGUAGE plpgsql;

-- SELECT * FROM GetListRegisteredByID('21521601');

CREATE OR REPLACE FUNCTION GetListRegistrationPeriod()
RETURNS TABLE("Bắt đầu đăng kí học phần" TIMESTAMP, "Kết thúc đăng kí học phần" TIMESTAMP) AS $$
BEGIN
    RETURN QUERY 
    SELECT startTime AS "Bắt đầu đăng kí học phần", endTime AS "Kết thúc đăng kí học phần"
	FROM RegistrationPeriod
    ORDER BY endTime DESC;
    
END;
$$ LANGUAGE plpgsql;

-- select * from GetListRegistrationPeriod();

create or replace function getListStudents()
returns table (
	"MSSV" varchar(100),
	"Họ tên" varchar(100)
) as $$
BEGIN 
	return query 
	select 
		profile.id as "MSSV", 
		profile.name as "Họ tên" 
	from profile, useracc, account
	where useracc.idprofile = profile.id
		and useracc.idaccount = account.username
		and account."role" = 'student';
end;
$$ language plpgsql;

-- select * from getListStudents()

-------------------------
------ CRUD -----------
CREATE OR REPLACE FUNCTION InsertAcc(
    IN v_username VARCHAR(100),
    IN v_password VARCHAR(1000),
    IN v_id VARCHAR(100)
)
RETURNS Bool AS $$
BEGIN
    -- Check username exist 

    IF (SELECT COUNT(*) FROM Account WHERE username = v_username) > 0 THEN
        RETURN false;
    END IF;

    -- Check id exist 

    IF (SELECT COUNT(*) FROM Profile WHERE id = v_id) > 0 THEN
        RETURN false;
    END IF;

    INSERT INTO Account(username, password)
    VALUES (v_username, v_password);

    INSERT INTO Profile(id)
    VALUES (v_id);

    INSERT INTO UserAcc(idAccount, idProfile)
    VALUES (v_username, v_id);
	
    RETURN true;
END;
$$ LANGUAGE plpgsql;

-- SELECT InsertAcc('student11', '123456', '21521611');

CREATE OR REPLACE FUNCTION insertRegisterCourse(
    IN courseId VARCHAR(100),
    IN courseName VARCHAR(100),
    IN profileId VARCHAR(100),
    IN profileName VARCHAR(100),
    IN courseNumberOfCredits INT,
    IN courseSchoolDay VARCHAR(100),
    IN courseLesson VARCHAR(100),
    IN courseClassroom VARCHAR(100),
    IN courseSemester VARCHAR(100),
    IN courseSchoolYear VARCHAR(100),
    IN courseStartDay DATE,
    IN courseEndDay DATE
)
RETURNS BOOLEAN AS $$
DECLARE
    lastEndTime TIMESTAMP;
    lastStartTime TIMESTAMP;
BEGIN
    -- Không được sửa danh sách dkhp khi đăng mở đăng kí 
    SELECT starttime, endtime INTO lastStartTime, lastEndTime FROM RegistrationPeriod ORDER BY starttime DESC LIMIT 1;

    IF (lastStartTime <= CURRENT_TIMESTAMP) AND (lastEndTime > CURRENT_TIMESTAMP) THEN 
        RETURN FALSE;
    END IF;
	
	
	INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) 
	VALUES (courseId, courseName, courseNumberOfCredits, courseSchoolDay, courseLesson, courseClassroom, courseSemester, courseSchoolYear, courseStartDay, courseEndDay);

	INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES (profileId, courseId, NULL, NULL);

    UPDATE Profile
    SET
        name = profileName
    WHERE
        id = profileId;

    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT insertRegisterCourse('IT003.O11', 'Cấu trúc dữ liệu và giải thuật', 'GV2', 'Trần Khắc Việt', 3, '3', '1234', 'C312', 'HK1', '2023-2024', '2023-09-11', '2024-01-06')


CREATE OR REPLACE FUNCTION UpdatePass(_username VARCHAR(100), _password VARCHAR(1000))
RETURNS bool AS $$
BEGIN
    UPDATE Account
    SET password = _password
    WHERE username = _username;
	return true;
END;
$$ LANGUAGE plpgsql;

-- SELECT UpdatePass('student1', '654321');

CREATE OR REPLACE FUNCTION UpdateProfile(_id VARCHAR(100), _name VARCHAR(100), _birthday TIMESTAMP, _gender VARCHAR(100), _level VARCHAR(100), _trainingSystem VARCHAR(100), _avatar BYTEA)
RETURNS bool AS $$
BEGIN
 	IF _birthday >= NOW() THEN
        RETURN false;
    END IF;
	
    UPDATE Profile
    SET name = _name,
        birthday = _birthday,
        gender = _gender,
        level = _level,
        trainingsystem = _trainingSystem,
        avatar = _avatar
    WHERE id = _id;
	return true;
END;
$$ LANGUAGE plpgsql;

-- SELECT UpdateProfile('21521601', 'Học sinh 1', '2000-01-01', 'Nam', 'Đại học', 'Chính quy', NULL);

CREATE OR REPLACE FUNCTION UpdateScore(
    IN v_idCourse VARCHAR(100),
    IN v_idProfile VARCHAR(100),
    IN v_processScore FLOAT,
    IN v_midtermScore FLOAT,
    IN v_finalScore FLOAT,
    IN v_practiceScore FLOAT
)
RETURNS BOOLEAN AS $$
BEGIN
    IF (
        v_processScore < 0 OR v_processScore > 10 or
        v_midtermScore < 0 OR v_midtermScore > 10 or
        v_finalScore < 0 OR v_finalScore > 10 or
        v_practiceScore < 0 or v_practiceScore > 10
    ) THEN
        RETURN FALSE;
    END IF;

    UPDATE Score AS s
    SET processScore = v_processScore,
        midtermScore = v_midtermScore,
        finalScore = v_finalScore,
        practiceScore = v_practiceScore
    FROM Schedule AS sc
    WHERE sc.idProfile = v_idProfile
        AND sc.idCourse = v_idCourse
        AND sc.idScore = s.id;

    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT UpdateScore('IT006.O14', '21521601', 8.3, null, 9.2, null);


CREATE OR REPLACE FUNCTION UpdateRatioScore(_idCourse VARCHAR(100), _ratioProcess FLOAT, _ratioMidterm FLOAT, _ratioFinal FLOAT, _ratioPractice FLOAT)
RETURNS bool AS $$
BEGIN
    IF _ratioProcess > 1 OR _ratioMidterm > 1 OR _ratioFinal > 1 OR _ratioPractice > 1 THEN
        RETURN false;
    END IF;

    UPDATE Score AS S
    SET ratioProcess = _ratioProcess,
        ratioMidterm = _ratioMidterm,
        ratioFinal = _ratioFinal,
        ratioPractice = _ratioPractice
    FROM Schedule AS Sch
    WHERE Sch.idCourse = _idCourse
        AND Sch.idScore = S.id;
	return true;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION updateRegistrationPeriod(p_startTime TIMESTAMP, p_endTime TIMESTAMP)
RETURNS BOOL AS $$
DECLARE
    lastEndTime TIMESTAMP;
    lastStartTime TIMESTAMP;
BEGIN

	IF p_endTime <= p_startTime THEN
        RETURN FALSE;
    END IF;

    IF p_endTime < CURRENT_TIMESTAMP THEN
        RETURN FALSE;
    END IF;

    UPDATE RegistrationPeriod
    SET startTime = p_startTime, endTime = p_endTime
    WHERE id = (SELECT id FROM RegistrationPeriod ORDER BY endtime DESC LIMIT 1);

    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- select updateRegistrationPeriod('2023-12-24', '2023-12-25')

CREATE OR REPLACE FUNCTION updateRegisterCourse(
    IN courseId VARCHAR(100),
    IN courseName VARCHAR(100),
    IN profileId VARCHAR(100),
    IN profileName VARCHAR(100),
    IN courseNumberOfCredits INT,
    IN courseSchoolDay VARCHAR(100),
    IN courseLesson VARCHAR(100),
    IN courseClassroom VARCHAR(100),
    IN courseSemester VARCHAR(100),
    IN courseSchoolYear VARCHAR(100),
    IN courseStartDay DATE,
    IN courseEndDay DATE
)
RETURNS BOOLEAN AS $$
DECLARE
    lastEndTime TIMESTAMP;
    lastStartTime TIMESTAMP;
BEGIN
    SELECT starttime, endtime INTO lastStartTime, lastEndTime FROM RegistrationPeriod ORDER BY starttime DESC LIMIT 1;

    -- Không được sửa danh sách dkhp khi đăng mở đăng kí 

    IF (lastStartTime <= CURRENT_TIMESTAMP) AND (lastEndTime > CURRENT_TIMESTAMP) THEN 
        RETURN FALSE;
    END IF;

    UPDATE Course
    SET
        name = courseName,
        numberOfCredits = courseNumberOfCredits,
        schoolDay = courseSchoolDay,
        lesson = courseLesson,
        classroom = courseClassroom,
        semester = courseSemester,
        schoolYear = courseSchoolYear,
        startDay = courseStartDay,
        endDay = courseEndDay
    WHERE
        id = courseId;

    UPDATE Profile
    SET
        name = profileName
    WHERE
        id = profileId;

    RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT updateregistercourse('IT003.O11', 'Cấu trúc dữ liệu và giải thuật 2', 'GV2', 'Trần Khắc Việt 2', 4, '3', '1234', 'C312', 'HK1', '2023-2024', '2023-09-11', '2024-01-06')


-- SELECT UpdateRatioScore('IT006.O14', 0.2, 0.3, 0.5, 0);

CREATE OR REPLACE FUNCTION AcceptCourse(
    IN v_idProfile VARCHAR(100),
    IN v_idCourse VARCHAR(100)
)
RETURNS BOOL AS $$
DECLARE
    v_idScore INT;
BEGIN
    -- Không được đăng ký môn ko có trong danh sách các môn được mở
    IF (SELECT COUNT(*) FROM Course WHERE id = v_idCourse) = 0 THEN
        RETURN FALSE;
    END IF;

    -- Không được đăng kí 1 môn nhiều lần
    IF (SELECT COUNT(*) FROM Schedule WHERE idProfile = v_idProfile AND idCourse = v_idCourse) > 0 THEN
        RETURN FALSE;
    END IF;
	
	-- Không được đăng kí trùng môn khác ngày học ví dụ IT003.O11 và IT003.O12
	IF (SELECT COUNT(*) 
		FROM Schedule 
		WHERE idProfile = v_idProfile 
		AND substring(idCourse from 1 for position('.' in idCourse)-1) = substring(v_idCourse from 1 for position('.' in v_idCourse)-1)
	) > 0 THEN
        RETURN FALSE;
    END IF;

    -- Các môn học không được trùng lịch học
    IF (SELECT COUNT(*) FROM
        (SELECT schoolDay, lesson FROM Course WHERE id = v_idCourse) AS infoCourse,
        (SELECT schoolDay, lesson FROM Schedule, Course WHERE Schedule.idCourse = Course.id AND Schedule.idProfile = v_idProfile) AS allInfoCourse
        WHERE infoCourse.schoolDay = allInfoCourse.schoolDay AND
            (infoCourse.lesson LIKE '%' || allInfoCourse.lesson || '%' OR
                allInfoCourse.lesson LIKE '%' || infoCourse.lesson || '%')) > 0 THEN
        RETURN FALSE;
    END IF;

    -- Khi sinh viên đã tham gia lớp học thì phải có bản điểm
    INSERT INTO Score(processScore)
    VALUES (NULL)
    RETURNING id INTO v_idScore;

    INSERT INTO Schedule (idCourse, idProfile, idScore)
    VALUES (v_idCourse, v_idProfile, v_idScore);
	RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT AcceptCourse('21521601', 'IT006.O14');

CREATE OR REPLACE FUNCTION RejectCourse(_idProfile VARCHAR(100), _idCourse VARCHAR(100))
RETURNS bool AS $$
DECLARE
    _idScore INT;
BEGIN
    SELECT INTO _idScore Score.id FROM Score, Schedule
    WHERE Schedule.idScore = Score.id
        AND Schedule.idProfile = _idProfile
        AND Schedule.idCourse = _idCourse;
		
	IF _idScore IS NULL THEN
        RETURN false;
    END IF;

    -- Check if any scores are not null
    IF EXISTS (
        SELECT 1
        FROM Score
        WHERE id = _idScore
            AND (processScore IS NOT NULL OR midtermScore IS NOT NULL OR finalScore IS NOT NULL OR practiceScore IS NOT NULL)
    ) THEN
        RETURN false;
    END IF;

    -- Delete from Schedule and Score tables
    DELETE FROM Schedule
    WHERE Schedule.idScore = _idScore;

    DELETE FROM Score
    WHERE id = _idScore;

    RETURN true;
END;
$$ LANGUAGE plpgsql;

-- SELECT RejectCourse('21521601', 'IT006.O14');

CREATE OR REPLACE FUNCTION JoinRegisterCourse(
    IN v_idProfile VARCHAR(100),
    IN v_idCourse VARCHAR(100)
)
RETURNS BOOL AS $$
BEGIN
    -- Không được đăng ký môn ko có trong danh sách các môn được mở
    IF (SELECT COUNT(*) FROM Course WHERE id = v_idCourse) = 0 THEN
        RETURN FALSE;
    END IF;

    -- Không được đăng kí 1 môn nhiều lần
    IF (SELECT COUNT(*) FROM RegisterCourse WHERE idCourse = v_idCourse AND idProfile = v_idProfile) > 0 THEN
        RETURN FALSE;
    END IF;
	
	-- Không được đăng kí trùng môn khác ngày học ví dụ IT003.O11 và IT003.O12
	IF (SELECT COUNT(*) 
		FROM RegisterCourse 
		WHERE idProfile = v_idProfile 
		AND substring(idCourse from 1 for position('.' in idCourse)-1) = substring(v_idCourse from 1 for position('.' in v_idCourse)-1)
	) > 0 THEN
        RETURN FALSE;
    END IF;
	
    -- Các môn học không được trùng lịch học
    IF (SELECT COUNT(*) FROM
        (SELECT Course.schoolDay, Course.lesson FROM Course WHERE id = v_idCourse) AS infoCourse,
        (SELECT Course.schoolDay, Course.lesson FROM RegisterCourse, Course WHERE RegisterCourse.idCourse = Course.id AND RegisterCourse.idProfile = v_idProfile) AS allInfoCourse
        WHERE allInfoCourse.schoolDay = infoCourse.schoolDay AND
            (allInfoCourse.lesson LIKE '%' || infoCourse.lesson || '%' OR
                infoCourse.lesson LIKE '%' || allInfoCourse.lesson || '%'
            )
        ) > 0 THEN
        RETURN FALSE;
    END IF;

    INSERT INTO RegisterCourse(idCourse, idProfile)
    VALUES(v_idCourse, v_idProfile);

	RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT JoinRegisterCourse('21521601', 'IT006.O14');

CREATE OR REPLACE FUNCTION LeaveRegisterCourse(
    IN v_idProfile VARCHAR(100),
    IN v_idCourse VARCHAR(100)
)
RETURNS bool AS $$
BEGIN
    DELETE FROM RegisterCourse
    WHERE idCourse = v_idCourse AND idProfile = v_idProfile;
	return true;
END;
$$ LANGUAGE plpgsql;

-- SELECT LeaveRegisterCourse('21521601', 'IT006.O14');

CREATE OR REPLACE FUNCTION clearRegisterCourse()
RETURNS VOID 
AS $$
BEGIN
	delete from Schedule;
	delete from RegisterCourse;
	delete from Score;
	delete from Course;
END;
$$ LANGUAGE plpgsql;

-- SELECT FROM clearRegisterCourse()



-- ===============================================================
-- =========================Insert data ==========================
-- ===============================================================



INSERT INTO public.account (username, password, role) VALUES ('admin', '$2a$12$dHT/7Q//H1zIiy6NlOtWu.pNw8IvxoWfx6qERkLH1YyKNZ81YMyve', 'admin');
INSERT INTO public.account (username, password, role) VALUES ('gv1', '$2a$12$NqGrVo//fTzgGnZGmWM2B.62eLDiuni56FSCb14l1XvlXEIo5KyMG', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('gv2', '$2a$12$IigUmWdfupuWId/QdAZV3.bNzDY6FEPDrBRXpvvKoq26.C.bWUYJS', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('gv3', '$2a$12$2T47o0UFqks4MzhYTYzuHObfOIidbLdPU9.61KYBOYbFmvoZss7v2', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('gv4', '$2a$12$2E8BpuvE2sfLPLfEnEe/bODy2s26qnyN4tKIpOHkULc1UVtVTrfZy', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('gv5', '$2a$12$y2TmA2I9sEg7zXVbXTBraOeU7huBjfIQssQJCS1lLS0RtERrYtQR.', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('gv6', '$2a$12$3eqQzimIRQ.BKg8Leg5.QOxLnbrm5/0T6NzkbTvWPPdghb.Iwpln2', 'teacher');
INSERT INTO public.account (username, password, role) VALUES ('sv21521601', '$2a$12$gXoeN8at8VUNkKR4//xDPO3W0R7RY9VoHqaXXQSw7IYuVY.wfxxbG', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521602', '$2a$12$tREEkrFfYhiaiN9GAo12m.FJaaVlN5cUMj9pyDiCxiomOgV/rrtRO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521603', '$2a$12$4nmNygxQqQNCpbQ0.fjBceCOtIfyQbqgrnhJXJdPOB447Gwimq9NW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521604', '$2a$12$XONx1lY8JOhp321UN3cY0.2.Fb.CpSw0vGhsSU666cegfN5kmRNzy', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521605', '$2a$12$67lhqVpONj1GtAAPkDdIJO7vvjQxuNcn8BKwjkXY4UuEhhD.ODM/u', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521606', '$2a$12$xWMnTX8CwxJk2E/Tzy7REOxbv1NK9slM9.CBcPJaQ34OypmOD2gkq', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521607', '$2a$12$gGM7zXBVU4lBn27ZPX3DBOcVunloc0Zv2q4eShBquRIQB84lOKyii', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521608', '$2a$12$RgxStxKyvDUvpES1HItL9uofbGRV3N2OYnJf2RCXzqMrJCy5oPrWC', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521609', '$2a$12$a3MCYyoeFkzgeP3nkViq9OD9xa4ekLowUa5.gxjjv1LUAhrTqBywu', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521610', '$2a$12$M7GwCFvJzTrm8mBUK.bDUeiAFI6uOLKRjJXnrV/dGFgLqqNX1OdIq', 'student');


INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521601', 'Trần Thế Sơn', '2004-05-07 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521602', 'Võ Khánh Hằng', '2004-06-06 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521603', 'Đỗ Ngọc Khôi', '2004-05-22 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521604', 'Nguyễn Ngọc Anh', '2004-01-22 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521605', 'Nguyễn Thúy Huyền', '2004-05-01 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521606', 'Lê Xuân An', '2004-09-08 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521607', 'Trần Nguyệt Ánh', '2004-03-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521608', 'Võ Ðức Tuệ', '2004-07-19 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521609', 'Đặng Ánh Thơ', '2004-03-13 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521610', 'Trần Thanh Phong', '2004-07-28 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV1', 'Nguyễn Thu Nga', '1980-08-02 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV2', 'Trần Khắc Việt', '1986-01-14 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV3', 'Trần Chí Khang', '1990-11-12 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV4', 'Dương Ngọc Huyền', '1990-07-18 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV5', 'Đỗ Tiến Ðức', '1988-08-08 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('GV6', 'Dương Xuân Hiền', '1991-07-18 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('QL', 'Phạm Hồng Oanh', '1976-12-03 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);


INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521601', '21521601');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521602', '21521602');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521603', '21521603');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521604', '21521604');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521605', '21521605');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521606', '21521606');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521607', '21521607');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521608', '21521608');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521609', '21521609');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('sv21521610', '21521610');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv1', 'GV1');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv2', 'GV2');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv3', 'GV3');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv4', 'GV4');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv5', 'GV5');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('gv6', 'GV6');
INSERT INTO public.useracc (idaccount, idprofile) VALUES ('admin', 'QL');


INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT004.O17', 'Cơ sở dữ liệu', 3, '3', '1234', 'B3.12', 'HK1', '2023-2024', '2023-09-11', '2023-12-02');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT004.O14', 'Cơ sở dữ liệu', 3, '2', '6789', 'B3.16', 'HK1', '2023-2024', '2023-09-11', '2023-12-02');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT004.O15', 'Cơ sở dữ liệu', 3, '2', '6789', 'B3.18', 'HK1', '2023-2024', '2023-09-11', '2023-12-02');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT004.O18', 'Cơ sở dữ liệu', 3, '3', '6789', 'B4.18', 'HK1', '2023-2024', '2023-09-11', '2023-12-02');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT003.O13', 'Cấu trúc dữ liệu và giải thuật', 3, '6', '678', 'B3.18', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT003.O14', 'Cấu trúc dữ liệu và giải thuật', 3, '3', '6789', 'B6.06', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT003.O12', 'Cấu trúc dữ liệu và giải thuật', 3, '4', '678', 'C106', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT003.O11', 'Cấu trúc dữ liệu và giải thuật', 3, '3', '1234', 'C314', 'HK1', '2023-2024', '2023-09-11', '2024-01-06');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA006.O11', 'Giải tích', 4, '4', '12345', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2024-01-06');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA006.O14', 'Giải tích', 4, '4', '67890', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2024-01-06');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA006.O15', 'Giải tích', 4, '2', '12345', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2024-01-06');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA006.O18', 'Giải tích', 4, '2', '67890', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2024-01-06');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT002.O11', 'Lập trình hướng đối tượng', 3, '6', '1234', 'B3.16', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT002.O12', 'Lập trình hướng đối tượng', 3, '6', '1234', 'B3.18', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT006.O16', 'Kiến trúc máy tính', 3, '2', '678', 'B4.20', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT006.O14', 'Kiến trúc máy tính', 3, '2', '345', 'B5.08', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA003.O12', 'Đại số tuyến tính', 3, '6', '6789', 'B1.14', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA003.O11', 'Đại số tuyến tính', 3, '3', '1234', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA003.O14', 'Đại số tuyến tính', 3, '5', '1234', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('MA003.O15', 'Đại số tuyến tính', 3, '5', '6789', 'B1.16', 'HK1', '2023-2024', '2023-09-11', '2023-12-30');
INSERT INTO public.course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) VALUES ('IT004.O13', 'Cơ sở dữ liệu', 3, '6', '1234', 'B1.14', 'HK1', '2023-2024', '2023-09-11', '2023-12-02');



INSERT INTO public.registrationperiod (id, starttime, endtime) VALUES (2, '2023-12-14 00:00:00', '2024-12-31 00:00:00');



INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV1', 'IT004.O13', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV1', 'IT004.O17', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV1', 'IT004.O14', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV1', 'IT003.O14', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV1', 'IT003.O12', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV2', 'IT004.O15', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV2', 'IT004.O18', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV2', 'IT003.O13', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV2', 'IT003.O11', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV3', 'MA006.O14', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV3', 'MA006.O18', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV3', 'MA003.O12', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV3', 'MA003.O15', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV4', 'IT002.O12', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV4', 'IT006.O16', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV5', 'IT002.O11', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV5', 'IT006.O14', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV6', 'MA006.O11', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV6', 'MA006.O15', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV6', 'MA003.O11', NULL, NULL);
INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('GV6', 'MA003.O14', NULL, NULL);


----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
--------------------------------- User action ------------------------------------------------------
----------------------------------------------------------------------------------------------------

-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O11', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O11', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521602');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O12', '21521602');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O14', '21521602');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O14', '21521602');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O15', '21521603');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O14', '21521603');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O15', '21521603');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O13', '21521603');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O18', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O15', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O18', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O12', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521605');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O15', '21521605');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O15', '21521605');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O13', '21521605');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O12', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O17', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O14', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O11', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O11', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O13', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O15', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O11', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O11', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O18', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O15', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521609');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O14', '21521609');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521609');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O14', '21521609');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O11', '21521609');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O16', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA006.O14', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('MA003.O11', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O18', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O13', '21521610');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O14', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT003.O12', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT004.O14', '21521601');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521602');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521603');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O11', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O14', '21521604');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521605');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O11', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O16', '21521606');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O12', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O14', '21521607');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT002.O11', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O14', '21521608');
-- INSERT INTO public.registercourse (idcourse, idprofile) VALUES ('IT006.O16', '21521609');



-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
-- INSERT INTO public.score (processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);



-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'IT006.O14', 1, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'IT004.O14', 2, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'MA003.O11', 3, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'MA006.O11', 4, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'IT003.O12', 5, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521601', 'IT002.O12', 6, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521602', 'IT004.O14', 7, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521602', 'IT003.O14', 8, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521602', 'MA006.O14', 9, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521602', 'IT002.O12', 10, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521602', 'MA003.O12', 11, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521603', 'MA006.O15', 12, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521603', 'IT004.O15', 13, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521603', 'MA003.O14', 14, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521603', 'IT002.O12', 15, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521603', 'IT003.O13', 16, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'IT006.O14', 17, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'MA006.O18', 18, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'IT004.O18', 19, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'IT003.O12', 20, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'MA003.O15', 21, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521604', 'IT002.O11', 22, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521605', 'IT004.O15', 23, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521605', 'MA006.O14', 24, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521605', 'MA003.O15', 25, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521605', 'IT002.O12', 26, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521605', 'IT003.O13', 27, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'IT006.O16', 28, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'IT004.O17', 29, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'IT003.O14', 30, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'MA006.O14', 31, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'IT002.O11', 32, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521606', 'MA003.O12', 33, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'IT006.O14', 34, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'IT004.O15', 35, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'MA003.O11', 36, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'MA006.O11', 37, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'IT002.O12', 38, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521607', 'IT003.O13', 39, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'IT006.O14', 40, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'MA003.O11', 41, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'IT004.O18', 42, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'MA006.O11', 43, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'MA006.O14', 44, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'MA003.O15', 45, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521608', 'IT002.O11', 46, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'IT006.O14', 47, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'IT006.O16', 48, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'IT003.O11', 49, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'MA006.O14', 50, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'MA003.O14', 51, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521609', 'IT002.O12', 52, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'IT006.O16', 53, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'MA003.O11', 54, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'IT004.O18', 55, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'MA006.O14', 56, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'IT002.O12', 57, NULL);
-- INSERT INTO public.schedule (idprofile, idcourse, idscore, note) VALUES ('21521610', 'IT003.O13', 58, NULL);
