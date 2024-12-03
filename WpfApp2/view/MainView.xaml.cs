using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        private Window mainWindow;

        private ObservableCollection<Worker> workers;
        private string sql = "Host=localhost;Port=5432;Database=workers_Db;Username=postgres;Password=123;";


        public MainView(Window mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            workers = new ObservableCollection<Worker>();

            LoadData();

            myDataGrid.ItemsSource = workers;

        }

        private void LoadData()
        {
            using (var sqlConnection = new NpgsqlConnection(sql))
            {
                sqlConnection.Open();

                using (var sqlCommand = new NpgsqlCommand("SELECT * FROM public.workers", sqlConnection))
                {
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Worker worker = new Worker
                            {
                                id = sqlDataReader.GetInt64(0),
                                surname = sqlDataReader.GetString(1),
                                name = sqlDataReader.GetString(2),
                                lastname = sqlDataReader.GetString(3),
                                job_title = sqlDataReader.GetString(4),
                                login = sqlDataReader.GetString(5),
                                password = sqlDataReader.GetString(6)
                            };

                            workers.Add(worker);
                        }
                    }
                }
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginView(mainWindow));
        }
    }
}
