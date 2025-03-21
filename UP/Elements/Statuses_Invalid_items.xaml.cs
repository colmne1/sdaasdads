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
using ClassConnection;

namespace UP.Elements
{
    /// <summary>
    /// Логика взаимодействия для Statuses_Invalid_items.xaml
    /// </summary>
    public partial class Statuses_Invalid_items : UserControl
    {
        ClassModules.Statuses_Invalid invalid;
        public Statuses_Invalid_items(ClassModules.Statuses_Invalid _invalid)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            invalid = _invalid;
            if (_invalid.OrderNumber != null)
            {
                invalid_name.Content += _invalid.DisabilityStatusID.ToString();
                prikaz.Content += _invalid.OrderNumber;
                nachStat.Content += _invalid.StartDate.ToString("dd.MM.yyyy");
                konStat.Content += _invalid.EndDate.ToString("dd.MM.yyyy");
                vidInvalid.Content += _invalid.DisabilityType;
                primech.Content += _invalid.Note.ToString();
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _invalid.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _invalid.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Statuses_Invalidi(invalid));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о инвалидах?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                    string query = $"Delete From Statuses_Invalid Where DisabilityStatusID = " + invalid.DisabilityStatusID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Statuses_Invalid);
                    }
                    else MessageBox.Show("Запрос на удаление инвалидов не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}