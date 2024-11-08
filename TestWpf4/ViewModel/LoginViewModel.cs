using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using TestWpf4.View;

namespace TestWpf4.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _loginText;
        private string _password;
        private readonly string _connectionString = @"Data Source=EUGENE; DataBase=Var4; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;"; 

        public LoginViewModel()
        {
            AuthorizeCommand = new RelayCommand(Authorize);
        }

        public string LoginText
        {
            get => _loginText;
            set
            {
                _loginText = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand AuthorizeCommand { get; }
        

        private void Authorize(object parameter)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        s.Id_Сотрудника, 
                        s.Логин, 
                        s.Пароль, 
                        s.Фамилия, 
                        s.Имя, 
                        s.Отчество, 
                        s.Оклад, 
                        s.Должность,
                        d.Название
                    FROM 
                        Сотрудники s
                    INNER JOIN 
                        Должности d ON s.Должность = d.Id_Должности
                    WHERE 
                        s.Логин = @Логин AND s.Пароль = @Пароль";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Логин", LoginText);
                    command.Parameters.AddWithValue("@Пароль", Password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idСотрудника = reader.GetInt32(0);
                            string логин = reader.GetString(1);
                            string пароль = reader.GetString(2);
                            string фамилия = reader.GetString(3);
                            string имя = reader.GetString(4);
                            string отчество = reader.GetString(5);
                            int? оклад = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);
                            int? должность = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7);
                            string названиеДолжности = reader.GetString(8);

                            // Вывод информации о пользователе и его должности
                            MessageBox.Show($"Добро пожаловать, {имя} {фамилия}!\nДолжность: {названиеДолжности}", "Успешная авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigateToRest( );
                            
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    
                }
            }
        }
        private void NavigateToRest( )
        {
            
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
