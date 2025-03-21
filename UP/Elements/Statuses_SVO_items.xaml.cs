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
    /// Логика взаимодействия для Statuses_SVO_items.xaml
    /// </summary>
    public partial class Statuses_SVO_items : UserControl
    {
        ClassModules.Statuses_SVO svo;
        public Statuses_SVO_items(ClassModules.Statuses_SVO _svo)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            svo = _svo;
            if (_svo.DocumentOsnov != null)
            {
                svo_name.Content += _svo.SVOStatusID.ToString();
                prikaz.Content += _svo.DocumentOsnov;
                nachStat.Content += _svo.StartDate.ToString("dd.MM.yyyy");
                konStat.Content += _svo.EndDate.ToString("dd.MM.yyyy");
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _svo.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _svo.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Statuses_SVO(svo));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о овз?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                    string query = $"Delete From Statuses_SVO Where Id_garage = " + svo.SVOStatusID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Statuses_SVO);
                    }
                    else MessageBox.Show("Запрос на удаление овз не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}