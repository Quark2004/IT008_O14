CREATE DATABASE ManageStudents
GO

USE ManageStudents
GO

Create Table Account (
    username NVARCHAR(100) PRIMARY KEY,
    password NVARCHAR(1000) NOT NULL,
    role NVARCHAR(100) DEFAULT N'student',
)
GO

CREATE TABLE Profile (
    id NVARCHAR(100) PRIMARY KEY,
    name NVARCHAR(100),
    birthday DATE,
    gender NVARCHAR(100), 
    level NVARCHAR(100) DEFAULT N'Đại học',
    trainingSystem NVARCHAR(100) DEFAULT N'Chính quy',
    avatar IMAGE,
)
GO


CREATE TABLE UserAcc (
    id int IDENTITY PRIMARY KEY,
    idAccount NVARCHAR(100) REFERENCES Account(username),
    idProfile NVARCHAR(100) REFERENCES Profile(id)
)
GO


CREATE TABLE Course (
    id nvarchar(100) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    numberOfCredits INT NOT NULL,
    schoolDay NVARCHAR(100) NOT NULL,
    lesson NVARCHAR(100) NOT NULL,
    classroom NVARCHAR(100) NOT NULL,
    semester NVARCHAR(100) NOT NULL,
    schoolYear NVARCHAR(100) NOT NULL,
    startDay DATE NOT NULL,
    endDay DATE NOT NULL,
)
GO

CREATE TABLE Score (
    id int IDENTITY PRIMARY KEY,
    processScore FLOAT,
    midtermScore FLOAT,
    finalScore FLOAT,
    practiceScore FLOAT,
    ratioProcess FLOAT DEFAULT 0.1,
    ratioMidterm FLOAT DEFAULT 0.2,
    ratioPractice FLOAT DEFAULT 0.3,
    ratioFinal FLOAT DEFAULT 0.4
)
GO

CREATE TABLE Schedule (
    id int IDENTITY PRIMARY KEY,
    idProfile NVARCHAR(100) NOT NULL REFERENCES Profile (id),
    idCourse NVARCHAR(100) NOT NULL REFERENCES Course(id),
    idScore int REFERENCES Score(id),
    note nvarchar(1000),
)
GO

Create TABLE RegisterCourse (
    id int PRIMARY KEY IDENTITY,
    idCourse NVARCHAR(100) REFERENCES Course(id),
    idProfile NVARCHAR(100) REFERENCES Profile(id),
)
GO

-- Store procedure 

create PROC Login  
    @username NVARCHAR(100),
    @password NVARCHAR(1000)
as 
BEGIN
    SELECT Profile.id as N'MSSV' FROM Account, Profile, UserAcc
    WHERE   Account.username = UserAcc.idAccount AND
            UserAcc.idProfile = Profile.id AND
            username = @username and [password] = @password
end
go

-- EXEC Login @username = N'student' , @password = N'123456'

create proc LoadProfileById
    @id NVARCHAR(100)
as
BEGIN
    SELECT  id as N'MSSV', 
            name as N'Tên', 
            birthday as N'Ngày sinh', 
            gender as N'Giới tính', 
            [level] as N'Bậc đào tạo', 
            trainingSystem as N'Hệ đào tạo', 
            avatar as N'Ảnh đại diện' 
    FROM Profile
    WHERE id = @id
end
go

-- EXEC LoadProfileById @id = N'21521601'

create proc GetScheduleByID
    @id nvarchar(100)
AS
BEGIN
    select Course.id as N'Mã môn học',
    Course.name as N'Tên môn học',
    Course.classroom as N'Phòng học',
    Course.startDay as N'Ngày bắt đầu',
    Course.endDay as N'Ngày kết thúc',
    Course.schoolDay as N'Thứ',
    Course.lesson as N'Tiết'
    from Schedule, Course
    WHERE Schedule.idProfile = @id
        and Schedule.idCourse = Course.id
    ORDER BY Course.schoolDay ASC, 
            Course.lesson ASC
