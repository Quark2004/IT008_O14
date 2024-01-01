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

CREATE OR REPLACE FUNCTION GetUnregisteredListById(_profileId varchar(100))
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
	return query
	select * from GetListRegisterCourse() as lrc
	where substring(lrc."Mã lớp" from 1 for position('.' in lrc."Mã lớp")-1) 
		not in 
			(select substring(lr."Mã lớp" from 1 for position('.' in lr."Mã lớp")-1) FROM GetListRegisteredByID(_profileId) as lr);
END;
$$ LANGUAGE plpgsql;

-- select * from GetUnregisteredListById('21521602');

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

create or replace function getListCourse() 
returns table (
	"Mã lớp" varchar(100),
	"Tên môn học" varchar(100)
) as $$
begin
	return query
	select 
		course.id as "Mã lớp", 
		course.name as "Tên môn học"
	from course
	where course.id in (
		select schedule.idcourse from Schedule, score
		where Schedule.idscore = score.id 
	)
	order by course.id;
end;
$$ LANGUAGE plpgsql;

-- select * from getListCourse(); 


CREATE OR REPLACE FUNCTION getListProfilesByCourseId(_idCourse VARCHAR(100))
RETURNS TABLE (
    "MSSV/MGV" VARCHAR(100),
    "Họ tên" VARCHAR(100)
) AS $$
BEGIN 
    RETURN QUERY 
    SELECT profile.id AS "MSSV/MGV", profile.name AS "Họ tên"
    FROM profile, schedule
    WHERE schedule.idcourse = _idCourse
    AND schedule.idProfile = profile.id
    ORDER BY profile.id;
END;
$$ LANGUAGE plpgsql;

-- select * from getListProfilesByCourseId('IT002.O11');

create or replace function getListAccounts() 
returns table (
	"Tên đăng nhập" varchar(100),
	"MSSV/MGV" varchar(100),
	"Vai trò" varchar(100)
) as $$
begin 
	return query
	select account.username as "Tên đăng nhập", useracc.idprofile as "MSSV/MGV", account.role as "Vai trò" from account, useracc
	where account.username = useracc.idaccount 
	and account.role != 'admin'
	order by useracc.idprofile;
end;
$$ LANGUAGE plpgsql;

-- select * from getListAccounts();

