using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;

namespace UP.Pages.PagesInTable
{
    /// <summary>
    /// Логика взаимодействия для SocialScholarships.xaml
    /// </summary>
    public partial class SocialScholarships : Page
    {
        ClassModules.SocialScholarships social;
        public SocialScholarships(ClassModules.SocialScholarships _social)
        {
            InitializeComponent();
            social = _social;
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
                student.Items.Add(cb_otdel);

                docOsn.Text = _social.DocumentReason;
                nachVipl.Text = _social.StartDate.ToString();
                konVipl.Text = _social.EndDate.ToString();
            }
        }
        private string selectedFilePath;
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"; // Фильтр файлов
            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
                FilePathTextBox.Text = selectedFilePath;
            }
        }
        private void Click_SocialScholarships_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Students Id_student_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.SocialScholarships);
            if (social.DocumentReason == null)
            {
                string query = $"Insert Into SocialScholarships ([ScholarshipID], [StudentID], [DocumentReason], [StartDate], [EndDate], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{docOsn.Text}', '{nachVipl.Text}', '{konVipl.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SocialScholarships);
                }
                else MessageBox.Show("Запрос на добавление стипендии не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update SocialScholarships Set [StudentID] = N'{Id_student_temp.StudentID.ToString()}', [DocumentReason] = N'{docOsn.Text}', [StartDate] = N'{nachVipl.Text}', [EndDate] = N'{konVipl.Text}', [Files] = '{fileContent}' Where [ScholarshipID] = {social.ScholarshipID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SocialScholarships);
                }
                else MessageBox.Show("Запрос на изменение стипендии не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_SocialScholarships_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_SocialScholarships_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                string query = "Delete SocialScholarships Where [ScholarshipID] = " + social.ScholarshipID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SocialScholarships);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SocialScholarships);
                }
                else MessageBox.Show("Запрос на удаление стипендии не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void TextBox_Data(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d{4}(-)\d{2}\1\d{2}$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

    }
}