END
GO

-- EXEC GetScheduleByID @id = N'21521601'

create PROC GetListRegisterCourse
AS
BEGIN
    select Course.name AS N'Tên môn học',
        Course.id AS N'Mã lớp',
        Profile.name AS N'Tên giảng viên',
        Course.numberOfCredits as N'Số tín chỉ',
        Course.schoolDay AS N'Thứ',
        Course.lesson AS N'Tiết',
        Course.classroom AS N'Phòng',
        Course.semester As N'Học kì',
        Course.semester as N'Năm học',
        Course.startDay AS N'Ngày bắt đầu',
        Course.endDay AS N'Ngày kết thúc' 
   
    from Schedule, Course, Profile, UserAcc, Account
    WHERE Schedule.idCourse = Course.id
        and Schedule.idProfile = Profile.id
        and UserAcc.idProfile = Profile.id
        and UserAcc.idAccount = Account.username
        and Account.role = 'teacher'
    ORDER BY Course.name, Course.id
END
GO

-- EXEC GetListRegisterCourse 
-- go

create proc GetLearningOutcomes
    @id NVARCHAR(100)
as 
BEGIN

    SELECT Course.id as N'Mã học phần',
        Course.name as N'Tên học phần',
        Course.numberOfCredits as N'Tín chỉ',
        Score.processScore as N'Điểm quá trình',
        Score.midtermScore as N'Điểm giữa kì',
        Score.practiceScore as N'Điểm thực hành',
        Score.finalScore as N'Điểm cuối kì',
        ROUND((Score.processScore * Score.ratioProcess 
                + Score.midtermScore * Score.ratioMidterm 
                + Score.practiceScore * Score.ratioPractice
                +  Score.finalScore * Score.ratioFinal
            ), 2) as N'Điểm học phần',
        Course.semester as N'Học kì',
        Course.schoolYear as N'Năm học'
    FROM Schedule, Course, Score
    WHERE Schedule.idCourse = Course.id
        and Schedule.idScore = Score.id
        and Schedule.idProfile = @id
END
GO

-- EXEC GetLearningOutcomes @id = N'21521601'
-- GO

CREATE PROC GetClassInCharge
    @id NVARCHAR(100)
AS
BEGIN
    SELECT  Course.id as N'Mã lớp', 
            Course.name as N'Tên môn học', 
            Course.schoolDay as N'Thứ', 
            Course.lesson as N'Tiết', 
            COUNT(*) as N'SLSV', 
            Schedule.note as N'Ghi chú'
    FROM Schedule, Course
    WHERE not idProfile = @id
        AND Schedule.idCourse IN
    (
        SELECT Course.id FROM Schedule, Course
        WHERE   Schedule.idCourse = Course.id
            AND Schedule.idProfile = @id
    )
    and Schedule.idCourse = Course.id
    GROUP BY Course.id, Course.name, Course.schoolDay, Course.lesson, Schedule.note
END
GO

-- EXEC GetClassInCharge @id = N'GV2'
-- GO

create proc GetListClass
    @idCourse NVARCHAR(100)
AS
BEGIN

    SELECT Profile.id as N'MSSV',
        Profile.name as N'Tên sinh viên',
        Score.processScore as N'Điểm quá trình',
        Score.midtermScore as N'Điểm giữa kì',
        Score.practiceScore as N'Điểm thực hành',
        Score.finalScore as N'Điểm cuối kì',
        ROUND((Score.processScore * ratioProcess 
                + Score.midtermScore * ratioMidterm
                + Score.practiceScore * ratioPractice
                +  Score.finalScore * ratioFinal
            ), 2) as N'Điểm học phần'
    FROM Schedule, Profile, Score
    WHERE Schedule.idProfile = Profile.id
        and Schedule.idScore = Score.id
        and Schedule.idCourse = @idCourse
    ORDER BY Profile.id
