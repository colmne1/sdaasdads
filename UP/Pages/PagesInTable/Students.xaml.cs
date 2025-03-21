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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassConnection;
using Microsoft.Win32;

namespace UP.Pages.PagesInTable
{
    /// <summary>
    /// Логика взаимодействия для Students.xaml
    /// </summary>
    public partial class Students : Page
    {
        ClassModules.Students students;
        public Students(ClassModules.Students _students)
        {
            InitializeComponent();
            students = _students;
            foreach (var item in Connection.Departments)
            {
                ComboBoxItem cb_otdel = new ComboBoxItem();
                cb_otdel.Tag = item.DepartmentID;
                cb_otdel.Content = item.DepartmentName;
                Family.Text = _students.LastName;
                Name.Text = _students.FirstName;
                octh.Text = _students.MiddleName;
                dateBrth.Text = _students.BirthDate.ToString();
                pol.Text = _students.Gender;
                kontNomer.Text = _students.ContactNumber;
                obraz.Text = _students.Obrazovanie;
                finance.Text = _students.Finance;
                godPostup.Text = _students.YearPostup.ToString();
                godOkonch.Text = _students.YearOkonch.ToString();
                infoOtchis.Text = _students.InfoOtchiz;
                dateOtchiz.SelectedDate = _students.DateOtchiz;
                primech.Text = _students.Note;
                svORodit.Text = _students.ParentsInfo;
                vziskanie.Text = _students.Vziskanie;
                otdel.Items.Add(cb_otdel);
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
        private void Click_Students_Redact(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Пожалуйста, выберите файл.");
                return;
            }
            string fileContent = File.ReadAllText(selectedFilePath);
            ClassModules.Departments Id_departments_temp;
            Id_departments_temp = ClassConnection.Connection.Departments.Find(x => x.DepartmentID == Convert.ToInt32(((ComboBoxItem)otdel.SelectedItem).Tag));
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Students);
            Regex regex = new Regex(@"^\+7\(\d{3}\)-\d{3}-\d{2}-\d{2}$");
            if (regex.IsMatch(kontNomer.Text))
            {
                e.Handled = true;

                if (students.LastName == null)
                {
                    string query = $"Insert Into Students ([StudentID], [LastName], [FirstName], [MiddleName], [BirthDate], [Gender], [ContactNumber], [Obrazovanie], [Otdelenie], [Groups], [Finance], [YearPostup], [YearOkonch], [InfoOtchiz], [DateOtchiz], [Note], [ParentsInfo], [Vziskanie], [Files]) Values({id.ToString()}, '{Family.Text}', '{Name.Text}', '{octh.Text}', '{dateBrth.Text}', '{pol.Text}', '{kontNomer.Text}', '{obraz.Text}', '{Id_departments_temp.DepartmentID.ToString()}', '{group.Text}', '{finance.Text}', '{godPostup.Text}', '{godOkonch.Text}', '{infoOtchis.Text}', '{dateOtchiz.Text}', '{primech.Text}', '{svORodit.Text}', '{vziskanie.Text}', '{fileContent}')";
                    var query_apply = Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Students);
                    }
                    else MessageBox.Show("Запрос на добавление студента не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string query = $"Update Students Set [LastName] = '{Family.Text}', [FirstName] = '{Name.Text}', [MiddleName] = '{octh.Text}', [BirthDate] = '{dateBrth.Text}', [Gender] = '{pol.Text}', [ContactNumber] = '{kontNomer.Text}', [Obrazovanie] = '{obraz.Text}', [Otdelenie] = '{Id_departments_temp.DepartmentID.ToString()}', [Groups] = '{group.Text}', [Finance] = '{finance.Text}', [YearPostup] = '{godPostup.Text}', [YearOkonch] = '{godOkonch.Text}', [InfoOtchiz] = '{infoOtchis.Text}', [DateOtchiz] = '{dateOtchiz.Text}', [Note] = '{primech.Text}', [ParentsInfo] = '{svORodit.Text}', [Vziskanie] = '{vziskanie.Text}', [Files] = '{fileContent}' Where [StudentID] = {students.StudentID}";
                    var query_apply = Login.Login.connection.ExecuteQuery(query);
                    if (query_apply != null)
                    {
                        Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                        MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Students);
                    }
                    else MessageBox.Show("Запрос на изменение студента не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Неправильный ввод контактного номера");
            }
        }

        private void Click_Cancel_Students_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Students_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                string query = "Delete Students Where [StudentID] = " + students.StudentID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Students);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Students);
                }
                else MessageBox.Show("Запрос на удаление студента не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void TextBox_LostFocus_3(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] words = textBox.Text.Split(' ');
            if (words.Any(word => word.Length == 0))
            {
                textBox.Text = "Ошибка: введите 1 слово";
                Family.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
            }
        }

        private void TextBox_GotFocus_3(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.StartsWith("Ошибка:"))
            {
                textBox.Text = "";
                ColorAnimation animation = new ColorAnimation();
                animation.From = (Color)ColorConverter.ConvertFromString("#FB3F51");
                animation.To = Colors.Transparent;
                animation.Duration = new Duration(TimeSpan.FromSeconds(2));
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Family.BorderBrush = brush;
            }
        }
        private void TextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] words = textBox.Text.Split(' ');
            if (words.Any(word => word.Length == 0))
            {
                textBox.Text = "Ошибка: введите 1 слово";
                Family.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
            }
        }

        private void TextBox_GotFocus_2(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.StartsWith("Ошибка:"))
            {
                textBox.Text = "";
                ColorAnimation animation = new ColorAnimation();
                animation.From = (Color)ColorConverter.ConvertFromString("#FB3F51");
                animation.To = Colors.Transparent;
                animation.Duration = new Duration(TimeSpan.FromSeconds(2));
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Family.BorderBrush = brush;
            }
        }
        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] words = textBox.Text.Split(' ');
            if (words.Any(word => word.Length == 0))
            {
                textBox.Text = "Ошибка: введите 1 слово";
                Family.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
            }
        }
        private void TextBox_LostFocus_4(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string[] words = textBox.Text.Split(' ');
            if (words.Any(word => word.Length == 0))
            {
                textBox.Text = "Ошибка: введите 1 слово";
                Family.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
            }
        }
        private void TextBox_GotFocus_4(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.StartsWith("Ошибка:"))
            {
                textBox.Text = "";
                ColorAnimation animation = new ColorAnimation();
                animation.From = (Color)ColorConverter.ConvertFromString("#FB3F51");
                animation.To = Colors.Transparent;
                animation.Duration = new Duration(TimeSpan.FromSeconds(2));
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Family.BorderBrush = brush;
            }
        }
        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.StartsWith("Ошибка:"))
            {
                textBox.Text = "";
                ColorAnimation animation = new ColorAnimation();
                animation.From = (Color)ColorConverter.ConvertFromString("#FB3F51");
                animation.To = Colors.Transparent;
                animation.Duration = new Duration(TimeSpan.FromSeconds(2));
                SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB3F51"));
                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Family.BorderBrush = brush;
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
        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[A-zА-я]*$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        //private void TextBox_Number(object sender, TextCompositionEventArgs e)
        //{
        //Regex regex = new Regex(@"^\+7\(\d{3}\)-\d{3}-\d{2}-\d{2}$");
        //    if (!regex.IsMatch(e.Text))
        //    {
        //        e.Handled = true;
        //    }
        //}

        private void TextBox_Data1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[1-9]$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
