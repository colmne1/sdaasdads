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
    /// Логика взаимодействия для SocialScholarships_items.xaml
    /// </summary>
    public partial class SocialScholarships_items : UserControl
    {
        ClassModules.SocialScholarships social;
        public SocialScholarships_items(ClassModules.SocialScholarships _social)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            social = _social;
            if (_social.DocumentReason != null)
            {
                soc_name.Content += _social.ScholarshipID.ToString();
                docOsn.Content += _social.DocumentReason.ToString();
                nachVipl.Content += _social.StartDate.ToString("dd.MM.yyyy");
                konVipl.Content += _social.EndDate.ToString("dd.MM.yyyy");
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _social.StudentID).LastName + " " + Connection.Students.FirstOrDefault(x => x.StudentID == _social.StudentID).FirstName;
            }
        }

        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.SocialScholarships(social));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о социально стипендии?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                    string query = $"Delete From SocialScholarships Where ScholarshipID = " + social.ScholarshipID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.SocialScholarships);
                    }
                    else MessageBox.Show("Запрос на удаление социальной стипендии не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
