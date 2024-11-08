using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using TestWpf4.Model;
using TestWpf4.View;

namespace TestWpf4.ViewModel
{
    public class AddOrderViewModel : INotifyPropertyChanged
    {
        private Заказ _newOrder;
        private ObservableCollection<Блюдо> _dishes;
        private ObservableCollection<Сотрудники> _employees;
        private readonly string _connectionString = @"Data Source=EUGENE; DataBase=Var4; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";

        public AddOrderViewModel()
        {
            NewOrder = new Заказ();
            LoadDishes();
            LoadEmployees();
            AddOrderCommand = new RelayCommand(AddOrder);
        }

        public Заказ NewOrder
        {
            get => _newOrder;
            set
            {
                _newOrder = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Блюдо> Dishes
        {
            get => _dishes;
            set
            {
                _dishes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Сотрудники> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddOrderCommand { get; }

        private void LoadDishes()
        {
            // Загрузка блюд из базы данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Id_Блюдо, 
                        Название
                    FROM 
                        Блюдо";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Блюдо> dishes = new ObservableCollection<Блюдо>();

                        while (reader.Read())
                        {
                            Блюдо dish = new Блюдо
                            {
                                Id_Блюдо = reader.GetInt32(0),
                                Название = reader.GetString(1)
                            };

                            dishes.Add(dish);
                        }

                        Dishes = dishes;
                    }
                }
            }
        }

        private void LoadEmployees()
        {
            // Загрузка сотрудников из базы данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Id_Сотрудника, 
                        Фамилия
                    FROM 
                        Сотрудники";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Сотрудники> employees = new ObservableCollection<Сотрудники>();

                        while (reader.Read())
                        {
                            Сотрудники employee = new Сотрудники
                            {
                                Id_Сотрудника = reader.GetInt32(0),
                                Фамилия = reader.GetString(1)
                            };

                            employees.Add(employee);
                        }

                        Employees = employees;
                    }
                }
            }
        }

        private void AddOrder(object parameter)
        {
            // Добавление нового заказа в базу данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Заказ (Офицанта, Стол)
                    VALUES (@Офицанта, @Стол)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Офицанта", NewOrder.Офицанта);
                    command.Parameters.AddWithValue("@Стол", NewOrder.Стол);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Заказ успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var shopViewModel = new RestaranViewModel();
                mainWindow.MainContent.Content = new RestaranWindow() { DataContext = shopViewModel };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
