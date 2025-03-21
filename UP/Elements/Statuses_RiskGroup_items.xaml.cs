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
    /// Логика взаимодействия для Statuses_RiskGroup_items.xaml
    /// </summary>
    public partial class Statuses_RiskGroup_items : UserControl
    {
        ClassModules.Statuses_RiskGroup risk;
        public Statuses_RiskGroup_items(ClassModules.Statuses_RiskGroup _risk)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            risk = _risk;
            if (_risk.RiskGroupType != null)
            {
                groupRisk_name.Content += _risk.RiskGroupID.ToString();
                dateUch.Content += _risk.DateStart.ToString("dd.MM.yyyy");
                dateSnat.Content += _risk.DateEnd.ToString("dd.MM.yyyy");
                osnUch.Content += _risk.OsnPost.ToString();
                osnSnat.Content += _risk.OsnSnat.ToString();
                prichPost.Content += _risk.PrichinaPost.ToString();
                prichSnat.Content += _risk.PrichinaSnat.ToString();
                primech.Content += _risk.Note.ToString();
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _risk.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _risk.StudentID).FirstName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Statuses_RiskGroup(risk));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о Группа риска/СОП?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                    string query = $"Delete From Statuses_RiskGroup Where RiskGroupID = " + risk.RiskGroupID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Statuses_RiskGroup);
                    }
                    else MessageBox.Show("Запрос на удаление группы риска/СОП не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
