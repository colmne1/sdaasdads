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

namespace UP.Elements
{
    /// <summary>
    /// Логика взаимодействия для Departments_items.xaml
    /// </summary>
    public partial class Departments_items : UserControl
    {
        ClassModules.Departments Departments;
        public Departments_items(ClassModules.Departments _Departments)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            Departments = _Departments;
            if (_Departments.DepartmentName != null)
            {
                otdel_name.Content = "Отделение №" + _Departments.DepartmentID.ToString();
                name.Content = "Наизвание: " + _Departments.DepartmentName;
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Departments(Departments));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о отделениях?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                    string query = $"Delete Departments сeh Where DepartmentID = " + Departments.DepartmentID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Departments);
                    }
                    else MessageBox.Show("Запрос на удаление отделений не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}