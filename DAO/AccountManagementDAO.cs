using Npgsql;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLSV.DAO.AccoutManagementDAO;

namespace QLSV.DAO
{
    internal class AccoutManagementDAO
    {
        public class AccountManagementDAO
        {
            private static AccountManagementDAO instance;

            public static AccountManagementDAO Instance
            {
                get
                {
                    if (instance == null) instance = new AccountManagementDAO();
                    return AccountManagementDAO.instance;
                }
                private set
                {
                    AccountManagementDAO.instance = value;
                }
            }

            public AccountManagementDAO() { }
            private string HashPassword(string password)
            {

                return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
            }

            public bool AddAccount(AccountManagementDTO account)
            {
                try
                {
                    string query = "SELECT CreateAccount(@username, @password, @role)";
                    using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            string hashedPassword = HashPassword(account.Password);

                            command.Parameters.AddWithValue("@username", account.Username);
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@role", account.Role);

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
            public bool UpdateAccount(AccountManagementDTO account)
            {
                try
                {
                    string query = "SELECT UpdateAccount(@username, @password, @role)";
                    using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            string hashedPassword = HashPassword(account.Password);
                            command.Parameters.AddWithValue("@username", account.Username);
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@role", account.Role);
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

            public bool DeleteAccount(string username)
            {
                try
                {
                    string query = "SELECT DeleteAccount(@username)";
                    using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@username", username);

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
            public AccountManagementDTO SearchAccount(string username)
            {
                try
                {
                    string query = "SELECT * FROM ReadAccount(@username)";
                    using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@username", username);

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    AccountManagementDTO account = new AccountManagementDTO
                                    {
                                        Username = reader["username"].ToString(),
                                        Password = reader["password"].ToString(),
                                        Role = reader["role"].ToString()
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

            public AccountManagementDTO SearchAccountExceptAdmin(string username)
            {
                try
                {
                    string query = "SELECT * FROM ReadAccount(@username)";
                    using (NpgsqlConnection connection = new NpgsqlConnection(DataProvider.Instance.connectionStr))
                    {
                        connection.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@username", username);

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string role = reader["role"].ToString();
                                    if(role == "admin")
                                    {
                                        return null;
                                    }
                                    AccountManagementDTO account = new AccountManagementDTO
                                    {
                                        Username = reader["username"].ToString(),
                                        Password = reader["password"].ToString(),
                                        Role = reader["role"].ToString()
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
            public DataTable GetAccountData()
            {
                try
                {
                    string query = "SELECT * FROM ReadAllAccounts()"; // Thay doi query tuy theo cau truc cua bang Account

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

            public DataTable GetAccountsExceptAdmin()
            {
                try
                {
                    string query = "SELECT * FROM ReadAllAccountsExceptAdmin()"; // Thay doi query tuy theo cau truc cua bang Account

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
}
