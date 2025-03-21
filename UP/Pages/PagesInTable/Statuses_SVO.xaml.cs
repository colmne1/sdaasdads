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
    /// Логика взаимодействия для Statuses_SVO.xaml
    /// </summary>
    public partial class Statuses_SVO : Page
    {
        ClassModules.Statuses_SVO svo;
        public Statuses_SVO(ClassModules.Statuses_SVO _svo)
        {
            InitializeComponent();
            svo = _svo;
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
                prikaz.Text = _svo.DocumentOsnov;
                nachStat.Text = _svo.StartDate.ToString();
                konStat.Text = _svo.EndDate.ToString();
                student.Items.Add(cb_otdel);
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

        private void Click_Statuses_SVO_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Students Id_student_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Statuses_Sirots);
            if (svo.DocumentOsnov == null)
            {
                string query = $"Insert Into Statuses_SVO ([SVOStatusID], [StudentID], [DocumentOsnov], [StartDate], [EndDate], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{prikaz.Text}', '{nachStat.Text}', '{konStat.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_SVO);
                }
                else MessageBox.Show("Запрос на добавление сво не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update Statuses_SVO Set [StudentID] = N'{Id_student_temp.StudentID.ToString()}', [DocumentOsnov] = N'{prikaz.Text}', [StartDate] = N'{nachStat.Text}', [EndDate] = N'{konStat.Text}', [Files] = '{fileContent}' Where [SVOStatusID] = {svo.SVOStatusID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_SVO);
                }
                else MessageBox.Show("Запрос на изменение сво не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_Statuses_SVO_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Statuses_SVO_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                string query = "Delete Statuses_SVO Where [SVOStatusID] = " + svo.SVOStatusID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_SVO);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_SVO);
                }
                else MessageBox.Show("Запрос на удаление сво не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