CREATE OR REPLACE FUNCTION GetListProfileInfo()
RETURNS TABLE("MSSV/MGV" VARCHAR(100), "Tên" VARCHAR(100), "Ngày sinh" TIMESTAMP, "Giới tính" VARCHAR(100), "Bậc đào tạo" VARCHAR(100), "Hệ đào tạo" VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY
    SELECT id as "MSSV/MGV",
           name as "Tên",
           birthday as "Ngày sinh",
           gender as "Giới tính",
           level as "Bậc đào tạo",
           trainingSystem as "Hệ đào tạo"
    FROM Profile
	order by id;
END;
$$ LANGUAGE plpgsql;
 
--  SELECT * FROM GetListProfileInfo();

create or replace function getRaitoScoreByCourseId(_idCourse varchar(100))
returns table (
	"Mã môn học" varchar(100),
	"Tỉ lệ điểm quá trình" float,
	"Tỉ lệ điểm giữa kì" float,
	"Tỉ lệ điểm thực hành" float,
	"Tỉ lệ điểm cuối kì" float
) as $$
begin
	return query
	select 
		schedule.idcourse as "Mã môn học",
		score.ratioprocess as "Tỉ lệ điểm quá trình", 
		score.ratiomidterm as "Tỉ lệ điểm giữa kì", 
		score.ratiopractice as "Tỉ lệ điểm thực hành", 
		score.ratiofinal as "Tỉ lệ điểm cuối kì" 
	from schedule, score
	where schedule.idscore = score.id
	and schedule.idcourse = _idCourse;
end;
$$ LANGUAGE plpgsql;

-- select * from getRaitoScoreByCourseId('IT003.O12');

-------------------------
------ CRUD -----------
CREATE OR REPLACE FUNCTION InsertAcc(
    IN v_username VARCHAR(100),
    IN v_password VARCHAR(1000),
	IN v_role varchar(100),
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

    INSERT INTO Account(username, password, role)
    VALUES (v_username, v_password, v_role);

    INSERT INTO Profile(id)
    VALUES (v_id);

    INSERT INTO UserAcc(idAccount, idProfile)
    VALUES (v_username, v_id);
	
    RETURN true;
END;
$$ LANGUAGE plpgsql;

--  SELECT InsertAcc('sv21521611', '123456', 'student', '21521611');

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
    IN courseStartDay TIMESTAMP,
    IN courseEndDay TIMESTAMP
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

-- SELECT UpdateRatioScore('IT006.O14', 0.2, 0.3, 0.5, 0);


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

-- select updateRegistrationPeriod('2023-12-25', '2023-12-26')

CREATE OR REPLACE FUNCTION updateRegisterCourse(
		IN courseId VARCHAR(100),
		IN profileId VARCHAR(100),
		IN courseNumberOfCredits INT,
		IN courseSchoolDay VARCHAR(100),
		IN courseLesson VARCHAR(100),
		IN courseClassroom VARCHAR(100),
		IN courseSemester VARCHAR(100),
		IN courseSchoolYear VARCHAR(100),
		IN courseStartDay TIMESTAMP,
		IN courseEndDay TIMESTAMP
	)
RETURNS BOOLEAN AS $$
DECLARE
	lastEndTime TIMESTAMP;
	lastStartTime TIMESTAMP;
BEGIN
	SELECT starttime, endtime INTO lastStartTime, lastEndTime FROM RegistrationPeriod ORDER BY starttime DESC LIMIT 1;

	-- Không được sửa danh sách dkhp khi đăng mở đăng kí 

	IF (lastStartTime <= CURRENT_TIMESTAMP) AND (lastEndTime > CURRENT_TIMESTAMP) THEN 
-- 		Raise exception 'Không được sửa danh sách dkhp khi đăng mở đăng kí ';
		RETURN FALSE;
	END IF;

	-- 	Check trùng lịch dạy của gv
	if (select count(*) from 
			(select "Thứ", "Tiết" from getschedulebyid(profileId)
			where "Mã môn học" != courseId
			and "Thứ" = courseSchoolDay
			and "Tiết"  LIKE '%' || courseLesson || '%')
	   ) > 0 
	 then 
-- 	 	Raise exception 'trùng lịch dạy của gv';
		return false;
	 end if;


	-- check trùng lịch phòng 
	if (
		select count(*) from 
			(select * from course
				where course.id != courseId
				and course.schoolday = courseSchoolDay
				and course.lesson like '%' || courseLesson || '%'
				and course.classroom = courseClassroom)
	) > 0
	then
-- 		Raise exception 'trùng lịch phòng';
		return false;
	end if;

	if courseEndDay <= courseStartDay 
	then return false;
	end if;


	UPDATE Course
	SET
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

	RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

-- SELECT updateregistercourse('IT003.O11', 'GV2', 4, '5', '6789', 'C3.12', 'HK2', '2023-2024', '2023-09-11', '2024-01-06');

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
	
	-- 	Xoá môn đã được accept khỏi danh sách dkhp 
	delete from RegisterCourse
	where idProfile = v_idProfile
		and idCourse = v_idCourse;
	
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
	
	-- Khi bị reject, sẽ xóa môn đó khỏi danh sách đăng kí học phần   	
	DELETE FROM registercourse
	where idcourse = _idCourse 
		AND idprofile = _idProfile;

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
INSERT INTO public.account (username, password, role) VALUES ('sv21521611', '$2a$12$QXZdMA1TAzprLB02IjA8uOKhu.YgJDEJsN7tBdv/MkBbbEkPw./YK', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521612', '$2a$12$GhztCQTHTVjOImRALsXJAeHZUBp4LEagNGwAvEI6TIMnT8UHwutda', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521613', '$2a$12$MA0vLh.9Gh0azX6Wih5ZoeWlGxKC3o0/1ChzBMJVYKtoz1qjyFUKe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521614', '$2a$12$HFbQ4rI/k.XZ/VH.SAaVzuZl4jhZ0WS4W60EocSxe3bB/Ly3uJTi2', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521615', '$2a$12$Gv1QF7QvXOTnlDVDMWzDxuFzGYn369x2VC81qaSCNTqCZf3hq9vIa', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521616', '$2a$12$x5.pvJ1iW9RF8TuF59pNcOZo045uD4AD03SVX94Q9/GippUWXRJ3a', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521617', '$2a$12$Bl6MweXWBJqCZqN2ng45BOhxvui1wNv09WGUDJ8qYvhOUMULDCpS2', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521618', '$2a$12$mgJ1OBpmK3CIk6/EY6aIDOTCGNNPQpI34yVJ4SxlSXaJWFzaLsfnS', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521619', '$2a$12$37itKtRjyCwx0DW3d.grIup7IKmTCGPuJeejRmYo5uMql9O6MXjKi', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521620', '$2a$12$1gAaB.cqNi9cSEI7JiMmOuWrROrxicJNo57vjpYorPC2qb7hnzc2S', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521621', '$2a$12$0Nh6KpUq2nrFEF6IoVAR6uHCKylZTibrn90WTtipp7IyhMdTcCxme', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521622', '$2a$12$yFFldabfUnfS5f29AOKxkO8EgtuFTvvciGf92c2aUnWJs2K.4YMH6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521623', '$2a$12$wIUubkGsWSnbKrPV5Y00KOu6LHtArg85ISanYI9UwpOpYVJYsMzWS', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521624', '$2a$12$wBrCviSixfBWjkqJB9naMeiIY9v5zyXcgNzLPb8JJGDsIianNOzDe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521625', '$2a$12$7JBf8xSVwJBEhMr3phVRdebSsgFPNxle4ptxg7CTwcPTPktAvpkmO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521626', '$2a$12$2Lw/zZ2L55zmwZFLXuqI0OT5o8bTKCJRhxz1oua/YnFsVSC7DbgY.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521627', '$2a$12$NZ8wPHB9kjqI7uPLIDWXBOpYIS2HM3RrEP3ekqJI4FEpGtxrV/erO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521628', '$2a$12$w4f1UnRWnVBlmeDOyVQajugf53ppgOy8DMV/7mjNji6dzAtEdX0Ky', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521629', '$2a$12$2BxXOHPt84SSleueq0aOM.DX9mX/RFBHWUansM9iUgnleFfFvqM1m', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521630', '$2a$12$bHpJEDmUKvd.ymzLf2XpeOdw.XvfEySQFdB7ez6gPQhswwdc1G8.q', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521631', '$2a$12$XVK.jgoYgCHnMA7tHlqz0OgqrXXDD3ZqR0nDMbtbKt8OH6iP06Fbe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521632', '$2a$12$nMT662PuExHIZ1LKIZGLQOUYQxNe9cv1vOHEgWnKw/eAP9Ij6Wyzq', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521633', '$2a$12$XLOEV.8iAxcZUFtO330jruVl6wo9zBP06iyYjgvT3tyAtSXzvb5GS', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521634', '$2a$12$VzA4x1g1RsQ69RciLBDrOOS53APlnNntuaUkhTlB.ONzW7S5r1xnm', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521635', '$2a$12$ModNxt/B.mAb.rX6bDcANu.FABWGOnEAsSYj7nD3/V6fiR/1k0z.C', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521636', '$2a$12$zO7MLs1M8LBGud.WhnEHmeaY8hiPHWucnP5GWpeycfjU60wNe6giG', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521637', '$2a$12$olQPE0YaWayPIrh1LJRC7ekCYin5LoXpid2X2IN/5eG4pqGFlB75e', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521638', '$2a$12$m7vILWgkY4lyQfpLJa4fketQJ2sUFO7fnQZDvjlF6S8EQWSXO/xGa', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521639', '$2a$12$uGEY.xLLjiPiASoABmpWweuoGjXyHLzDOfq9hXYhami..PYMlHhf.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521640', '$2a$12$BgqvRLuQAaCVIp/RzRBIquMJAsp5bAOO7XNlEa0lURNsTHF50Gbg6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521641', '$2a$12$8IX1nDZ2pOb1BWQRyCkJB.eOYOserXMw2ulaT0xRdPATJBAeUBwJu', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521642', '$2a$12$ZCCtBVtyx0O4qQilwm0vEuK5eMPh5Uc5yOq8O3V3fMWo8dddcTIr.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521643', '$2a$12$f4X5ixrMO3fITLE7/N1ENu88JV7o1DN6xS6ZjYU/V2YfXUS6oRuoq', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521644', '$2a$12$g7/nnngxjP/GosByy30lSO/O0pZXIQlKXTEa6CVgzC0QvHaoqP8nC', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521645', '$2a$12$8gbtdAFUxrmSfG6ZPLw.l.kv9LuI6ssPWAvYQqpEplXmg6NWBi30i', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521646', '$2a$12$lg9oZ1xJUjceVUp/cGcM1u3W4/hYNsnpjPo0lb7MWYqhdor2ojY4K', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521647', '$2a$12$qAC.PoLu11cCiIzGUk23.esU/3QQdYO4SqoJSx4eiaVZy/VWEEpI.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521648', '$2a$12$WCc.Xzqs2mJjVCPEaZBoEekHtrFl4AzLfOqQJFRXwVJi/s9LaT25O', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521649', '$2a$12$nlVzFGY37Ym/GDIvWWbKd.n0aSK/O/vvkretILmjHZk6BG4AyqGKW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521650', '$2a$12$9aqAxN6ERNX2qM87zCENL.G/4BehQFBnulZZFI.aSuYULYzOqfx0y', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521651', '$2a$12$uofAgLdEEUl1YtDDUgjdMumxwWyc0ZKfrPR17re4bvpHUQ3MF23i6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521652', '$2a$12$qMzdtvn.r1K/NQ7Rru1C2utmjs0LfbOp5iamAPjMbqE5MJT36EZxS', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521653', '$2a$12$JUNU2UV9qD8iqZGbWkevsOFhPMe309O2TWxIaBvmJ34Rsv3x8pP9e', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521654', '$2a$12$Tdr.i1rU7HTBogOOWCGCNuq8H.R289JqJOWEk/1emOzlZTBVkgsVa', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521655', '$2a$12$9ThEXR1svHauffNXXsGfO.alXe8NMkKhMTdkQc8B5Qdc7zkdEncG6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521656', '$2a$12$yp/PNK8xG12mn6eOstQ/X.6aWaua5U45mz6bkyDI7cNdT.d76oVqO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521657', '$2a$12$5PXFMc.DtpREcV.zIlW4T.9wXlOl9ZNnmR/i8493DIbvJuSn8Qhmu', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521658', '$2a$12$xginKx8xXDaeP8N8CVYV9OYA2xSndPCQwQTjfyeV0SxhwOENZsCfm', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521659', '$2a$12$JArCVbjKvsy9M5yxK3GK2.4555emEZKNrtxfXd5G.ERgOxUlYjk86', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521660', '$2a$12$pJCh5KNeu5L3DdTZBx7CH.x3K6uEL9nilOJrTSaUwnNe2HWLut8lK', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521661', '$2a$12$bMbRoJVmZACojP1QieBgBu17Hj/zU7Q116eJqeKoCYhzZWk4eyEkm', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521662', '$2a$12$GpMIoIElTwLaA9zXAr3lUui6Vs8rE2Pxv2PGIH7G81wgTfx0EBIi6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521663', '$2a$12$kt5vtIW7qp8tHihXpEOXye001uYUOSDOpeim93PfHpQ.ak0uvl0de', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521664', '$2a$12$C6MR71zZer6IxjvKjjo4Ee2v0Ats5QMZqy5xt1NbGZCn6/5F103V.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521665', '$2a$12$6vICDOpLiPJLFJLgk.5nsu0LBUetaJJMvKrm/jauJcp8gjHQ.n.PW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521666', '$2a$12$yX5bErNWTrvcckWxrmU4wugElsoUogPW5bzl8wRDB9BLFNyXmxdbW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521667', '$2a$12$yqG2C86HwO9/zob3VlJV0u7OU3Or7KDZhtvzIhCmiYizOqr.62F/m', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521668', '$2a$12$WnPjVrwRr/zS/yDxsYyGe.eYkn0eTpo95r2wOUNlghpV2r163VZbq', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521669', '$2a$12$fWlavfXiORDHjlhTUzJfNei.STX7T1cHCYH8MZQBnOaDBtde9bMP.', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521670', '$2a$12$GycNQ3tLsUIygP/LLhlH.O51WVRmrkP1JUdIcJH7CIj7w6hkp8e8S', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521671', '$2a$12$CFsR6VGC9RIVoMjAxf78iOiGRbSdr5XlJRwp2t/TE91lP/qaaYzzu', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521672', '$2a$12$sFgJZOdWu1bVPwS5kku8y./0vvcyNqR3tliRlZhqI6v6XiBpLqxCe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521673', '$2a$12$MMOEJqPecl.KVUuyGeOlGO6srxWbtlchQ4mOUy62mzundBf9r1nru', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521674', '$2a$12$XVpyHab114Il4/.uAO1f4OVsBbLPI7DfrKAZ8N0l8oro/6rv5BZQG', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521675', '$2a$12$sEdOabsFHpAQFtBsj4N03O0oUBdmKJaQ8sWVBNmLkY50UY.0nxIXy', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521676', '$2a$12$Gm6w1e4lnVRnqyf.0/l.o.HaE8DdDELTjRHz/I9/FRfAf8lzTSHLO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521677', '$2a$12$pLNVuVR/sdyRkYGAE1BT0.vcLvPro8jNYm1.HuhkV0GXSmnS6Y4ou', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521678', '$2a$12$a1YxsWWdMCMZ0LBK9uX18O7huHgaWsEdgOGVM6z7A0pI3e.75O8.W', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521679', '$2a$12$QS10y9zyGZ6oIu9HzZDlXelP4NpncY/0IGwlJdp54MFV/puRJ1/EW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521680', '$2a$12$8Bylv01xJ3Ag8cFnzfuuu.y/R17m/WQIPbeb50qpB8jxMxB07do1e', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521681', '$2a$12$ZVTXET/sTlJsY8Dge8TTUu174Kf5iY6dB89GIYutbmmqGsZp4l2Ta', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521682', '$2a$12$NrBbN8LTlXQt4pApTHJ2m.m/p2KWV/8Q272T4TICLrXPiI2Ka7YX6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521683', '$2a$12$v1ARSGpzq6rc1mg.799Hg.o3gE0sgobfoiLS6vYIkB0vD7fDJ24/2', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521684', '$2a$12$NCWfT3DTUwjV9W.ahhQxVuPcl3X6xnOZ.7aQmEJfRa/ZGIeoXrZya', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521685', '$2a$12$wLldl4/DsrQyZDvnKyzrmeNSVI/fUGNytGMfgqR6EoxLS1SGolu3C', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521686', '$2a$12$S2zh1SfW4TghBTPj6xwTqeoi.rN1PO2hcDv9ddcy6OhmMEs8fXfMe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521687', '$2a$12$sYL4rbDA7ovQ03NTs1KtEuIHVp6kKTtSUbBBwykIMerqmPw5e8Ade', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521688', '$2a$12$EPMhYtr7.KNa3XlNutqqa.ZxgHVgL.p2pqXIA223fs3szfvNbxG6e', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521689', '$2a$12$Clnq7k8VbyP.PmNBGXHsquqTvoK9R/46aOox2nbajs5OXlo6/9xKe', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521690', '$2a$12$AcSwwQqC0uh/ooQtZ7e7X.hq6hWDta1mfZpf7r/hdZTUDYr7CKeQ6', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521691', '$2a$12$.O0ogA4wPFb9.EvGEWY2E.uIq8P32bHMJzMx5klgtWhFpEOpEhSdO', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521692', '$2a$12$dx/VvtY9NsoAb/ibccHvlecSLkGzy.q..1nbWyrqYmvQLDMUFeirm', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521693', '$2a$12$8h4ONENdYlzOAq/i4xlwCOzgM9JtuY5UO9inZYyRiEtB863n7xa7e', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521694', '$2a$12$N.rNxHmLDT/BKGvS0msT0ORMUHHIT9kuq6zv9InjOwg9wOKGNMrvW', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521695', '$2a$12$m5L4OiuxP07EUDq4W6oLheDFr4qLvCTpS0H4E/vrIwLpfy2ovYBxi', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521696', '$2a$12$ytlN8hltXJx4spaoscC5TOXAnRuFsAvqL30ygiPs1GMMlDkF51o7S', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521697', '$2a$12$UqFeSqFJ1AcaD8SysTsctebv0.29zc61e3g8zvn4vlxpj2IQcePea', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521698', '$2a$12$edIAZfNUM3sjFUe3c4nDgOc4bhjzQSGQWsRmktPYIHUrfpR0lh7Ee', 'student');
INSERT INTO public.account (username, password, role) VALUES ('sv21521699', '$2a$12$hApudIeEfGDhf9Z/9bKtUeyD25QS1sRHQNMxeaWgZtqYMGilzmKd.', 'student');


--
-- TOC entry 4924 (class 0 OID 213581)
-- Dependencies: 219
-- Data for Name: course; Type: TABLE DATA; Schema: public; Owner: postgres
--

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


--
-- TOC entry 4921 (class 0 OID 213552)
-- Dependencies: 216
-- Data for Name: profile; Type: TABLE DATA; Schema: public; Owner: postgres
--

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
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521615', 'Lê Việt Trung', '2004-04-13 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521614', 'Trầm Minh Thành', '2004-07-29 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521616', 'Doãn Văn Công Chính', '2004-03-30 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521618', 'Cao Thọ HuyHuy', '2004-07-03 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521619', 'Chu Trần Nhật Linh', '2004-11-07 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521621', 'Trần Vũ Lam', '2004-06-25 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521622', 'Hoàng Trần Ngọc Vinh', '2004-08-29 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521623', 'Đỗ Đoàn Đức Anh', '2004-12-28 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521625', 'Nguyễn Hoàng Vinh', '2004-02-21 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521626', 'Trần Duy Mạnh', '2004-10-10 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521627', 'Nguyễn Cảnh Thọ', '2004-11-22 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521628', 'Trương Ngọc Nam', '2004-09-09 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521629', 'Nguyễn Vủ Linh', '2004-12-21 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521632', 'Phạm Đức Huy', '2004-08-27 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521633', 'Nguyễn Cảnh Thọ', '2004-10-17 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521634', 'Võ Tiến Tài', '2004-04-05 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521635', 'Võ Phước Thọ', '2004-06-26 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521636', 'Nguyễn Bảo Hùng', '2004-11-26 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521638', 'Nguyễn Ngọc Quang Thịnh', '2004-05-20 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521639', 'Lý Đạo Nam', '2004-04-19 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521641', 'Dương Tiến Nghĩa', '2004-11-25 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521642', 'Phan Chí Nguyên', '2004-05-01 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521644', 'Huỳnh Quốc Tuấn', '2004-03-28 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521646', 'Phạm Triệu Khiêm', '2004-11-08 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521647', 'Nguyễn Hoàng Duy Tân', '2004-07-06 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521648', 'Trần Vũ Lam', '2004-09-12 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521649', 'Đoàn Minh Duẩn', '2004-07-25 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521655', 'Đỗ Phương Hòa', '2004-07-15 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521612', 'Nguyễn Đồ Tùng', '2004-03-05 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521613', 'Trần Vũ Long', '2004-10-01 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521657', 'Hoàng Khánh Chi', '2004-09-08 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521658', 'Trần Thị Thiên Nhi', '2004-01-01 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521660', 'Phạm Thị Kim Mai', '2004-08-30 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521661', 'Nguyễn Thị Vy', '2004-09-02 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521663', 'Nguyễn Lê Ánh Nhi', '2004-12-31 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521665', 'Nguyễn Thị Viết Hương', '2004-08-12 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521667', 'Lưu Thị Tâm', '2004-05-19 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521668', 'Vũ Hải Yến', '2004-06-28 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521670', 'Huỳnh Lê Phương Trang', '2004-05-18 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521671', 'Vũ Thị Hồng Diệu', '2004-10-03 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521672', 'Võ Hồng Mỹ Duyên', '2004-12-16 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521674', 'Nguyễn Ngọc Khánh Đoan', '2004-05-05 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521677', 'Phan Thị Lan Anh', '2004-06-13 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521678', 'Bùi Thị Bích Hằng', '2004-10-02 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521679', 'Phan Thị Lan Anh', '2004-01-02 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521682', 'Bùi Thị Hồng Nhung', '2004-03-12 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521683', 'Trần Thị Hồng Trinh', '2004-02-16 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521684', 'Đỗ Hà Phương', '2004-01-29 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521686', 'Phan Thị Thu Thảo', '2004-06-09 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521689', 'Trần Thị Huỳnh Anh', '2004-09-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521690', 'Huỳnh Thị Phượng Anh', '2004-07-25 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521691', 'Lê Nguyễn Minh Thư', '2004-03-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521692', 'Ngô Hồng Đức', '2004-06-26 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521695', 'Nguyễn Lưu Như Thiên', '2004-11-03 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521696', 'Vũ Thị Hồng Diệu', '2004-11-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521697', 'Huỳnh Bá Vinh', '2004-05-01 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521652', 'Lê Bích Ngọc', '2004-02-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521654', 'Nguyễn Nữ Lệ Quyên', '2004-01-09 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521651', 'Phạm Thu Giang', '2004-01-23 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521653', 'Hoàng Khánh Chi', '2004-01-07 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521656', 'Nguyễn Thị Vy', '2004-12-03 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521659', 'Trần Nguyên Đông', '2004-05-29 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521662', 'Nguyễn Thị Thanh Vân', '2004-01-05 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521664', 'Đặng Thị Mỹ Trinh', '2004-04-24 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521611', 'Hà Đăng Tú', '2004-11-01 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521617', 'Vũ Ngọc Hoàng Anh', '2004-05-04 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521620', 'Hà Minh Triết', '2004-10-09 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521624', 'Lê Đình Tùng', '2004-06-07 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521630', 'Huỳnh Phước Đức', '2004-08-07 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521631', 'Nguyễn Lê Minh Đạt', '2004-07-31 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521637', 'Liêu Quốc Thuận', '2004-06-27 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521640', 'Nguyễn Công Nhất', '2004-07-06 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521643', 'Vũ Gia Bảo', '2004-03-18 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521645', 'Ngô Minh Nhựt', '2004-06-06 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521650', 'Nguyễn Đoàn Hữu Hiếu', '2004-11-22 00:00:00', 'Nam', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521666', 'Phạm Thị Kiều Oanh', '2004-03-07 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521669', 'Trần Thiên Thư', '2004-02-03 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521673', 'Trần Thị Kim Xuân', '2004-03-16 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521675', 'Trương Thị Mỹ Nương', '2004-11-13 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521676', 'Trần Thị Kim Xuân', '2004-02-24 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521680', 'Đặng Thị Nhạn', '2004-04-15 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521681', 'Phạm Quỳnh Trang', '2004-04-07 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521685', 'Trần Thị Ngọc Hân', '2004-07-15 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521687', 'Bùi Thị Hồng Nhung', '2004-08-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521688', 'Vũ Thị Thùy Linh', '2004-10-09 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521693', 'Phạm Thị Kiều Oanh', '2004-12-22 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521694', 'Trịnh Huỳnh Trang', '2004-08-21 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521698', 'Nguyễn Thị Ngọc Mỹ', '2004-11-24 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);
INSERT INTO public.profile (id, name, birthday, gender, level, trainingsystem, avatar) VALUES ('21521699', 'Nguyễn Thị Uyên Chi', '2004-08-16 00:00:00', 'Nữ', 'Đại học', 'Chính quy', NULL);


--
-- TOC entry 4930 (class 0 OID 213629)
-- Dependencies: 225
-- Data for Name: registercourse; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (1, 'MA006.O11', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (2, 'MA003.O11', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (3, 'MA006.O14', '21521602');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (4, 'MA003.O12', '21521602');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (5, 'IT004.O14', '21521602');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (6, 'IT003.O14', '21521602');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (7, 'MA006.O15', '21521603');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (8, 'MA003.O14', '21521603');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (9, 'IT004.O15', '21521603');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (10, 'IT003.O13', '21521603');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (11, 'MA006.O18', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (12, 'MA003.O15', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (13, 'IT004.O18', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (14, 'IT003.O12', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (15, 'MA006.O14', '21521605');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (16, 'IT004.O15', '21521605');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (17, 'MA003.O15', '21521605');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (18, 'IT003.O13', '21521605');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (19, 'MA006.O14', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (20, 'MA003.O12', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (21, 'IT004.O17', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (22, 'IT003.O14', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (23, 'MA006.O11', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (24, 'MA003.O11', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (25, 'IT003.O13', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (26, 'IT004.O15', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (27, 'MA006.O11', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (28, 'MA003.O11', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (29, 'IT004.O18', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (30, 'MA006.O14', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (31, 'MA003.O15', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (32, 'IT002.O12', '21521609');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (33, 'IT006.O14', '21521609');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (34, 'MA006.O14', '21521609');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (35, 'MA003.O14', '21521609');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (36, 'IT003.O11', '21521609');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (37, 'IT002.O12', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (38, 'IT006.O16', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (39, 'MA006.O14', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (40, 'MA003.O11', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (41, 'IT004.O18', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (42, 'IT003.O13', '21521610');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (43, 'IT006.O14', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (44, 'IT003.O12', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (45, 'IT002.O12', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (46, 'IT004.O14', '21521601');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (47, 'IT002.O12', '21521602');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (48, 'IT002.O12', '21521603');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (49, 'IT002.O11', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (50, 'IT006.O14', '21521604');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (51, 'IT002.O12', '21521605');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (52, 'IT002.O11', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (53, 'IT006.O16', '21521606');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (54, 'IT002.O12', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (55, 'IT006.O14', '21521607');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (56, 'IT002.O11', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (57, 'IT006.O14', '21521608');
INSERT INTO public.registercourse (id, idcourse, idprofile) VALUES (58, 'IT006.O16', '21521609');


--
-- TOC entry 4932 (class 0 OID 213646)
-- Dependencies: 227
-- Data for Name: registrationperiod; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.registrationperiod (id, starttime, endtime) VALUES (2, '2023-12-14 00:00:00', '2024-12-31 00:00:00');


--
-- TOC entry 4926 (class 0 OID 213590)
-- Dependencies: 221
-- Data for Name: score; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (1, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (3, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (4, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (5, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (6, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (8, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (9, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (10, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (11, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (12, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (13, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (14, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (15, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (16, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (17, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (18, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (19, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (20, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (21, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (22, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (23, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (24, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (25, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (26, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (27, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (28, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (29, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (30, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (31, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (32, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (33, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (34, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (35, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (36, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (37, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (38, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (39, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (40, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (41, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (42, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (43, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (44, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (45, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (46, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (47, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (48, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (49, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (50, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (51, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (52, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (53, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (54, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (55, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (56, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (57, NULL, NULL, NULL, NULL, 0.2, 0.3, 0, 0.5);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (58, NULL, NULL, NULL, NULL, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (60, 5, 10, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (62, 9, 6, 7, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (63, 5, 8, 5, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (65, 6, 8, 7, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (66, 9, 7, 5, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (67, 10, 5, 9, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (69, 10, 10, 5, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (70, 9, 8, 10, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (71, 9, 6, 10, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (73, 10, 10, 5, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (74, 8, 9, 10, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (76, 8, 5, 6, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (77, 10, 8, 6, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (78, 7, 8, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (80, 7, 8, 5, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (81, 7, 9, 7, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (83, 5, 8, 8, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (84, 6, 6, 9, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (85, 10, 6, 9, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (87, 5, 8, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (88, 7, 6, 5, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (90, 8, 8, 7, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (91, 8, 5, 5, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (92, 10, 7, 6, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (94, 9, 6, 7, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (95, 5, 5, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (96, 7, 7, 9, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (98, 5, 10, 5, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (99, 9, 5, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (101, 6, 7, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (102, 9, 10, 9, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (103, 6, 5, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (105, 7, 7, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (106, 6, 9, 6, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (2, 7, 8, 8, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (7, 9, 10, 7, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (59, 7, 6, 8, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (61, 10, 6, 10, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (64, 5, 7, 6, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (68, 9, 6, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (72, 7, 7, 8, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (75, 5, 7, 6, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (79, 8, 5, 6, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (82, 6, 6, 5, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (86, 5, 7, 8, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (89, 6, 9, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (93, 6, 8, 8, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (97, 8, 5, 6, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (100, 6, 10, 7, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (104, 9, 10, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (107, 7, 5, 7, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (108, 10, 7, 10, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (109, 8, 5, 7, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (110, 6, 9, 10, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (111, 6, 6, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (112, 6, 8, 8, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (113, 10, 6, 8, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (114, 8, 7, 7, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (115, 6, 10, 8, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (116, 7, 5, 10, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (117, 6, 7, 5, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (118, 7, 6, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (119, 8, 8, 9, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (120, 8, 6, 5, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (121, 7, 8, 9, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (122, 9, 5, 9, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (123, 8, 10, 9, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (124, 6, 6, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (125, 7, 7, 9, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (126, 8, 10, 7, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (127, 8, 10, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (128, 9, 8, 10, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (129, 9, 8, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (130, 6, 6, 5, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (131, 9, 10, 5, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (132, 7, 8, 7, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (133, 7, 8, 8, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (134, 7, 7, 7, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (135, 5, 5, 5, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (136, 10, 9, 7, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (137, 10, 9, 7, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (138, 5, 6, 9, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (139, 6, 8, 5, 8, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (140, 9, 10, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (141, 8, 10, 8, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (142, 10, 9, 8, 7, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (143, 6, 8, 10, 6, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (144, 5, 6, 6, 10, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (145, 8, 5, 8, 9, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (146, 5, 9, 6, 5, 0.1, 0.2, 0.3, 0.4);
INSERT INTO public.score (id, processscore, midtermscore, finalscore, practicescore, ratioprocess, ratiomidterm, ratiopractice, ratiofinal) VALUES (147, 10, 7, 10, 9, 0.1, 0.2, 0.3, 0.4);


--
-- TOC entry 4928 (class 0 OID 213605)
-- Dependencies: 223
-- Data for Name: schedule; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (1, 'GV1', 'IT004.O13', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (2, 'GV1', 'IT004.O17', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (3, 'GV1', 'IT004.O14', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (4, 'GV1', 'IT003.O14', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (5, 'GV1', 'IT003.O12', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (6, 'GV2', 'IT004.O15', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (7, 'GV2', 'IT004.O18', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (8, 'GV2', 'IT003.O13', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (9, 'GV2', 'IT003.O11', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (10, 'GV3', 'MA006.O14', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (11, 'GV3', 'MA006.O18', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (12, 'GV3', 'MA003.O12', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (13, 'GV3', 'MA003.O15', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (14, 'GV4', 'IT002.O12', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (15, 'GV4', 'IT006.O16', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (16, 'GV5', 'IT002.O11', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (17, 'GV5', 'IT006.O14', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (18, 'GV6', 'MA006.O11', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (19, 'GV6', 'MA006.O15', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (20, 'GV6', 'MA003.O11', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (21, 'GV6', 'MA003.O14', NULL, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (22, '21521601', 'IT006.O14', 1, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (23, '21521601', 'IT004.O14', 2, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (24, '21521601', 'MA003.O11', 3, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (25, '21521601', 'MA006.O11', 4, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (26, '21521601', 'IT003.O12', 5, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (27, '21521601', 'IT002.O12', 6, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (28, '21521602', 'IT004.O14', 7, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (29, '21521602', 'IT003.O14', 8, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (30, '21521602', 'MA006.O14', 9, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (31, '21521602', 'IT002.O12', 10, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (32, '21521602', 'MA003.O12', 11, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (33, '21521603', 'MA006.O15', 12, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (34, '21521603', 'IT004.O15', 13, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (35, '21521603', 'MA003.O14', 14, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (36, '21521603', 'IT002.O12', 15, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (37, '21521603', 'IT003.O13', 16, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (38, '21521604', 'IT006.O14', 17, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (39, '21521604', 'MA006.O18', 18, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (40, '21521604', 'IT004.O18', 19, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (41, '21521604', 'IT003.O12', 20, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (42, '21521604', 'MA003.O15', 21, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (43, '21521604', 'IT002.O11', 22, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (44, '21521605', 'IT004.O15', 23, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (45, '21521605', 'MA006.O14', 24, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (46, '21521605', 'MA003.O15', 25, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (47, '21521605', 'IT002.O12', 26, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (48, '21521605', 'IT003.O13', 27, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (49, '21521606', 'IT006.O16', 28, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (50, '21521606', 'IT004.O17', 29, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (51, '21521606', 'IT003.O14', 30, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (52, '21521606', 'MA006.O14', 31, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (53, '21521606', 'IT002.O11', 32, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (54, '21521606', 'MA003.O12', 33, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (55, '21521607', 'IT006.O14', 34, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (56, '21521607', 'IT004.O15', 35, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (57, '21521607', 'MA003.O11', 36, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (58, '21521607', 'MA006.O11', 37, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (59, '21521607', 'IT002.O12', 38, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (60, '21521607', 'IT003.O13', 39, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (61, '21521608', 'IT006.O14', 40, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (62, '21521608', 'MA003.O11', 41, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (63, '21521608', 'IT004.O18', 42, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (64, '21521608', 'MA006.O11', 43, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (65, '21521608', 'MA006.O14', 44, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (66, '21521608', 'MA003.O15', 45, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (67, '21521608', 'IT002.O11', 46, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (68, '21521609', 'IT006.O14', 47, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (69, '21521609', 'IT006.O16', 48, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (70, '21521609', 'IT003.O11', 49, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (71, '21521609', 'MA006.O14', 50, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (72, '21521609', 'MA003.O14', 51, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (73, '21521609', 'IT002.O12', 52, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (74, '21521610', 'IT006.O16', 53, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (75, '21521610', 'MA003.O11', 54, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (76, '21521610', 'IT004.O18', 55, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (77, '21521610', 'MA006.O14', 56, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (78, '21521610', 'IT002.O12', 57, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (79, '21521610', 'IT003.O13', 58, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (80, '21521611', 'IT004.O14', 59, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (81, '21521612', 'IT004.O14', 60, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (82, '21521613', 'IT004.O14', 61, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (83, '21521614', 'IT004.O14', 62, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (84, '21521615', 'IT004.O14', 63, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (85, '21521616', 'IT004.O14', 64, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (86, '21521617', 'IT004.O14', 65, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (87, '21521618', 'IT004.O14', 66, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (88, '21521619', 'IT004.O14', 67, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (89, '21521620', 'IT004.O14', 68, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (90, '21521621', 'IT004.O14', 69, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (91, '21521622', 'IT004.O14', 70, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (92, '21521623', 'IT004.O14', 71, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (93, '21521624', 'IT004.O14', 72, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (94, '21521625', 'IT004.O14', 73, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (95, '21521626', 'IT004.O14', 74, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (96, '21521627', 'IT004.O14', 75, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (97, '21521628', 'IT004.O14', 76, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (98, '21521629', 'IT004.O14', 77, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (99, '21521630', 'IT004.O14', 78, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (100, '21521631', 'IT004.O14', 79, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (101, '21521632', 'IT004.O14', 80, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (102, '21521633', 'IT004.O14', 81, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (103, '21521634', 'IT004.O14', 82, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (104, '21521635', 'IT004.O14', 83, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (105, '21521636', 'IT004.O14', 84, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (106, '21521637', 'IT004.O14', 85, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (107, '21521638', 'IT004.O14', 86, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (108, '21521639', 'IT004.O14', 87, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (109, '21521640', 'IT004.O14', 88, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (110, '21521641', 'IT004.O14', 89, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (111, '21521642', 'IT004.O14', 90, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (112, '21521643', 'IT004.O14', 91, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (113, '21521644', 'IT004.O14', 92, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (114, '21521645', 'IT004.O14', 93, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (115, '21521646', 'IT004.O14', 94, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (116, '21521647', 'IT004.O14', 95, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (117, '21521648', 'IT004.O14', 96, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (118, '21521649', 'IT004.O14', 97, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (119, '21521650', 'IT004.O14', 98, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (120, '21521651', 'IT004.O14', 99, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (121, '21521652', 'IT004.O14', 100, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (122, '21521653', 'IT004.O14', 101, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (123, '21521654', 'IT004.O14', 102, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (124, '21521655', 'IT004.O14', 103, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (125, '21521656', 'IT004.O14', 104, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (126, '21521657', 'IT004.O14', 105, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (127, '21521658', 'IT004.O14', 106, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (128, '21521659', 'IT004.O14', 107, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (129, '21521660', 'IT004.O14', 108, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (130, '21521661', 'IT004.O14', 109, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (131, '21521662', 'IT004.O14', 110, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (132, '21521663', 'IT004.O14', 111, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (133, '21521664', 'IT004.O14', 112, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (134, '21521665', 'IT004.O14', 113, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (135, '21521666', 'IT004.O14', 114, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (136, '21521667', 'IT004.O14', 115, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (137, '21521668', 'IT004.O14', 116, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (138, '21521669', 'IT004.O14', 117, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (139, '21521670', 'IT004.O14', 118, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (140, '21521671', 'IT004.O14', 119, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (141, '21521672', 'IT004.O14', 120, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (142, '21521673', 'IT004.O14', 121, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (143, '21521674', 'IT004.O14', 122, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (144, '21521675', 'IT004.O14', 123, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (145, '21521676', 'IT004.O14', 124, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (146, '21521677', 'IT004.O14', 125, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (147, '21521678', 'IT004.O14', 126, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (148, '21521679', 'IT004.O14', 127, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (149, '21521680', 'IT004.O14', 128, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (150, '21521681', 'IT004.O14', 129, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (151, '21521682', 'IT004.O14', 130, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (152, '21521683', 'IT004.O14', 131, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (153, '21521684', 'IT004.O14', 132, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (154, '21521685', 'IT004.O14', 133, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (155, '21521686', 'IT004.O14', 134, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (156, '21521687', 'IT004.O14', 135, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (157, '21521688', 'IT004.O14', 136, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (158, '21521689', 'IT004.O14', 137, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (159, '21521690', 'IT004.O14', 138, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (160, '21521691', 'IT004.O14', 139, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (161, '21521692', 'IT004.O14', 140, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (162, '21521693', 'IT004.O14', 141, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (163, '21521694', 'IT004.O14', 142, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (164, '21521695', 'IT004.O14', 143, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (165, '21521696', 'IT004.O14', 144, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (166, '21521697', 'IT004.O14', 145, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (167, '21521698', 'IT004.O14', 146, NULL);
INSERT INTO public.schedule (id, idprofile, idcourse, idscore, note) VALUES (168, '21521699', 'IT004.O14', 147, NULL);


--
-- TOC entry 4923 (class 0 OID 213563)
-- Dependencies: 218
-- Data for Name: useracc; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (1, 'sv21521601', '21521601');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (2, 'sv21521602', '21521602');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (3, 'sv21521603', '21521603');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (4, 'sv21521604', '21521604');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (5, 'sv21521605', '21521605');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (6, 'sv21521606', '21521606');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (7, 'sv21521607', '21521607');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (8, 'sv21521608', '21521608');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (9, 'sv21521609', '21521609');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (10, 'sv21521610', '21521610');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (11, 'gv1', 'GV1');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (12, 'gv2', 'GV2');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (13, 'gv3', 'GV3');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (14, 'gv4', 'GV4');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (15, 'gv5', 'GV5');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (16, 'gv6', 'GV6');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (17, 'admin', 'QL');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (18, 'sv21521611', '21521611');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (19, 'sv21521612', '21521612');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (20, 'sv21521613', '21521613');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (21, 'sv21521614', '21521614');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (22, 'sv21521615', '21521615');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (23, 'sv21521616', '21521616');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (24, 'sv21521617', '21521617');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (25, 'sv21521618', '21521618');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (26, 'sv21521619', '21521619');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (27, 'sv21521620', '21521620');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (28, 'sv21521621', '21521621');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (29, 'sv21521622', '21521622');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (30, 'sv21521623', '21521623');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (31, 'sv21521624', '21521624');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (32, 'sv21521625', '21521625');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (33, 'sv21521626', '21521626');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (34, 'sv21521627', '21521627');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (35, 'sv21521628', '21521628');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (36, 'sv21521629', '21521629');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (37, 'sv21521630', '21521630');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (38, 'sv21521631', '21521631');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (39, 'sv21521632', '21521632');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (40, 'sv21521633', '21521633');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (41, 'sv21521634', '21521634');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (42, 'sv21521635', '21521635');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (43, 'sv21521636', '21521636');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (44, 'sv21521637', '21521637');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (45, 'sv21521638', '21521638');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (46, 'sv21521639', '21521639');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (47, 'sv21521640', '21521640');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (48, 'sv21521641', '21521641');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (49, 'sv21521642', '21521642');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (50, 'sv21521643', '21521643');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (51, 'sv21521644', '21521644');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (52, 'sv21521645', '21521645');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (53, 'sv21521646', '21521646');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (54, 'sv21521647', '21521647');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (55, 'sv21521648', '21521648');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (56, 'sv21521649', '21521649');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (57, 'sv21521650', '21521650');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (58, 'sv21521651', '21521651');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (59, 'sv21521652', '21521652');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (60, 'sv21521653', '21521653');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (61, 'sv21521654', '21521654');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (62, 'sv21521655', '21521655');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (63, 'sv21521656', '21521656');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (64, 'sv21521657', '21521657');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (65, 'sv21521658', '21521658');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (66, 'sv21521659', '21521659');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (67, 'sv21521660', '21521660');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (68, 'sv21521661', '21521661');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (69, 'sv21521662', '21521662');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (70, 'sv21521663', '21521663');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (71, 'sv21521664', '21521664');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (72, 'sv21521665', '21521665');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (73, 'sv21521666', '21521666');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (74, 'sv21521667', '21521667');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (75, 'sv21521668', '21521668');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (76, 'sv21521669', '21521669');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (77, 'sv21521670', '21521670');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (78, 'sv21521671', '21521671');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (79, 'sv21521672', '21521672');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (80, 'sv21521673', '21521673');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (81, 'sv21521674', '21521674');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (82, 'sv21521675', '21521675');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (83, 'sv21521676', '21521676');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (84, 'sv21521677', '21521677');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (85, 'sv21521678', '21521678');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (86, 'sv21521679', '21521679');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (87, 'sv21521680', '21521680');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (88, 'sv21521681', '21521681');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (89, 'sv21521682', '21521682');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (90, 'sv21521683', '21521683');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (91, 'sv21521684', '21521684');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (92, 'sv21521685', '21521685');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (93, 'sv21521686', '21521686');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (94, 'sv21521687', '21521687');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (95, 'sv21521688', '21521688');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (96, 'sv21521689', '21521689');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (97, 'sv21521690', '21521690');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (98, 'sv21521691', '21521691');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (99, 'sv21521692', '21521692');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (100, 'sv21521693', '21521693');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (101, 'sv21521694', '21521694');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (102, 'sv21521695', '21521695');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (103, 'sv21521696', '21521696');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (104, 'sv21521697', '21521697');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (105, 'sv21521698', '21521698');
INSERT INTO public.useracc (id, idaccount, idprofile) VALUES (106, 'sv21521699', '21521699');
