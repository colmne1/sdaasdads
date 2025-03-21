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
    /// Логика взаимодействия для Statuses_OVZ_items.xaml
    /// </summary>
    public partial class Statuses_OVZ_items : UserControl
    {
        ClassModules.Statuses_OVZ ovz;
        public Statuses_OVZ_items(ClassModules.Statuses_OVZ _ovz)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            ovz = _ovz;
            if (_ovz.Prikaz != null)
            {
                ovz_name.Content += _ovz.OVZStatusID.ToString();
                prikaz.Content += _ovz.Prikaz;
                nachStat.Content += _ovz.StartDate.ToString("dd.MM.yyyy");
                konStat.Content += _ovz.EndDate.ToString("dd.MM.yyyy");
                primech.Content += _ovz.Note.ToString();
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _ovz.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _ovz.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Statuses_OVZ(ovz));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о овз?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_OVZ);
                    string query = $"Delete From Statuses_OVZ Where OVZStatusID = " + ovz.OVZStatusID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_OVZ);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Statuses_OVZ);
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