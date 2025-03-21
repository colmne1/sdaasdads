using ClassConnection;
using Microsoft.Win32;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using UP.Classes;
using UP.Pages.Login;

namespace UP.Pages
{
    public partial class Main : Page
    {
        public enum page_main
        {
            Rooms,
            SocialScholarships,
            Statuses_RiskGroup,
            Statuses_Invalid,
            SPPP_Meetings,
            Students,
            Departments,
            Obshaga,
            Statuses_OVZ,
            Statuses_SVO,
            Statuses_Sirots,
            none
        }
        public static page_main page_select;
        public static Main main;
        public Main()
        {
            InitializeComponent();
            main = this;
            page_select = page_main.none;
            FilterDateDatePicker.SelectedDate = DateTime.Today;
        }
        public void CreateConnect(bool connectApply)
        {
            if (connectApply == true)
            {
                Login.Login.connection.LoadData(Connection.Tables.Rooms);
                Login.Login.connection.LoadData(Connection.Tables.SocialScholarships);
                Login.Login.connection.LoadData(Connection.Tables.Statuses_RiskGroup);
                Login.Login.connection.LoadData(Connection.Tables.Statuses_Invalid);
                Login.Login.connection.LoadData(Connection.Tables.SPPP_Meetings);
                Login.Login.connection.LoadData(Connection.Tables.Students);
                Login.Login.connection.LoadData(Connection.Tables.Departments);
                Login.Login.connection.LoadData(Connection.Tables.Obshaga);
                Login.Login.connection.LoadData(Connection.Tables.Statuses_OVZ);
                Login.Login.connection.LoadData(Connection.Tables.Statuses_SVO);
                Login.Login.connection.LoadData(Connection.Tables.Statuses_Sirots);
            }
        }
        public void RoleUser()
        {
            WhoAmI.Content = $"Здравствуйте, {Login.Login.UserInfo[0]}! Роль - {Login.Login.UserInfo[1]}";
        }
        public void OpenPageLogin()
        {
            DoubleAnimation opgridAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.6)
            };
            opgridAnimation.Completed += delegate
            {
                MainWindow.init.frame.Navigate(new Login.Login());
                DoubleAnimation opgrisdAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1.2)
                };
                MainWindow.init.frame.BeginAnimation(Frame.OpacityProperty, opgrisdAnimation);
            };
            MainWindow.init.frame.BeginAnimation(Frame.OpacityProperty, opgridAnimation);
        }
        private void LoadRooms()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.Rooms)
                {
                    if (page_select == page_main.Rooms)
                    {
                        parrent.Children.Add(new Elements.Rooms_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Rooms && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Rooms(new ClassModules.Rooms());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Rooms(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
                page_select = page_main.Rooms;
                parrent.Children.Clear();
                LoadRooms();
            ApplyFilter();

        }

        private void LoadSocialScholarships()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.SocialScholarships)
                {
                    if (page_select == page_main.SocialScholarships)
                    {
                        parrent.Children.Add(new Elements.SocialScholarships_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.SocialScholarships && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.SocialScholarships(new ClassModules.SocialScholarships());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_SocialScholarships(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.SocialScholarships;
            parrent.Children.Clear();
            LoadSocialScholarships();
            ApplyFilter();
        }

        private void LoadStatuses_RiskGroup()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.StatusesRiskGroups)
                {
                    if (page_select == page_main.Statuses_RiskGroup)
                    {
                        parrent.Children.Add(new Elements.Statuses_RiskGroup_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Statuses_RiskGroup && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Statuses_RiskGroup(new ClassModules.Statuses_RiskGroup());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Statuses_RiskGroup(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Statuses_RiskGroup;
            parrent.Children.Clear();
            LoadStatuses_RiskGroup();
            ApplyFilter();
        }

        private void LoadStatuses_Invalid()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.StatusesInvalids)
                {
                    if (page_select == page_main.Statuses_Invalid)
                    {
                        parrent.Children.Add(new Elements.Statuses_Invalid_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Statuses_Invalid && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Statuses_Invalidi(new ClassModules.Statuses_Invalid());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Statuses_Invalid(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Statuses_Invalid;
            parrent.Children.Clear();
            LoadStatuses_Invalid();
            ApplyFilter();
        }

        private void LoadSPPP_Meetings()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.SpppMeetings)
                {
                    if (page_select == page_main.SPPP_Meetings)
                    {
                        parrent.Children.Add(new Elements.SPPP_Meetings_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.SPPP_Meetings && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.SPPP_Meetings(new ClassModules.SPPP_Meetings());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_SPPP_Meetings(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.SPPP_Meetings;
            parrent.Children.Clear();
            LoadSPPP_Meetings();
            ApplyFilter();
        }
        private void LoadStudents()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var students_itm in ClassConnection.Connection.Students)
                {
                    if (page_select == page_main.Students)
                    {
                        parrent.Children.Add(new Elements.Students_items(students_itm));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Students && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Students(new ClassModules.Students());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Students(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Students;
            parrent.Children.Clear();
            LoadStudents();
            ApplyFilter();
        }
        private void LoadDepartments()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.Departments)
                {
                    if (page_select == page_main.Departments)
                    {
                        parrent.Children.Add(new Elements.Departments_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Departments && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Departments(new ClassModules.Departments());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Departments(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Departments;
            parrent.Children.Clear();
            LoadDepartments();
            ApplyFilter();
        }

        private void LoadObshaga()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.Obshagas)
                {
                    if (page_select == page_main.Obshaga)
                    {
                        parrent.Children.Add(new Elements.Obshaga_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Obshaga && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Obshaga(new ClassModules.Obshaga());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Obshaga(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Obshaga;
            parrent.Children.Clear();
            LoadObshaga();
            ApplyFilter();
        }

        private void LoadStatuses_OVZ()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.StatusesOvzs)
                {
                    if (page_select == page_main.Statuses_OVZ)
                    {
                        parrent.Children.Add(new Elements.Statuses_OVZ_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Statuses_OVZ && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Statuses_OVZ(new ClassModules.Statuses_OVZ());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Statuses_OVZ(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Statuses_OVZ;
            parrent.Children.Clear();
            LoadStatuses_OVZ();
            ApplyFilter();
        }

        private void LoadStatuses_SVO()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.StatusesSvos)
                {
                    if (page_select == page_main.Statuses_SVO)
                    {
                        parrent.Children.Add(new Elements.Statuses_SVO_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Statuses_SVO && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Statuses_SVO(new ClassModules.Statuses_SVO());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Statuses_SVO(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Statuses_SVO;
            parrent.Children.Clear();
            LoadStatuses_SVO();
            ApplyFilter();
        }

        private void LoadStatuses_Sirots()
        {
            Dispatcher.InvokeAsync(async () =>
            {
                foreach (var item in ClassConnection.Connection.StatusesSirots)
                {
                    if (page_select == page_main.Statuses_Sirots)
                    {
                        parrent.Children.Add(new Elements.Statuses_Sirots_items(item));
                        await Task.Delay(90);
                    }
                }
                if (page_select == page_main.Statuses_Sirots && Login.Login.UserInfo[1] == "admin")
                {
                    var add = new Pages.PagesInTable.Statuses_Sirots(new ClassModules.Statuses_Sirots());
                    parrent.Children.Add(new Elements.Add(add));
                }
            });
        }
        private void Click_Statuses_Sirots(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible) MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            page_select = page_main.Statuses_Sirots;
            parrent.Children.Clear();
            LoadStatuses_Sirots();
            ApplyFilter();
        }
        private bool isDataLoaded = false;

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            rooms_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            SocialScholarships_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            GroupRisk_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            Invalid_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            Students_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            Zavedenia_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            Obshaga_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            OVZ_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            SVO_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            Sirots_itms.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C2C2C"));
            parrent.Children.Clear();
            page_select = page_main.none;
            Login.Login.UserInfo[0] = ""; Login.Login.UserInfo[1] = "";
            OpenPageLogin();
        }

        public void Animation_move(Control control1, Control control2, Frame frame_main = null, Page pages = null, page_main page_restart = page_main.none)
        {
            if (page_restart != page_main.none)
            {
                if (page_restart == page_main.Rooms)
                {
                    page_select = page_main.none;
                    Click_Rooms(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.SocialScholarships)
                {
                    page_select = page_main.none;
                    Click_SocialScholarships(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Statuses_RiskGroup)
                {
                    page_select = page_main.none;
                    Click_Statuses_RiskGroup(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Statuses_Invalid)
                {
                    page_select = page_main.none;
                    Click_Statuses_Invalid(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.SPPP_Meetings)
                {
                    page_select = page_main.none;
                    Click_SPPP_Meetings(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Students)
                {
                    page_select = page_main.none;
                    Click_Students(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Departments)
                {
                    page_select = page_main.none;
                    Click_Departments(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Obshaga)
                {
                    page_select = page_main.none;
                    Click_Obshaga(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Statuses_OVZ)
                {
                    page_select = page_main.none;
                    Click_Statuses_OVZ(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Statuses_SVO)
                {
                    page_select = page_main.none;
                    Click_Statuses_SVO(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.Statuses_Sirots)
                {
                    page_select = page_main.none;
                    Click_Statuses_Sirots(new object(), new RoutedEventArgs());
                }
            }
            else
            {
                DoubleAnimation opgridAnimation = new DoubleAnimation();
                opgridAnimation.From = 1;
                opgridAnimation.To = 0;
                opgridAnimation.Duration = TimeSpan.FromSeconds(0.3);
                opgridAnimation.Completed += delegate
                {
                    if (pages != null)
                    {
                        frame_main.Navigate(pages);
                    }
                    control1.Visibility = Visibility.Hidden;
                    control2.Visibility = Visibility.Visible;
                    DoubleAnimation opgriAnimation = new DoubleAnimation();
                    opgriAnimation.From = 0;
                    opgriAnimation.To = 1;
                    opgriAnimation.Duration = TimeSpan.FromSeconds(0.4);
                    control2.BeginAnimation(ScrollViewer.OpacityProperty, opgriAnimation);
                };
                control1.BeginAnimation(ScrollViewer.OpacityProperty, opgridAnimation);
            }
        }

        private void Click_Export(object sender, MouseButtonEventArgs e)
        {
            var export = new ExpWindow();
            export.ShowDialog();
        }
        private void ApplyFilter()
        {
            if (page_select != page_main.Students)
            {
                filterpanel.Visibility = Visibility.Hidden;
                return;
            }
            else
            {
                filterpanel.Visibility = Visibility.Visible;
                parrent.Children.Clear(); // Очищаем текущий список

                // 1. Получаем базовый набор студентов (все)
                IEnumerable<ClassModules.Students> filteredStudents = ClassConnection.Connection.Students;

                // 2. Применяем фильтры (как в предыдущем примере)
                // Вместо DateTime filterDate - не используется в коде, удален.
                //DateTime filterDate = FilterDateDatePicker.SelectedDate ?? DateTime.Today;

                // Используем nullable int, чтобы корректно обрабатывать отсутствие значений в TextBox
                int? yearPostup = null;
                int? yearOtchiz = null;

                // Получаем значения из TextBox, если они заполнены, и пытаемся преобразовать в int.
                if (!string.IsNullOrEmpty(FilterPostupDatePicker.Text))
                {
                    if (int.TryParse(FilterPostupDatePicker.Text, out int parsedYear))
                    {
                        yearPostup = parsedYear;
                    }
                    else
                    {
                        // Обработка ошибки: TextBox содержит некорректное значение (не число)
                        MessageBox.Show("Некорректный формат года поступления: " + FilterPostupDatePicker.Text);
                    }
                }

                if (!string.IsNullOrEmpty(FilterOtchisDatePicker.Text))
                {
                    if (int.TryParse(FilterOtchisDatePicker.Text, out int parsedYear))
                    {
                        yearOtchiz = parsedYear;
                    }
                    else
                    {
                        // Обработка ошибки: TextBox содержит некорректное значение (не число)
                        MessageBox.Show("Некорректный формат года отчисления: " + FilterOtchisDatePicker.Text);
                    }
                }

                string lastNameFilter = FilterLastNameTextBox.Text?.Trim();
                string groupFilter = FilterGroupTextBox.Text?.Trim();
                string roomFilter = FilterRoomTextBox.Text?.Trim();

                // Фильтрация по фамилии и группе
                if (!string.IsNullOrEmpty(lastNameFilter))
                {
                    filteredStudents = filteredStudents.Where(s => s.LastName.ToLower().Contains(lastNameFilter.ToLower()));
                }

                if (!string.IsNullOrEmpty(groupFilter))
                {
                    filteredStudents = filteredStudents.Where(s => s.Groups.ToLower().Contains(groupFilter.ToLower()));
                }

                // Фильтрация по CheckBox
                if (FilterSPPPCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.SpppMeetings.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterVzyskCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => s.Vziskanie != null);
                }

                if (FilterSirotsCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.StatusesSirots.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterInvalidCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.StatusesInvalids.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterOVZCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.StatusesOvzs.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterSVOCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.StatusesSvos.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterSocialCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.SocialScholarships.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterRiskCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => ClassConnection.Connection.StatusesRiskGroups.Any(sp => sp.StudentID == s.StudentID));
                }

                if (FilterSovershenCheckBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => DateTime.Now.Year - s.BirthDate.Year >= 18);
                }

                if (FilterOtchizBox.IsChecked == true)
                {
                    filteredStudents = filteredStudents.Where(s => s.DateOtchiz == null);
                }

                if (!string.IsNullOrEmpty(roomFilter))
                {
                    //Преобразовываем roomFilter в int? и используем для сравнения
                    if (int.TryParse(roomFilter, out int roomNumber))
                    {
                        filteredStudents = filteredStudents.Where(s => Connection.Obshagas.Any(o => o.StudentID == s.StudentID && o.RoomNumber == roomNumber));
                    }
                    else
                    {
                        //Обработка случая, когда roomFilter не является числом (например, логирование ошибки)
                        Console.WriteLine("Некорректный формат номера комнаты: " + roomFilter);
                    }
                }

                // Фильтрация по году поступления (год из TextBox)
                if (!String.IsNullOrEmpty(yearPostup.ToString()))
                {
                    filteredStudents = filteredStudents.Where(s => s.YearPostup == yearPostup.Value);
                }

                // Фильтрация по году отчисления (год из TextBox)
                if (!String.IsNullOrEmpty(yearOtchiz.ToString()))
                {
                    filteredStudents = filteredStudents.Where(s => s.YearOkonch == yearOtchiz.Value);
                }

                // Пол
                string genderFilter = (FilterGenderComboBox.SelectedItem as ComboBoxItem)?.Content as string;
                if (genderFilter == "Мужской")
                {
                    filteredStudents = filteredStudents.Where(s => s.Gender == "Male"); // Исправлено: Мужской -> Male
                }
                else if (genderFilter == "Женский")
                {
                    filteredStudents = filteredStudents.Where(s => s.Gender == "Female"); // Исправлено: Женский -> Female
                }

                // Отображаем отфильтрованные данные
                Dispatcher.InvokeAsync(async () =>
                {
                    // Очищаем контейнер parrent перед добавлением новых элементов
                    parrent.Children.Clear(); // Важно добавить это!

                    foreach (var student in filteredStudents)
                    {
                        if (page_select == page_main.Students)
                        {
                            parrent.Children.Add(new Elements.Students_items(student)); // Используйте ваш метод для отображения студента
                            await Task.Delay(90); // Задержка может быть проблемой с производительностью
                        }
                    }
                });
            }
        }

        // Обработчики событий
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void Filter_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

    }
}