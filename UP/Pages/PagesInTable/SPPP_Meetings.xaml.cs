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
    /// Логика взаимодействия для SPPP_Meetings.xaml
    /// </summary>
    public partial class SPPP_Meetings : Page
    {
        ClassModules.SPPP_Meetings sppp;
        public SPPP_Meetings(ClassModules.SPPP_Meetings _sppp)
        {
            InitializeComponent();
            sppp = _sppp;
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
                student.Items.Add(cb_otdel);

                RentStart.Text = _sppp.Date.ToString();
                osnVizov.Text = _sppp.OsnVizov;
                sotrud.Text = _sppp.Sotrudniki;
                predstav.Text = _sppp.Predstaviteli;
                prichVizov.Text = _sppp.ReasonForCall;
                resh.Text = _sppp.Reshenie;
                primech.Text = _sppp.Note;
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
        private void Click_SPPP_Meetings_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Students Id_student_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.SPPP_Meetings);
            if (sppp.OsnVizov == null)
            {
                string query = $"Insert Into SPPP_Meetings ([MeetingID], [StudentID], [Date], [OsnVizov], [Sotrudniki], [Predstaviteli], [ReasonForCall], [Reshenie], [Note], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{RentStart.Text}', '{osnVizov.Text}', '{sotrud.Text}', '{predstav.Text}', '{prichVizov.Text}', '{resh.Text}', '{primech.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SPPP_Meetings);
                }
                else MessageBox.Show("Запрос на добавление СППП не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update SPPP_Meetings Set [StudentID] = N'{Id_student_temp.StudentID.ToString()}', [Date] = N'{RentStart.Text}', [OsnVizov] = N'{osnVizov.Text}', [Sotrudniki] = N'{sotrud.Text}', [Predstaviteli] = N'{predstav.Text}', [ReasonForCall] = N'{prichVizov.Text}', [Reshenie] = N'{resh.Text}', [Note] = N'{primech.Text}', [Files] = '{fileContent}' Where [MeetingID] = {sppp.MeetingID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SPPP_Meetings);
                }
                else MessageBox.Show("Запрос на изменение СППП не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_SPPP_Meetings_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_SPPP_Meetings_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                string query = "Delete SPPP_Meetings Where [MeetingID] = " + sppp.MeetingID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.SPPP_Meetings);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.SPPP_Meetings);
                }
                else MessageBox.Show("Запрос на удаление СППП не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
