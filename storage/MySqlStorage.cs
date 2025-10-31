using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SqlAppTest.storage;
using SqlAppTest;

namespace SqlAppTest.storage
{
    public class MySqlStorage : Storage
    {
        private string _connectionString;
        private MySqlConnection _connection;

        public MySqlStorage()
        {
          
        }
        public void setupDataSource()
        {
            _connectionString = $"Server={ConfigManager.MySql.HOST};Port={ConfigManager.MySql.PORT};Database={ConfigManager.MySql.DATABASE};User Id={ConfigManager.MySql.USER};Password={ConfigManager.MySql.PASSWORD};";
            _connection = new MySqlConnection(_connectionString);

            try
            {
                _connection.Open();
                Console.WriteLine("Подключение к базе sqlApp успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения: " + ex.Message);
            }
        }

        public void Close()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("Подключение закрыто.");
            }
        }

        public void initDataBase()
        {
            if (_connection == null)
                throw new InvalidOperationException("Соединение не инициализировано. Вызовите setupDataSource() перед initDataBase().");

            string query = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Username VARCHAR(100) NOT NULL UNIQUE,
                    Password VARCHAR(255) NOT NULL
                );";

            using (var cmd = new MySqlCommand(query, _connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Таблица Users создана или уже существует.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при создании таблицы: " + ex.Message);
                }
            }
        }

        public void addUser(string username, string password)
        {
            string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
            using (var cmd = new MySqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.ExecuteNonQuery();
            }
        }


        public User getUSer(string username)
        {
            string query = "SELECT Username, Password FROM Users WHERE Username=@Username";
            using (var cmd = new MySqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            name = reader.GetString(0),
                            password = reader.GetString(1)
                        };
                    }
                }
            }
            return null;
        }

        public void removeUser(User user)
        {
            string query = "DELETE FROM Users WHERE Username=@Username";
            using (var cmd = new MySqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@Username", user.name);
                cmd.ExecuteNonQuery();
            }
        }
    }
}