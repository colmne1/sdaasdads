using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using UP.Classes;
using static UP.Classes.Report;
namespace UP.Elements
{
    /// <summary>
    /// Логика взаимодействия для ObshagaReportControl.xaml
    /// </summary>
    public partial class ObshagaReportControl : UserControl
    {
        public ObshagaReportControl()
        {
            InitializeComponent();
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            string familiaFilter = FamiliaFilterTextBox.Text;
            DateTime? nachaloPerioda = NachaloPeriodaDatePicker.SelectedDate;
            DateTime? konecPerioda = KonecPeriodaDatePicker.SelectedDate;
            string gruppaFilter = GruppaFilterTextBox.Text;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    Report.GenerateReport(saveFileDialog.FileName, familiaFilter, nachaloPerioda, konecPerioda, gruppaFilter);
                    MessageBox.Show("Отчет успешно сгенерирован и сохранен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}");
                }
            }
        }
    }
}