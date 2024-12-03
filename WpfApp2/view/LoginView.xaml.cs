using System;
using Npgsql;
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

namespace DesignApp.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        private Window mainWindow;
        private string sql = "Server = localhost; " + "Port = 5432; " + "Database = workers_Db; " + "User Id = postgres; " + "Password = 123;";
        public LoginView(Window mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string login = txtUser.Text;
            string password = txtPass.Password;

            CheckData(login, password);
        }

        private void CheckData(string login, string password)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(sql);
            sqlConnection.Open();

            NpgsqlCommand sqlCommand = new NpgsqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM public.workers";

            NpgsqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            bool flag = false;
            while (sqlDataReader.Read())
            {
                Worker worker = new Worker();
                worker.login = sqlDataReader.GetString(5);
                worker.password = sqlDataReader.GetString(6);

                if (worker.login.Equals(login) && worker.password.Equals(password))
                {
                    worker.id = sqlDataReader.GetInt64(0);
                    worker.surname = sqlDataReader.GetString(1);
                    worker.name = sqlDataReader.GetString(2);
                    worker.lastname = sqlDataReader.GetString(3);
                    worker.job_title = sqlDataReader.GetString(4);
                    flag = true;
                    NavigationService.Navigate(new MainView(mainWindow));
                }
            }
            if (!flag)
            {
                MessageBox.Show("Incorrect Data. Please, try again");
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                mainWindow.DragMove();
        }
    }
}
