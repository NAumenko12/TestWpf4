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
    public class AddDishViewModel : INotifyPropertyChanged
    {
        private Блюдо _newDish;
        private ObservableCollection<Категория_блюда> _categories;
        private readonly string _connectionString = @"Data Source=EUGENE; DataBase=Var4; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";

        public AddDishViewModel()
        {
            NewDish = new Блюдо();
            LoadCategories();
            AddDishCommand = new RelayCommand(AddDish);
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

        public ObservableCollection<Категория_блюда> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddDishCommand { get; }

        private void LoadCategories()
        {
            // Загрузка категорий из базы данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Id_Категория_блюда, 
                        Название
                    FROM 
                        Категория_Блюда";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Категория_блюда> categories = new ObservableCollection<Категория_блюда>();

                        while (reader.Read())
                        {
                            Категория_блюда category = new Категория_блюда
                            {
                                Id_Категория_блюда = reader.GetInt32(0),
                                Название = reader.GetString(1)
                            };

                            categories.Add(category);
                        }

                        Categories = categories;
                    }
                }
            }
        }

        private void AddDish(object parameter)
        {
            // Добавление нового блюда в базу данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Блюдо (Название, Вес, Категория, Доступ_для_заказа, Стоимость)
                    VALUES (@Название, @Вес, @Категория, @Доступ_для_заказа, @Стоимость)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Название", NewDish.Название);
                    command.Parameters.AddWithValue("@Вес", NewDish.Вес);
                    command.Parameters.AddWithValue("@Категория", NewDish.Категория);
                    command.Parameters.AddWithValue("@Доступ_для_заказа", NewDish.Доступ_для_заказа);
                    command.Parameters.AddWithValue("@Стоимость", NewDish.Стоимость);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Блюдо успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
