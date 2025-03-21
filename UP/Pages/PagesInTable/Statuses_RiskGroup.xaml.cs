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
    /// Логика взаимодействия для Statuses_RiskGroup.xaml
    /// </summary>
    public partial class Statuses_RiskGroup : Page
    {
        ClassModules.Statuses_RiskGroup risk;
        public Statuses_RiskGroup(ClassModules.Statuses_RiskGroup _risk)
        {
            InitializeComponent();
            risk = _risk;
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
                tip.Text = _risk.RiskGroupType;
                nachUch.Text = _risk.DateStart.ToString();
                konUch.Text = _risk.DateEnd.ToString();
                osnUch.Text = _risk.OsnPost;
                osnSnatUch.Text = _risk.OsnSnat;
                prichUch.Text = _risk.PrichinaPost;
                prichSnUch.Text = _risk.PrichinaSnat;
                primech.Text = _risk.Note;
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
        private void Click_Statuses_RiskGroup_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Students Id_student_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Statuses_RiskGroup);
            if (risk.RiskGroupType == null)
            {
                string query = $"Insert Into Statuses_RiskGroup ([RiskGroupID], [StudentID], [RiskGroupType], [DateStart], [DateEnd], [OsnPost], [OsnSnat], [PrichinaPost], [PrichinaSnat], [Note], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{tip.Text}', '{nachUch.Text}', '{konUch.Text}', '{osnUch.Text}', '{osnSnatUch.Text}', '{prichUch.Text}', '{prichSnUch.Text}', '{primech.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_RiskGroup);
                }
                else MessageBox.Show("Запрос на добавление группы риска не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update Statuses_RiskGroup Set [StudentID] = N'{Id_student_temp.StudentID.ToString()}', [RiskGroupType] = N'{tip.Text}', [DateStart] = N'{nachUch.Text}', [DateEnd] = N'{konUch.Text}', [OsnPost] = N'{osnUch.Text}', [OsnSnat] = N'{osnSnatUch.Text}', [PrichinaPost] = N'{prichUch.Text}', [PrichinaSnat] = N'{prichSnUch.Text}', [Note] = N'{primech.Text}', [Files] = '{fileContent}' Where [RiskGroupID] = {risk.RiskGroupID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_RiskGroup);
                }
                else MessageBox.Show("Запрос на изменение группы риска не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_Statuses_RiskGroup_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Statuses_RiskGroup_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                string query = "Delete Statuses_RiskGroup Where [RiskGroupID] = " + risk.RiskGroupID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Statuses_RiskGroup);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Statuses_RiskGroup);
                }
                else MessageBox.Show("Запрос на удаление группы риска не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
