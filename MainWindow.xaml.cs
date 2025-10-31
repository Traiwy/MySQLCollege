using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqlAppTest.storage;

namespace SqlAppTest
{
    public partial class MainWindow : Window
    {
        private MySqlStorage _storage;
        public MainWindow()
        {
            InitConfig();
            InitMySql();
            InitializeComponent();
            
        }


        private void InitConfig()
        {
            try
            {
                ConfigManager configManager = new ConfigManager("resources/config.yml");
                configManager.load();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке конфигурации: " + ex.Message);
            }
        }
        
        private void InitMySql()
        {
            _storage = new MySqlStorage();
            _storage.setupDataSource();
            _storage.initDataBase();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                var existingUser = _storage.getUSer(username);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь уже зарегистрирован!");
                }
                else
                {
                    _storage.addUser(username, password);
                    MessageBox.Show("Пользователь успешно зарегистрирован!");
                }

 
                LoginTextBox.Clear();
                PasswordBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _storage.Close();
        }
    }
}
    
