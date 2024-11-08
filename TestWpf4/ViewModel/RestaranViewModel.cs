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
    public class RestaranViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Состав_Заказа> _products;
        private Блюдо _newDish;

        public string _connectionString = @"Data Source=EUGENE; DataBase=Var4; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";

        public RestaranViewModel()
        {
            _products = new ObservableCollection<Состав_Заказа>();
            _newDish = new Блюдо();
            LoadProducts();

            AddDishCommand = new RelayCommand(OpenAddDishWindow);
            AddOrderCommand = new RelayCommand(OpenAddOrderWindow);
            GoHomeNavigateCommand = new RelayCommand(GoHome);
        }

        public ObservableCollection<Состав_Заказа> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Блюдо NewDish
        {
            get => _newDish;
            set
            {
                _newDish = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddDishCommand { get; }
        public ICommand AddOrderCommand { get; }
        public ICommand GoHomeNavigateCommand { get; }

        private void LoadProducts()
        {
            // Загрузка данных из базы данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        s.Id_Состав_Блюда, 
                        z.Id_Заказа,
                        s.Блюдо, 
                        s.Заказ, 
                        s.Колличество, 
                        s.Статус,
                        b.Название,
                        b.Вес,
                        b.Доступ_для_заказа,
                        z.Стол
                    FROM 
                        Состав_Заказа s
                    INNER JOIN 
                        Блюдо b ON s.Блюдо = b.Id_Блюдо
                    INNER JOIN 
                        Заказ z ON s.Заказ = z.Id_Заказа";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Состав_Заказа> products = new ObservableCollection<Состав_Заказа>();

                        while (reader.Read())
                        {
                            Состав_Заказа product = new Состав_Заказа
                            {
                                Id_Состав_Блюда = reader.GetInt32(0),
                                Блюдо = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                Заказ = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                Колличество = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                Статус = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Блюдо1 = new Блюдо
                                {
                                    Название = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    Вес = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                                    Доступ_для_заказа = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                                },
                                Заказ1 = new Заказ
                                {
                                    Стол = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                                }
                            };

                            products.Add(product);
                        }

                        Products = products;
                    }
                }
            }
        }

        private void OpenAddDishWindow(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new AddDishWindow();
            }
        }

        private void OpenAddOrderWindow(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new AddOrderWindowxaml();
            }
        }

        private void GoHome(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new LoginWindow();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
