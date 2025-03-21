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
    /// Логика взаимодействия для SPPP_Meetings_items.xaml
    /// </summary>
    public partial class SPPP_Meetings_items : UserControl
    {
        ClassModules.SPPP_Meetings sppp;
        public SPPP_Meetings_items(ClassModules.SPPP_Meetings _sppp)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            sppp = _sppp;
            if (_sppp.OsnVizov != null)
            {
                sppp_name.Content += _sppp.MeetingID.ToString();
                date.Content += _sppp.Date.ToString("dd.MM.yyyy");
                osnVizov.Content += _sppp.OsnVizov;
                sotrud.Content += _sppp.Sotrudniki.ToString();
                predstav.Content += _sppp.Predstaviteli.ToString();
                prichViz.Content += _sppp.ReasonForCall.ToString();
                resh.Content += _sppp.Reshenie.ToString();
                primech.Content += _sppp.Note.ToString();
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _sppp.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _sppp.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.SPPP_Meetings(sppp));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о СППП?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                    string query = $"Delete From SPPP_Meetings Where MeetingID = " + sppp.MeetingID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.SPPP_Meetings);
                    }
                    else MessageBox.Show("Запрос на удаление СППП не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}