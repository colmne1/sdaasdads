using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using ClassConnection;
using ClassModules;
using System.Linq; // Убедитесь, что вы включили это!

public class Report3
{
    public class Report3Data
    {
        public string FIO { get; set; }
        public string DataRozhdeniya { get; set; }
        public string Gruppa { get; set; }
        public string KontaktnyNomer { get; set; }
        public string Finansirovanie { get; set; }
        public string Obrazovanie { get; set; }
        public string StatusSirota { get; set; }
        public string StatusInvalid { get; set; }
        public string StatusOVZ { get; set; }
        public string DocumentOsnovanie1 { get; set; } // First "Document Osnovanie" column
        public string DocumentOsnovanie2 { get; set; } // Second "Document Osnovanie" column
        public string Otchislenie { get; set; }
    }

    public static List<Report3Data> GetData()
    {
        // Создайте экземпляр класса Connection
        Connection dbConnection = new Connection(); //Создаем экземпляр
        Connection.Connect();

        //Загружаем нужные таблицы
        dbConnection.LoadData(Connection.Tables.Students);
        dbConnection.LoadData(Connection.Tables.Statuses_Sirots);
        dbConnection.LoadData(Connection.Tables.Statuses_Invalid);
        dbConnection.LoadData(Connection.Tables.Statuses_OVZ);
        dbConnection.LoadData(Connection.Tables.SocialScholarships);
        dbConnection.LoadData(Connection.Tables.Statuses_SVO);
        dbConnection.LoadData(Connection.Tables.Statuses_RiskGroup);
        dbConnection.LoadData(Connection.Tables.Obshaga);
        dbConnection.LoadData(Connection.Tables.Departments);

        List<Report3Data> reportData = new List<Report3Data>();

        //Собираем данные, заполняя структуру.  Используем пример с одной записью из задания
        Students student = Connection.Students.FirstOrDefault(s => s.LastName == "Ощепков");
        if (student != null)
        {
            Report3Data data = new Report3Data();
            data.FIO = $"{student.LastName} {student.FirstName} {student.MiddleName}";
            data.DataRozhdeniya = student.BirthDate.ToString("dd.MM.yyyy");
            data.Gruppa = student.Groups;
            data.KontaktnyNomer = student.ContactNumber;
            data.Finansirovanie = student.Finance;
            data.Obrazovanie = student.Obrazovanie;

            //Формируем статус "сирота"
            Statuses_Sirots sirota = Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == student.StudentID);
            if (sirota != null)
            {
                data.StatusSirota = $"Приказ № {sirota.OrderNumber} от {sirota.StartDate.ToString("dd.MM.yyyy")} - {sirota.EndDate.ToString("dd.MM.yyyy")}";
            }

            //Формируем статус "инвалид"
            Statuses_Invalid invalid = Connection.StatusesInvalids.FirstOrDefault(i => i.StudentID == student.StudentID);
            if (invalid != null)
            {
                data.StatusInvalid = $"Приказ № {invalid.OrderNumber} от {invalid.StartDate.ToString("dd.MM.yyyy")} - {invalid.EndDate.ToString("dd.MM.yyyy")}";
            }

            //Формируем статус "ОВЗ"
            Statuses_OVZ ovz = Connection.StatusesOvzs.FirstOrDefault(o => o.StudentID == student.StudentID);
            if (ovz != null)
            {
                data.StatusOVZ = $"Приказ № {ovz.Prikaz} от {ovz.StartDate.ToString("dd.MM.yyyy")} - {ovz.EndDate.ToString("dd.MM.yyyy")}";
            }

            //Документ-основание берем просто дату рождения студента, т.к. ничего другого не подходит
            data.DocumentOsnovanie1 = student.BirthDate.ToString("dd.MM.yyyy");
            data.DocumentOsnovanie2 = student.BirthDate.ToString("dd.MM.yyyy");

            //Заполняем отчисление
            data.Otchislenie = student.InfoOtchiz;

            reportData.Add(data);
        }

