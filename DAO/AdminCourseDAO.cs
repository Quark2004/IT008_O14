using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using QLSV.DTO;
using System.Windows.Input;
using OfficeOpenXml;
using System.IO;

namespace QLSV.DAO
{
    internal class AdminCourseDAO
    {
        private static AdminCourseDAO instance;

        public static AdminCourseDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new AdminCourseDAO();
                return AdminCourseDAO.instance;
            }
            private set
            {
                AdminCourseDAO.instance = value;
            }
        }

        private AdminCourseDAO() { }

        public bool AddCourse(CourseDTO course)
        {
            try
            {
                string query = "INSERT INTO Course (id, name, numberofcredits, schoolday, lesson, classroom, semester, schoolyear, startday, endday) " +
                               "VALUES (@id, @name, @numberofcredits, @schoolday, @lesson, @classroom, @semester, @schoolyear, @startday, @endday)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", course.id);
                        command.Parameters.AddWithValue("@name", course.name);
                        command.Parameters.AddWithValue("@numberofcredits", course.numberofcredits);
                        command.Parameters.AddWithValue("@schoolday", course.schoolday);
                        command.Parameters.AddWithValue("@lesson", course.lesson);
                        command.Parameters.AddWithValue("@classroom", course.classroom);
                        command.Parameters.AddWithValue("@semester", course.semester);
                        command.Parameters.AddWithValue("@schoolyear", course.schoolyear);
                        command.Parameters.AddWithValue("@startday", course.startday);
                        command.Parameters.AddWithValue("@endday", course.endday);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateCourse(CourseDTO course)
        {
            try
            {
                string query = "UPDATE Course SET name = @name, numberofcredits = @numberofcredits, schoolday = @schoolday, " +
                               "lesson = @lesson, classroom = @classroom, semester = @semester, schoolyear = @schoolyear, " +
                               "startday = @startday, endday = @endday WHERE id = @id";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", course.id);
                        command.Parameters.AddWithValue("@name", course.name);
                        command.Parameters.AddWithValue("@numberofcredits", course.numberofcredits);
                        command.Parameters.AddWithValue("@schoolday", course.schoolday);
                        command.Parameters.AddWithValue("@lesson", course.lesson);
                        command.Parameters.AddWithValue("@classroom", course.classroom);
                        command.Parameters.AddWithValue("@semester", course.semester);
                        command.Parameters.AddWithValue("@schoolyear", course.schoolyear);
                        command.Parameters.AddWithValue("@startday", course.startday);
                        command.Parameters.AddWithValue("@endday", course.endday);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteCourse(string courseId)
        {
            try
            {
                string query = "DELETE FROM Course WHERE id = @id";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", courseId);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public CourseDTO SearchCourse(string courseId, string courseName)
        {
            string query = "SELECT * FROM Course WHERE Id = @id And  Name = @name LIMIT 1";

            using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", courseId);
                    command.Parameters.AddWithValue("@name", courseName);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CourseDTO course = new CourseDTO
                            {
                                id = reader["id"].ToString(),
                                name = reader["name"].ToString(),

                                numberofcredits = Convert.ToInt32(reader["numberofcredits"]),
                                schoolday = reader["schoolday"].ToString(),
                                lesson = reader["lesson"].ToString(),
                                classroom = reader["classroom"].ToString(),
                                semester = reader["semester"].ToString(),
                                schoolyear = reader["schoolyear"].ToString(),

                                startday = Convert.ToDateTime(reader["startday"]),
                                endday = Convert.ToDateTime(reader["endday"])
                            };
                            return course; 
                        }
                    }
                }
            }

            return null; 
        }
        public CourseDTO SearchCourseInExcel(string filePath, string courseId, string courseName)
        {
            DataTable dataFromExcel = GetDataFromExcel(filePath); 

            DataRow[] foundRows = dataFromExcel.Select($"id = '{courseId}' AND name = '{courseName}'");

            if (foundRows.Length > 0)
            {
                DataRow row = foundRows[0];
                CourseDTO course = new CourseDTO
                {
                    id = row["id"].ToString(),
                    name = row["Name"].ToString(),
                    numberofcredits = Convert.ToInt32(row["numberofcredits"]),
                    schoolday = row["schoolday"].ToString(),
                    lesson = row["lesson"].ToString(),
                    classroom = row["classroom"].ToString(),
                    semester = row["semester"].ToString(),
                    schoolyear = row["schoolyear"].ToString(),
                    startday = Convert.ToDateTime(row["startday"]),
                    endday = Convert.ToDateTime(row["endday"]),
                };
                return course;
            }

            return null;
        }

        public DataTable GetDataFromExcel(string filePath)
        {
            DataTable dt = new DataTable();

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

                FileInfo file = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; 

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Đọc dữ liệu từ Excel vào DataTable
                    for (int col = 1; col <= colCount; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow excelRow = dt.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            excelRow[col - 1] = worksheet.Cells[row, col].Value;
                        }
                        dt.Rows.Add(excelRow);
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Đã xảy ra lỗi khi đọc từ Excel: " + ex.Message);
            }

            return dt;
        }

        public DataTable GetCourseData()
        {
            try
            {
                string query = "SELECT * FROM Course";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable data = new DataTable();
                            adapter.Fill(data);
                            return data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

    }
}