END
go

-- EXEC GetListClass @idCourse = N'IT003.1'
-- go

create proc GetListRegisteredByID
    @id nvarchar(100)
AS
BEGIN
    select Course.id as N'Mã môn học',
    Course.name as N'Tên môn học',
    Course.classroom as N'Phòng học',
    Course.startDay as N'Ngày bắt đầu',
    Course.endDay as N'Ngày kết thúc',
    Course.schoolDay as N'Thứ',
    Course.lesson as N'Tiết'
    from RegisterCourse, Course
    WHERE RegisterCourse.idProfile = @id
        and RegisterCourse.idCourse = Course.id
        ORDER BY Course.schoolDay ASC, 
            Course.lesson ASC 
END
GO

-- EXEC GetListRegisteredByID @id = N'21521601'


-------------------------
------ CRUD
create proc InsertAcc
    @username nvarchar(100),
    @password NVARCHAR(1000),
    @id NVARCHAR(100)
AS
BEGIN
    if ((SELECT COUNT(*) FROM Account
            WHERE username = @username) > 0) RETURN

    if ((SELECT COUNT(*) FROM Profile
            WHERE id = @id) > 0) RETURN

    INSERT into Account(username, [password])
    VALUES (@username, @password)

    Insert into Profile(id)
    values (@id)

    INSERT into UserAcc(idAccount, idProfile)
    VALUES (@username, @id)
END
GO

-- EXEC InsertAcc @username = N'student11', 
--                 @password = N'123456', 
--                 @id = N'21521611'
-- GO

CREATE proc UpdatePass
    @username NVARCHAR(100),
    @password NVARCHAR(1000)
AS
BEGIN
    UPDATE Account 
    set [password] = @password
    WHERE username = @username
END
GO

-- EXEC UpdatePass @username = N'student1' , @password = N'123456'
-- go



-- EXEC UpdatePass @username = N'student0' , @password = N'654321'

create PROC UpdateProfile 
    @id NVARCHAR(100),
    @name NVARCHAR(100),
    @birthday DATE,
    @gender NVARCHAR(100),
    @level NVARCHAR(100),
    @trainingSystem NVARCHAR(100),
    @avatar IMAGE
AS 
BEGIN
    UPDATE Profile
    SET name = @name,
        birthday = @birthday,
        gender = @gender,
        [level] = @level,
        trainingSystem = @trainingSystem,
        avatar = @avatar
    WHERE id = @id
end
GO

-- EXEC UpdateProfile @id = N'21521601' ,
--             @name = N'Student 101',
--             @birthday = N'2000-01-01',
--             @gender = N'Nam',
--             @level = N'Đại học',
--             @trainingSystem = N'Chính quy',
--             @avatar = Null       
-- go


create PROC UpdateScore
    @idCourse NVARCHAR(100),
    @idProfile NVARCHAR(100),
    @processScore FLOAT,
    @midtermScore FLOAT,
    @finalScore FLOAT,
    @practiceScore FLOAT
AS
BEGIN
    if (
        @processScore < 0 or @processScore > 10 or 
        @midtermScore < 0 or @midtermScore > 10 or 
        @finalScore < 0 or @processScore > 10 or 
        @practiceScore < 0 or @processScore > 10
    ) RETURN

    UPDATE Score
    SET processScore = @processScore,
        midtermScore = @midtermScore,
        finalScore  = @finalScore,
        practiceScore = @practiceScore
    FROM Score, Schedule
    WHERE Schedule.idProfile = @idProfile
        and Schedule.idCourse = @idCourse
        and Schedule.idScore = Score.id
END
GO

-- EXEC UpdateScore @idCourse = N'IT003.1',
--                 @idProfile = N'21521601',
--                 @processScore = 7.3,
--                 @midtermScore = 7.1,
--                 @finalScore = 9.2,
--                 @practiceScore = 10
-- go