        return reportData;
    }

    public static void GenerateReport(string filePath)
    {
        Document document = new Document(PageSize.A4.Rotate()); // Landscape orientation
        try
        {
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Define fonts
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); // Replace with your font path
            Font headerFont = new Font(baseFont, 12, Font.BOLD);
            Font bodyFont = new Font(baseFont, 10, Font.NORMAL);

            // Add header information
            Paragraph header1 = new Paragraph("Пермский авиационный техникум им. А.Д. Швецова", bodyFont);
            header1.Alignment = Element.ALIGN_LEFT;
            document.Add(header1);

            Paragraph header2 = new Paragraph("База данных по воспитательной работе", headerFont);
            header2.Alignment = Element.ALIGN_LEFT;
            document.Add(header2);

            Paragraph header3 = new Paragraph("Сироты (Действующие)/Инвалиды (Действующие)/ОВЗ (Действующие)/Группа риска (Действующие)/СВО (Действующие)/Социальная степендия (Действующие)/на дату: 08.02.2025", bodyFont);
            header3.Alignment = Element.ALIGN_LEFT;
            document.Add(header3);


            // Create the table
            PdfPTable table = new PdfPTable(12); // 12 columns
            table.WidthPercentage = 100;

            float[] columnWidths = { 0.1f, 0.07f, 0.06f, 0.1f, 0.09f, 0.07f, 0.1f, 0.1f, 0.1f, 0.09f, 0.09f, 0.1f }; // Adjust as needed
            table.SetWidths(columnWidths);


            AddTableHeaderCell(table, "ФИО", headerFont);
            AddTableHeaderCell(table, "Дата рождения", headerFont);
            AddTableHeaderCell(table, "Группа", headerFont);
            AddTableHeaderCell(table, "Контактный номер", headerFont);
            AddTableHeaderCell(table, "Финансирование", headerFont);
            AddTableHeaderCell(table, "Образование", headerFont);
            AddTableHeaderCell(table, "Приказ о присвоении статуса \"сирота\"", headerFont);
            AddTableHeaderCell(table, "Приказ о присвоении статуса \"инвалид\"", headerFont);
            AddTableHeaderCell(table, "Приказ о присвоении статуса \"ОВЗ\"", headerFont);
            AddTableHeaderCell(table, "Документ основание", headerFont);
            AddTableHeaderCell(table, "Документ основание", headerFont);
            AddTableHeaderCell(table, "Отчисление", headerFont);

            List<Report3Data> dataList = GetData();
            foreach (var data in dataList)
            {
                AddTableCell(table, data.FIO, bodyFont);
                AddTableCell(table, data.DataRozhdeniya, bodyFont);
                AddTableCell(table, data.Gruppa, bodyFont);
                AddTableCell(table, data.KontaktnyNomer, bodyFont);
                AddTableCell(table, data.Finansirovanie, bodyFont);
                AddTableCell(table, data.Obrazovanie, bodyFont);
                AddTableCell(table, data.StatusSirota, bodyFont);
                AddTableCell(table, data.StatusInvalid, bodyFont);
                AddTableCell(table, data.StatusOVZ, bodyFont);
                AddTableCell(table, data.DocumentOsnovanie1, bodyFont);
                AddTableCell(table, data.DocumentOsnovanie2, bodyFont);
                AddTableCell(table, data.Otchislenie, bodyFont);
            }

            document.Add(table);


            // Summary Information
            Paragraph summary = new Paragraph($"Количество записей: {dataList.Count}", bodyFont);
            summary.Alignment = Element.ALIGN_LEFT;
            document.Add(summary);

            // Date printed
            Paragraph datePrinted = new Paragraph($"Дата печати: {DateTime.Now:dd.MM.yyyy}", bodyFont);
            datePrinted.Alignment = Element.ALIGN_RIGHT;
            document.Add(datePrinted);

        }
        catch (DocumentException de)
        {
            Console.Error.WriteLine(de.Message);
        }
        catch (IOException ioe)
        {
            Console.Error.WriteLine(ioe.Message);
        }
        finally
        {
            document.Close();
        }
    }

    private static void AddTableHeaderCell(PdfPTable table, string text, Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font));
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        table.AddCell(cell);
    }

    private static void AddTableCell(PdfPTable table, string text, Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font));
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        table.AddCell(cell);
    }
}