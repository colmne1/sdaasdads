using ClassConnection;
using Microsoft.Win32;
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

namespace UP.Pages.PagesInTable
{
    /// <summary>
    /// Логика взаимодействия для Obshaga.xaml
    /// </summary>
    public partial class Obshaga : Page
    {
        ClassModules.Obshaga obshaga;
        public Obshaga(ClassModules.Obshaga _obshaga)
        {
            InitializeComponent();
            obshaga = _obshaga;
            foreach (var item in Connection.Rooms)
            {
                ComboBoxItem cb_room = new ComboBoxItem();
                cb_room.Tag = item.RoomID;
                cb_room.Content = "Название комнаты: " + item.RoomName;
                RentStart.Text = _obshaga.CheckInDate.ToString();
                RentOut.Text = _obshaga.CheckOutDate.ToString();
                primech.Text = _obshaga.Note;
                room.Items.Add(cb_room);
            }
            foreach (var item in Connection.Students)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.StudentID;
                cb_otdel.Content = item.FirstName;
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
        private void Click_Obshaga_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
               MessageBox.Show("Пожалуйста, выберите файл.");
               return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);

            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Departments);
            ClassModules.Students Id_student_temp;
            ClassModules.Rooms Id_room_temp;
            Id_student_temp = ClassConnection.Connection.Students.Find(x => x.StudentID == Convert.ToInt32(((ComboBoxItem)student.SelectedItem).Tag));
            Id_room_temp = ClassConnection.Connection.Rooms.Find(x => x.RoomID == Convert.ToInt32(((ComboBoxItem)room.SelectedItem).Tag));
            if (obshaga.CheckInDate == null)
            {
                string query = $"Insert Into Obshaga ([DormitoryID], [StudentID], [RoomNumber], [CheckInDate], [CheckOutDate], [Note], [Files]) Values ({id.ToString()}, '{Id_student_temp.StudentID.ToString()}', '{Id_room_temp.RoomID.ToString()}', '{RentStart.ToString()}', '{RentOut.ToString()}', '{primech.Text}', '{fileContent}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Obshaga);
                }
                else MessageBox.Show("Запрос на добавление общежития не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update Obshaga Set [StudentID], [RoomNumber], [CheckInDate], [CheckOutDate], [Note] = N'{Id_student_temp.StudentID.ToString()}', '{room.Text}', '{RentStart.ToString()}', '{RentOut.ToString()}', '{primech.Text}', [Files] = '{fileContent}' Where [DormitoryID] = {obshaga.DormitoryID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Obshaga);
                }
                else MessageBox.Show("Запрос на изменение общежития не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Click_Cancel_Obshaga_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Obshaga_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                string query = "Delete Obshaga Where [DormitoryID] = " + obshaga.DormitoryID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Departments);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Obshaga);
                }
                else MessageBox.Show("Запрос на удаление общежития не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
