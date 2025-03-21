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
    /// Логика взаимодействия для Statuses_Invalidi.xaml
    /// </summary>
    public partial class Statuses_Invalidi : Page
    {
        ClassModules.Statuses_Invalid invalid;
        public Statuses_Invalidi(ClassModules.Statuses_Invalid _invalid)
        {
            InitializeComponent();
            invalid = _invalid;
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
                student.Items.Add(cb_otdel);
                prikaz.Text = _invalid.OrderNumber;
                primech.Text = _invalid.Note;
                vidInvalid.Text = _invalid.DisabilityType;
                nachStat.SelectedDate = _invalid.StartDate;
                konStat.SelectedDate = _invalid.EndDate;
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
        private void Click_Statuses_Invalidi_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Students Id_student_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Statuses_Invalid);
            if (invalid.OrderNumber == null)
            {
                string query = $"Insert Into Statuses_Invalid ([DisabilityStatusID], [StudentID], [OrderNumber], [StartDate], [EndDate], [DisabilityType], [Note], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{prikaz.Text}', '{nachStat.Text}', '{konStat.Text}', '{vidInvalid.Text}', '{primech.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_Invalid);
                }
                else MessageBox.Show("Запрос на добавление инвалида не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update Statuses_Invalid Set [StudentID] = N'{Id_student_temp.StudentID.ToString()}', [OrderNumber] = N'{prikaz.Text}', [StartDate] = N'{nachStat.Text}', [EndDate] = N'{konStat.Text}', [DisabilityType] = N'{vidInvalid.Text}', [Note] = N'{primech.Text}', [Files] = '{fileContent}' Where [MeetingID] = {invalid.DisabilityStatusID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_Invalid);
                }
                else MessageBox.Show("Запрос на изменение инвалида не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_Statuses_Invalidi_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Statuses_Invalidi_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                string query = "Delete Statuses_Invalid Where [DisabilityStatusID] = " + invalid.DisabilityStatusID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_Invalid);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_Invalid);
                }
                else MessageBox.Show("Запрос на удаление инвалида не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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