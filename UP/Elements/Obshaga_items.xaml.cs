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
    /// Логика взаимодействия для Obshaga_items.xaml
    /// </summary>
    public partial class Obshaga_items : UserControl
    {
        ClassModules.Obshaga obshaga;
        public Obshaga_items(ClassModules.Obshaga _obshaga)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            obshaga = _obshaga;
            if (_obshaga.CheckInDate != null)
            {
                obshaga_name.Content += _obshaga.DormitoryID.ToString();
                room.Content +=  _obshaga.RoomNumber.ToString();
                dateZas.Content += _obshaga.CheckInDate.ToString("dd.MM.yyyy");
                dateVis.Content += _obshaga.CheckOutDate.ToString("dd.MM.yyyy");
                primech.Content += _obshaga.Note;
                student.Content += Connection.Students.FirstOrDefault(x => x.StudentID == _obshaga.StudentID).LastName + " "+ Connection.Students.FirstOrDefault(x => x.StudentID == _obshaga.StudentID).FirstName;

            }
        }

        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Obshaga(obshaga));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о общежитии?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Obshaga);
                    string query = $"Delete From Obshaga Where DormitoryID = " + obshaga.DormitoryID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Obshaga);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Obshaga);
                    }
                    else MessageBox.Show("Запрос на удаление общежития не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