create PROC UpdateRatioScore
    @idCourse NVARCHAR(100),
    @ratioProcess FLOAT,
    @ratioMidterm FLOAT,
    @ratioFinal FLOAT,
    @ratioPractice FLOAT
AS
BEGIN
    IF (@ratioPractice > 1 or 
        @ratioMidterm > 1 or
        @ratioFinal > 1 or 
        @ratioProcess > 1) RETURN

    UPDATE Score
    SET ratioProcess = @ratioProcess,
        ratioMidterm = @ratioMidterm,
        ratioFinal  = @ratioFinal,
        ratioPractice = @ratioPractice
    FROM Score, Schedule
    WHERE Schedule.idCourse = @idCourse
        and Schedule.idScore = Score.id
END
GO

-- EXEC UpdateRatioScore @idCourse = N'IT003.1',
--                 @ratioProcess = 0.2,
--                 @ratioMidterm = 0.3,
--                 @ratioFinal = 0.5,
--                 @ratioPractice = 0



create PROC JoinCourse 
    @idProfile NVARCHAR(100),
    @idCourse NVARCHAR(100)
AS
BEGIN
    -- Không được đăng ký môn ko có trong danh sách các môn được mở 
    if ((SELECT COUNT(*) FROM Course
            WHERE id = @idCourse)  = 0) RETURN

    -- Không được đăng kí 1 môn nhiều lần
    IF (SELECT COUNT(*) FROM Schedule
        WHERE idProfile = @idProfile and
                idCourse = @idCourse) > 0 RETURN

    -- Các môn học không được trùng lịch học
    if (select COUNT(*) FROM
        (SELECT schoolDay, lesson FROM Course WHERE id = @idCourse) as infoCourse,
        (SELECT schoolDay, lesson FROM Schedule, Course WHERE Schedule.idCourse = Course.id and Schedule.idProfile = @idProfile) as allInfoCourse
            WHERE infoCourse.schoolDay = allInfoCourse.schoolDay AND 
                    (infoCourse.lesson like '%' + allInfoCourse.lesson + '%' or
                        allInfoCourse.lesson like '%' + infoCourse.lesson + '%')) > 0 RETURN

    -- Khi sinh viên đã tham gia lớp học thì phải có bản điểm
    INSERT into Score(processScore)
    VALUES (NULL)

    DECLARE @idScore int

    SELECT @idScore = id FROM  Score 
    WHERE id = (SELECT MAX(id)  FROM Score)

    INSERT Schedule (idCourse, idProfile, idScore)
    VALUES (@idCourse, @idProfile, CAST(@idScore as nvarchar))
END
GO

-- EXEC JoinCourse @idProfile = N'21521601' , @idCourse = N'IT001.1'

create PROC LeaveCourse
    @idProfile NVARCHAR(100),
    @idCourse NVARCHAR(100)
AS
BEGIN
    DECLARE @idScore int 

    SELECT @idScore = Score.id from Score, Schedule 
        WHERE Schedule.idScore = Score.id AND
            Schedule.idProfile = @idProfile AND
            Schedule.idCourse = @idCourse

    delete Schedule 
    WHERE Schedule.idScore = @idScore

    delete Score
    WHERE id = @idScore
END
GO

-- EXEC LeaveCourse @idProfile =  N'21521601' , @idCourse = N'IT001.2'

