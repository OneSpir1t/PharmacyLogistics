using PharmacyLogistics.Entities;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace PharmacyLogistics.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            if (AptContext.aptContext.Database.CanConnect())
            {
                AptContext.aptContext.Database.OpenConnection();
            }
            Login_TextBox.Text = "Valeriya@mail.ru";
            Password_PasswordBox.Password = "VH5#s";
            //Login_TextBox.Text = "2";
            //Password_PasswordBox.Password = "2";
        }

        private string? password;

        private void ShowHidePassword_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ShowHidePassword_Image.Source.ToString() == "pack://application:,,,/Resources/OpenEye.png")
            {
                ShowHidePassword_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/CloseEye.png"));
                Password_PasswordBox.Visibility = Visibility.Visible;
                Password_TextBox.Visibility = Visibility.Hidden;
                Password_PasswordBox.Password = password;

            }
            else
            {
                ShowHidePassword_Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/OpenEye.png"));
                Password_TextBox.Visibility = Visibility.Visible;
                Password_PasswordBox.Visibility = Visibility.Hidden;
                Password_TextBox.Text = password;
            }
        }

        private void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Login_TextBox.Text) && !string.IsNullOrEmpty(password) && password.Length > 0)
            {
                if (AptContext.aptContext.Database.CanConnect())
                {
                    User? user = AptContext.aptContext.Users.FirstOrDefault(u => u.Login == Login_TextBox.Text && u.Password == password);
                    if (user != null)
                    {
                        Hide();
                        password = "";
                        Password_PasswordBox.Password = "";
                        Password_TextBox.Text = default;
                        MessageBox.Show($"Вы вошли как {user.UserRole.Name.ToLower()}, добро пожаловать, {string.Join(' ', user.Surname, user.Name, user.Patryonomic)}", "Успешно");
                        var mw = new UserWindow(user);
                        mw.Owner = this;
                        mw.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Уведомление");
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к базе данных db_apteka", "Ошибка подключения");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Уведомление");
            }
            
        }

        private void Password_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = Password_TextBox.Text;
        }

        private void Password_PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = Password_PasswordBox.Password;
        }
    }
}
