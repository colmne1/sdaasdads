using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Rooms.xaml
    /// </summary>
    public partial class Rooms : Page
    {
        ClassModules.Rooms rooms;
        public Rooms(ClassModules.Rooms _rooms)
        {
            InitializeComponent();
            rooms = _rooms;
            if (_rooms.RoomName != null)
            {

                nameroom.Text = _rooms.RoomName;
                vmestim.Text = _rooms.Vmestim;
            }
        }

        private void Click_Rooms_Redact(object sender, RoutedEventArgs e)
        {
            int id = Login.Login.connection.SetLastId(ClassConnection.Connection.Tables.Rooms);
            if (rooms.RoomName == null)
            {
                string query = $"Insert Into Rooms ([RoomID], [RoomName], [Vmestim]) Values ('{id.ToString()}', '{nameroom.Text}','{vmestim.Text}')";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Rooms);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Rooms);
                }
                else MessageBox.Show("Запрос на добавление комнаты не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string query = $"Update Rooms Set [RoomName] = '{nameroom.Text}', [Vmestim] = '{vmestim.Text}' Where [RoomID] = {rooms.RoomID}";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Rooms);
                    MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Rooms);
                }
                else MessageBox.Show("Запрос на изменение комнаты не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Click_Cancel_Rooms_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Rooms_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Rooms);
                string query = "Delete Rooms Where [RoomID] = " + rooms.RoomID.ToString() + "";
                var query_apply = Login.Login.connection.ExecuteQuery(query);
                if (query_apply != null)
                {
                    Login.Login.connection.LoadData(ClassConnection.Connection.Tables.Rooms);
                    Main.main.Animation_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.Rooms);
                }
                else MessageBox.Show("Запрос на удаление комнаты не был обработан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void TextBox_Chisla(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[1-9]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
