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
using System.Windows.Shapes;
using UP.Elements;

namespace UP.Pages
{
    /// <summary>
    /// Логика взаимодействия для ExpWindow.xaml
    /// </summary>
    public partial class ExpWindow : Window
    {
        public ExpWindow()
        {
            InitializeComponent();
        }
        private void GenerateObshagaReport_Click(object sender, RoutedEventArgs e)
        {
            ReportContent.Content = new ObshagaReportControl();
        }

        private void GenerateStudentReport_Click(object sender, RoutedEventArgs e)
        {
            ReportContent.Content = new StudentReportControl();
        }

        private void GenerateLgotnikiReport_Click(object sender, RoutedEventArgs e)
        {
            ReportContent.Content = new LgotnikiReportControl();
        }
    }
}