create PROC JoinRegisterCourse (
    @idProfile NVARCHAR(100),
    @idCourse NVARCHAR(100)
)
AS
BEGIN
    -- Không được đăng ký môn ko có trong danh sách các môn được mở 
    if ((SELECT COUNT(*) FROM Course
            WHERE id = @idCourse)  = 0) RETURN

    -- Không được đăng kí 1 môn nhiều lần
    if ((SELECT COUNT(*) FROM RegisterCourse
            WHERE idCourse = @idCourse and 
                    idProfile = @idProfile) > 0) RETURN
                
    -- Các môn học không được trùng lịch học
    
    if ((SELECT COUNT(*) FROM
            (SELECT Course.schoolDay, Course.lesson FROM Course WHERE id = @idCourse) AS infoCourse,
            (SELECT Course.schoolDay, Course.lesson FROM RegisterCourse, Course WHERE RegisterCourse.idCourse = Course.id and RegisterCourse.idProfile = @idProfile) as allInfoCourse
            WHERE allInfoCourse.schoolDay = infoCourse.schoolDay AND
                    (allInfoCourse.lesson LIKE '%' + infoCourse.lesson + '%' or 
                        infoCourse.lesson LIKE '%' + allInfoCourse.lesson + '%'
                    )
        ) > 0) RETURN

    INSERT RegisterCourse(idCourse, idProfile)
    VALUES(@idCourse, @idProfile)
END
GO

-- EXEC JoinRegisterCourse @idProfile = N'21521601' , @idCourse = N'IT001.1'

create PROC LeaveRegisterCourse (
    @idProfile NVARCHAR(100),
    @idCourse NVARCHAR(100)
)
AS
BEGIN
    DELETE RegisterCourse
    WHERE idCourse = @idCourse AND
            idProfile = @idProfile
end
go

-- EXEC LeaveRegisterCourse @idProfile = N'21521601' , @idCourse = N'IT001.1'
-- GO

-----------------------------------------

-- Insert Data

-- Profile

-- Student


DECLARE @i INT = 1
WHILE @i <= 10
BEGIN

    DECLARE @gender NVARCHAR(100) = N'Nam' 
    IF (RAND() * 10 < 5)
    BEGIN
        SET @gender = N'Nữ'
    end 

   
    INSERT into Profile(id, name)
    VALUES (CAST(@i + 21521600 as nvarchar), N'Student ' + CAST(@i as nvarchar))

    set @i = @i + 1
END
GO

-- Teacher
INSERT into Profile(id, name)
VALUES (N'GV1', N'Teacher 1')

INSERT into Profile(id, name)
VALUES (N'GV2', N'Teacher 2')

INSERT into Profile(id, name)
VALUES (N'GV3', N'Teacher 3')

-- Manager
INSERT into Profile(id, name)
VALUES (N'QL', N'Manage 1')


-- Account

-- Manage

INSERT into Account(username, [password], role)
VALUES (N'admin', N'123456', N'admin')

-- Teacher
INSERT into Account(username, [password], role)
VALUES (N'teacher1', N'123456', N'teacher')

INSERT into Account(username, [password], role)
VALUES (N'teacher2', N'123456', N'teacher')

INSERT into Account(username, [password], role)
VALUES (N'teacher3', N'123456', N'teacher')

-- Student

DECLARE @i int = 1
WHILE @i <= 10 
BEGIN
    INSERT into Account(username, [password])
    VALUES (N'student' + CAST(@i as nvarchar), N'123456')

    set @i = @i + 1
end
go


-- UserAcc
DECLARE @i int = 1
WHILE @i <= 10 
BEGIN
    INSERT INTO UserAcc(idAccount, idProfile)
    VALUES (N'Student'+CAST(@i as nvarchar), CAST(@i + 21521600 as nvarchar))

    set @i = @i + 1
end
go

DECLARE @i int = 1
WHILE @i <= 3 
BEGIN
    INSERT INTO UserAcc(idAccount, idProfile)
    VALUES (N'teacher'+CAST(@i as nvarchar), N'GV' + CAST(@i as nvarchar))

    set @i = @i + 1
end
go

INSERT INTO UserAcc(idAccount, idProfile)
VALUES (N'admin', N'QL')


-- Course

-- IT001
INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT001.1', N'Nhập môn lập trình', '2023-09-11', '2023-12-30', 2, N'12345', N'B2.22', 4, N'HK1', N'2023-2024')

INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT001.2', N'Nhập môn lập trình', '2023-09-11', '2023-12-30', 2, N'6789', N'B5.02', 4, N'HK1', N'2023-2024')

INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT001.3', N'Nhập môn lập trình', '2023-09-11', '2023-12-30', 4, N'1234', N'B1.22', 4, N'HK1', N'2023-2024')

INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT001.4', N'Nhập môn lập trình', '2023-09-11', '2023-12-30', 4, N'67890', N'B3.02', 4, N'HK1', N'2023-2024')

-- IT002
INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT002.1', N'Cấu trúc dữ liệu và giải thuật', '2023-09-11', '2023-12-30', 2, N'67890', N'B3.18', 4, N'HK1', N'2023-2024')


-- IT003
INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT003.1', N'Cấu trúc dữ liệu và giải thuật', '2023-09-11', '2023-12-30', 6, N'12345', N'B3.18', 4, N'HK1', N'2023-2024')

INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT003.2', N'Cấu trúc dữ liệu và giải thuật', '2023-09-11', '2023-12-30', 3, N'67890', N'B3.08', 4, N'HK1', N'2023-2024')

INSERT into Course(id, name, startDay, endDay, schoolDay, lesson ,classroom, numberOfCredits, semester, schoolYear)
VALUES (N'IT003.3', N'Cấu trúc dữ liệu và giải thuật', '2023-09-11', '2023-12-30', 5, N'6789', N'B5.02', 4, N'HK1', N'2023-2024')

-- Score
DECLARE @i int = 0
DECLARE @min int = 5
DECLARE @max int = 10

WHILE @i < 20
BEGIN
    INSERT into Score(processScore, midtermScore, finalScore, practiceScore)
    VALUES (ROUND(RAND() * (@max - @min) + @min, 2), ROUND(RAND() * (@max - @min) + @min, 2), ROUND(RAND() * (@max - @min) + @min, 2), ROUND(RAND() * (@max - @min) + @min, 2))
    set @i = @i + 1
END
GO


-- Schedule

-- Teacher

INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT001.2', N'GV1')
INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT001.3', N'GV1')
INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT003.3', N'GV1')

INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT001.1', N'GV2')
INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT001.4', N'GV2')

INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT003.1', N'GV3')
INSERT into Schedule(idCourse, idProfile)
VALUES (N'IT003.2', N'GV3')

DECLARE @idScore int = 1
DECLARE @i int  = 1
WHILE @i <= 10
BEGIN
    if (@i % 3 = 1)
    BEGIN
        INSERT into Schedule(idCourse, idProfile, idScore) 
        VALUES (N'IT001.2', CAST(21521600 + @i as nvarchar), @idScore),
            (N'IT003.1', CAST(21521600 + @i as nvarchar), @idScore + 1)
            
        SET @idScore = @idScore + 2
    end

    if (@i % 3 = 2)
    BEGIN
        INSERT into Schedule(idCourse, idProfile, idScore) 
        VALUES (N'IT001.4', CAST(21521600 + @i as nvarchar), @idScore),
            (N'IT003.2', CAST(21521600 + @i as nvarchar), @idScore + 1)

        SET @idScore = @idScore + 2
        
    end

    if (@i % 3 = 0)
    BEGIN
        INSERT into Schedule(idCourse, idProfile, idScore) 
        VALUES (N'IT001.1', CAST(21521600 + @i as nvarchar), @idScore),
            (N'IT003.3', CAST(21521600 + @i as nvarchar), @idScore + 1)

        SET @idScore = @idScore + 2
        
    end

    SET @i = @i + 1
    
end
go

update Schedule
set idCourse = N'IT001.3'
WHERE idProfile = N'21521609' 
    or idCourse = N'21521604'
GO

select * from Account
select * from Profile
select * from UserAcc
select * from Course
select * from Score
select * from Schedule
go
