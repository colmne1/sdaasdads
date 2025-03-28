﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static Report2;

namespace UP.Elements
{
    /// <summary>
    /// Логика взаимодействия для StudentReportControl.xaml
    /// </summary>
    public partial class StudentReportControl : UserControl
    {
        public StudentReportControl()
        {
            InitializeComponent();
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StudentIDTextBox.Text, out int studentId))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        Report2.GenerateReport(saveFileDialog.FileName, studentId);
                        MessageBox.Show("Отчет успешно сгенерирован и сохранен!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный ID студента.");
            }
        }
    }
}


