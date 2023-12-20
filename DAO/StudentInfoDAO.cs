using Npgsql;
using NpgsqlTypes;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
    public class StudentInfoDAO
    {
        private static StudentInfoDAO instance;

        public static StudentInfoDAO Instance
        {
            get
            {
                if (instance == null) instance = new StudentInfoDAO();
                return StudentInfoDAO.instance;
            }
            private set
            {
                StudentInfoDAO.instance = value;
            }
        }

        private StudentInfoDAO() { }

        public StudentInfo LoadStudentInfo(string id)
        {
            string query = "SELECT * FROM LoadProfileById( :id )";
            DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
            StudentInfo info = new StudentInfo(data.Rows[0]);

            return info;
        }

        public bool AddStudent(StudentInfo student)
        {
            try
            {
                string query = "SELECT CreateProfile(@id, @name, @birthday, @gender, @level, @trainingsystem, @avatar)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", student.Id);
                        command.Parameters.AddWithValue("@name", student.Name);
                        command.Parameters.AddWithValue("@birthday", NpgsqlDbType.Timestamp, student.Birthday);
                        command.Parameters.AddWithValue("@gender", student.Gender);
                        command.Parameters.AddWithValue("@level", student.EducationLevel);
                        command.Parameters.AddWithValue("@trainingsystem", student.TrainingSystem);
                        command.Parameters.AddWithValue("@avatar", NpgsqlDbType.Bytea, student.Avatar);

                        bool res = (bool)command.ExecuteScalar();

                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public bool UpdateStudent(StudentInfo student)
        {
            try
            {
                string query = "SELECT UpdateProfile(@id, @name, @birthday, @gender, @level, @trainingsystem, @avatar)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", student.Id);
                        command.Parameters.AddWithValue("@name", student.Name);
                        command.Parameters.AddWithValue("@birthday", NpgsqlDbType.Timestamp, student.Birthday);
                        command.Parameters.AddWithValue("@gender", student.Gender);
                        command.Parameters.AddWithValue("@level", student.EducationLevel);
                        command.Parameters.AddWithValue("@trainingsystem", student.TrainingSystem);
                        command.Parameters.AddWithValue("@avatar", NpgsqlDbType.Bytea, student.Avatar);
                        bool res = (bool)command.ExecuteScalar();

                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }

        }

        public bool DeleteStudent(string id)
        {
            try
            {
                string query = "SELECT DeleteProfileById(@id)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        bool res = (bool)command.ExecuteScalar();

                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public StudentInfo GetStudentById(string id)
        {
            try
            {
                string query = "SELECT * FROM ReadProfileById(@id)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var avatarObj = reader["avatar"];
                                byte[] avatar = null;
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    IFormatter formatter = new BinaryFormatter();
                                    formatter.Serialize(stream, avatarObj);

                                    avatar = stream.ToArray();
                                }
                                StudentInfo account = new StudentInfo()
                                {
                                    Id = reader["id"].ToString(),
                                    Name = reader["name"].ToString(),
                                    Gender = reader["gender"].ToString(),
                                    Birthday = Convert.ToDateTime(reader["birthday"]),
                                    EducationLevel = reader["level"].ToString(),
                                    TrainingSystem = reader["trainingsystem"].ToString(),
                                    Avatar = avatar,
                                };
                                return account;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }
        public StudentInfo SearchStudent(string name)
        {
            try
            {
                string query = "SELECT * FROM ReadProfileByName(@name)";
                using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var avatarObj = reader["avatar"];
                                byte[] avatar = null;
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    IFormatter formatter = new BinaryFormatter();
                                    formatter.Serialize(stream, avatarObj);

                                    avatar = stream.ToArray();
                                }
                                StudentInfo account = new StudentInfo()
                                {
                                    Id = reader["id"].ToString(),
                                    Name = reader["name"].ToString(),
                                    Gender = reader["gender"].ToString(),
                                    Birthday = Convert.ToDateTime(reader["birthday"]),
                                    EducationLevel = reader["level"].ToString(),
                                    TrainingSystem = reader["trainingsystem"].ToString(),
                                    Avatar = avatar,
                                };
                                return account;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }

        public DataTable GetStudents()
        {
            try
            {
                string query = "SELECT * FROM ReadAllProfiles()"; // Thay doi query tuy theo cau truc cua bang Account

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
