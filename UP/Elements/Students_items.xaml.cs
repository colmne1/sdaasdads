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
    /// Логика взаимодействия для Students_items.xaml
    /// </summary>
    public partial class Students_items : UserControl
    {
        ClassModules.Students student;
        public Students_items(ClassModules.Students _student)
        {
            InitializeComponent();
            if (Pages.Login.Login.UserInfo[1] != "admin") Buttons.Visibility = Visibility.Hidden;
            student = _student;
            if (_student.LastName != null)
            {
                student_name.Content += _student.StudentID.ToString();
                lastName.Content += _student.LastName.ToString();
                firstName.Content += _student.FirstName.ToString();
                midName.Content += _student.MiddleName.ToString();
                dateBrth.Content += _student.BirthDate.ToString("dd.MM.yyyy");
                gender.Content += _student.Gender.ToString();
                nomer.Content += _student.ContactNumber.ToString();
                obrazovanie.Content += _student.Obrazovanie.ToString();
                otdel.Content += Connection.Departments.FirstOrDefault(x => x.DepartmentID == _student.Otdelenie).DepartmentName;
                group.Content += _student.Groups.ToString();
                finance.Content += _student.Finance.ToString();
                godPostup.Content += _student.YearPostup.ToString();
                godOkonch.Content += _student.YearOkonch.ToString();
                infoOOtchis.Content += _student.InfoOtchiz.ToString();
                dataOtchis.Content += _student.DateOtchiz.ToString("dd.MM.yyyy");
                primech.Content += _student.Note.ToString();
                svORod.Content += _student.ParentsInfo.ToString();
                vziskan.Content += _student.Vziskanie.ToString();
            }
        }
        private void Click_redact(object sender, RoutedEventArgs e) => MainWindow.main.Animation_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesInTable.Students(student));

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить информацию о студентах?", "Удаление информации", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                    string query = $"Delete From Students Where StudentID = " + student.StudentID.ToString() + "";
                    var query_apply = Pages.Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Pages.Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Pages.Main.page_main.Students);
                    }
                    else MessageBox.Show("Запрос на удаление студентов не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}