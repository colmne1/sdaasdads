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
    /// Логика взаимодействия для Statuses_Sirots_items.xaml
    /// </summary>
    public partial class Statuses_Sirots_items : UserControl
    {
        ClassModules.Statuses_Sirots sirots;
        public Statuses_Sirots_items(ClassModules.Statuses_Sirots _sirots)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            sirots = _sirots;
            if (_sirots.OrderNumber != null)
            {
                sirot_name.Content += _sirots.OrphanStatusID.ToString();
                prikaz.Content += _sirots.OrderNumber;
                nachStat.Content += _sirots.StartDate.ToString("dd.MM.yyyy");
                konStat.Content += _sirots.EndDate.ToString("dd.MM.yyyy");
                primech.Content += _sirots.Note.ToString();
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _sirots.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _sirots.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Statuses_Sirots(sirots));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о овз?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Sirots);
                    string query = $"Delete From Statuses_Sirots Where OrphanStatusID = " + sirots.OrphanStatusID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Sirots);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Statuses_Sirots);
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