using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SqlAppTest
{
    public class ConfigManager
    {
        private string _filePath;

        public ConfigManager(string filePath)
        {
            _filePath = filePath;
        }


        public void load()
        {
            Console.WriteLine($"Текущая директория: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Ищем файл по пути: {Path.GetFullPath(_filePath)}");
            Console.WriteLine($"Файл существует: {File.Exists(_filePath)}");
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Файл конфигурации не найден", _filePath);

            var yamlContent = File.ReadAllText(_filePath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var config = deserializer.Deserialize<ConfigRoot>(yamlContent);
            parse(config);

        }

        private void parse(ConfigRoot config)
        {
            MySql.HOST = config.mysql.Host;
            MySql.PORT = config.mysql.Port;
            MySql.USER = config.mysql.User;
            MySql.PASSWORD = config.mysql.Password;
            MySql.DATABASE = config.mysql.Database;
        }

        private class ConfigRoot
        {
            public MySqlConfig mysql { get; set; }
        }


        private class MySqlConfig
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string Database { get; set; }
        }
        public static class MySql
        {
            public static string HOST;
            public static int PORT;
            public static string USER;
            public static string PASSWORD;
            public static string DATABASE;
        }
    }


}
