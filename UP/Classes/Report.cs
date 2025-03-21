using ClassModules;
using ClassConnection;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Windows.Media;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Windows.Input;
namespace UP.Classes
{

    public class Report
    {
        public class ReportData
        {
            public string Komnata { get; set; } // Комната
            public string FIO { get; set; } // ФИО
            public string DataRozhdeniya { get; set; } // Дата рождения
            public string Gruppa { get; set; } // Группа
            public string KontaktnyNomer { get; set; } // Контактный номер
            public string KontaktyRoditeley { get; set; } // Контакты родителей
            public string DataPervogoZaseleniya { get; set; } // Дата первого заселения
            public string ObshezhitiePrimechanie { get; set; } // Общежитие (примечание)
            public string Otchislenie { get; set; } // Отчисление
        }
        public static List<ReportData> GetData(string familiaFilter, DateTime? nachaloPerioda, DateTime? konecPerioda, string gruppaFilter)
        {
            List<ReportData> reportData = new List<ReportData>();
            List<Students> students = new List<Students>(); //Создаем список студентов

            // Начните с базового запроса
            string query = "SELECT * FROM Students WHERE 1=1";  // 1=1 для упрощения добавления условий

            // Добавьте условия WHERE в зависимости от примененных фильтров
            if (!string.IsNullOrEmpty(familiaFilter))
            {
                query += " AND LastName LIKE '%" + familiaFilter + "%'"; // Фильтр по фамилии
            }

            if (nachaloPerioda.HasValue)
            {
                query += " AND BirthDate >= '" + nachaloPerioda.Value.ToString("yyyy-MM-dd") + "'"; // Фильтр по дате начала периода
            }

            if (konecPerioda.HasValue)
            {
                query += " AND BirthDate <= '" + konecPerioda.Value.ToString("yyyy-MM-dd") + "'"; // Фильтр по дате конца периода
            }
            if (!string.IsNullOrEmpty(gruppaFilter))
            {
                query += " AND Groups LIKE '%" + gruppaFilter + "%'";
            }
            // Подключение к БД (Если еще не подключено)
            if (!Connection.IsConnected)
            {
                Connection.Connect();
            }
            // Создаем экземпляр класса Connection
            Connection dbConnection = new Connection(); //Создаем экземпляр

            // Выполнение запроса
            SqlDataReader reader = dbConnection.ExecuteQuery(query);

            if (reader != null)
            {
                while (reader.Read())
                {
                    Students student = new Students
                    {
                        StudentID = Convert.ToInt32(reader.GetValue(0)),
                        LastName = Convert.ToString(reader.GetValue(1)),
                        FirstName = Convert.ToString(reader.GetValue(2)),
                        MiddleName = Convert.ToString(reader.GetValue(3)),
                        BirthDate = Convert.ToDateTime(reader.GetValue(4)),
                        Gender = Convert.ToString(reader.GetValue(5)),
                        ContactNumber = Convert.ToString(reader.GetValue(6)),
                        Obrazovanie = Convert.ToString(reader.GetValue(7)),
                        Otdelenie = Convert.ToInt32(reader.GetValue(8)),
                        Groups = Convert.ToString(reader.GetValue(9)),
                        Finance = Convert.ToString(reader.GetValue(10)),
                        YearPostup = Convert.ToInt32(reader.GetValue(11)),
                        YearOkonch = Convert.ToInt32(reader.GetValue(12)),
                        InfoOtchiz = Convert.ToString(reader.GetValue(13)),
                        DateOtchiz = Convert.ToDateTime(reader.GetValue(14)),
                        Note = Convert.ToString(reader.GetValue(15)),
                        ParentsInfo = Convert.ToString(reader.GetValue(16)),
                        Vziskanie = Convert.ToString(reader.GetValue(17)),
                        Files = Convert.ToString(reader.GetValue(18))
                        //Files = reader.IsDBNull(6) ? null : (byte[])reader.GetValue(18)
                    };
                    students.Add(student);
                }
            }
            else
            {
                Console.WriteLine("Ошибка при выполнении запроса");
            }

            //Закрываем SqlDataReader
            reader.Close();
            //Выводим информацию в reportData
            dbConnection.LoadData(Connection.Tables.Obshaga);
            dbConnection.LoadData(Connection.Tables.Rooms);

            foreach (Students student in students)
            {
                // Find the corresponding student
                Obshaga obshaga = Connection.Obshagas.FirstOrDefault(s => s.StudentID == student.StudentID);
                Rooms room = (obshaga != null) ? Connection.Rooms.FirstOrDefault(r => r.RoomID == obshaga.RoomNumber) : null;

                reportData.Add(new ReportData
                {
                    Komnata = (room != null) ? room.RoomName : "N/A", // room number
                    FIO = $"{student.LastName}\n{student.FirstName}\n{student.MiddleName}",
                    DataRozhdeniya = student.BirthDate.ToString("dd.MM.yyyy"),
                    Gruppa = student.Groups,
                    KontaktnyNomer = student.ContactNumber,
                    KontaktyRoditeley = student.ParentsInfo,  //Assuming parent info is what you want for parent contact
                    DataPervogoZaseleniya = (obshaga != null) ? obshaga.CheckInDate.ToString("dd.MM.yyyy") : "",
                    ObshezhitiePrimechanie = (obshaga != null) ? obshaga.Note : "",
                    Otchislenie = (obshaga != null && obshaga.CheckOutDate != DateTime.MinValue) ? obshaga.CheckOutDate.ToString("dd.MM.yyyy") : ""
                });
            }

            return reportData;
        }


        public static void GenerateReport(string filePath, string familiaFilter, DateTime? nachaloPerioda, DateTime? konecPerioda, string gruppaFilter)
        {
            // 1. Create a Document object
            Document document = new Document(PageSize.A4);

            try
            {
                // 2. Create a PdfWriter object
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                // 3. Open the Document
                document.Open();

                // Define fonts
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); // Replace with your font path
                Font headerFont = new Font(baseFont, 12, Font.BOLD);
                Font bodyFont = new Font(baseFont, 10, Font.NORMAL);

                // Add header information similar to the image
                Paragraph header1 = new Paragraph("Пермский авиационный техникум им. А.Д. Швецова", bodyFont);
                header1.Alignment = Element.ALIGN_LEFT;
                document.Add(header1);

                Paragraph header2 = new Paragraph("База данных по воспитательной работе", headerFont);
                header2.Alignment = Element.ALIGN_LEFT;
                document.Add(header2);

                string filterString = $"Проживающие в общежитии";

                if (!string.IsNullOrEmpty(familiaFilter))
                {
                    filterString += $"/Фамилия: {familiaFilter}";
                }
                else
                {
                    filterString += "/Фамилия: Все";
                }
                if (nachaloPerioda.HasValue && konecPerioda.HasValue)
                {
                    filterString += $"/Период: с {nachaloPerioda.Value.ToString("dd.MM.yyyy")} по {konecPerioda.Value.ToString("dd.MM.yyyy")}";
                }

                if (!string.IsNullOrEmpty(gruppaFilter))
                {
                    filterString += $"/Группа: {gruppaFilter}";
                }

                filterString += "/По алфавиту/на дату: " + DateTime.Now.ToString("dd.MM.yyyy");

                Paragraph header3 = new Paragraph(filterString, bodyFont);
                header3.Alignment = Element.ALIGN_LEFT;
                document.Add(header3);


                // 4. Create a table
                PdfPTable table = new PdfPTable(8);  // 8 columns
                table.WidthPercentage = 100; // Table fills the page width

                // Set column widths (adjust these to match your image)
                float[] widths = new float[] { 0.06f, 0.15f, 0.08f, 0.06f, 0.13f, 0.12f, 0.11f, 0.13f }; // Example widths
                table.SetWidths(widths);



                // Add table headers (adjust font as needed)
                AddTableHeaderCell(table, "Комната", headerFont);
                AddTableHeaderCell(table, "ФИО", headerFont);
                AddTableHeaderCell(table, "Дата\nрождения", headerFont);
                AddTableHeaderCell(table, "Группа", headerFont);
                AddTableHeaderCell(table, "Контактный\nномер", headerFont);
                AddTableHeaderCell(table, "Контакты\nродителей", headerFont);
                AddTableHeaderCell(table, "Дата первого\nзаселения", headerFont);
                AddTableHeaderCell(table, "Общежитие\n(примечание)", headerFont);

                // 5. Fetch your data (replace this with your actual data source)
                List<ReportData> reportData = GetData(familiaFilter, nachaloPerioda, konecPerioda, gruppaFilter); // Передаем параметры фильтров

                // 6. Add data rows to the table
                foreach (var rowData in reportData)
                {
                    AddTableCell(table, rowData.Komnata, bodyFont);
                    AddTableCell(table, rowData.FIO, bodyFont);
                    AddTableCell(table, rowData.DataRozhdeniya, bodyFont);
                    AddTableCell(table, rowData.Gruppa, bodyFont);
                    AddTableCell(table, rowData.KontaktnyNomer, bodyFont);
                    AddTableCell(table, rowData.KontaktyRoditeley, bodyFont);
                    AddTableCell(table, rowData.DataPervogoZaseleniya, bodyFont);
                    AddTableCell(table, rowData.ObshezhitiePrimechanie, bodyFont);
                }

                document.Add(table);


                // Add summary information
                Paragraph summary = new Paragraph($"Количество записей: {reportData.Count}", bodyFont);
                summary.Alignment = Element.ALIGN_LEFT;
                document.Add(summary);

                // Add date printed info
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
                // 7. Close the Document
                document.Close();
            }
        }


        private static void AddTableHeaderCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE; // Center text vertically
            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
        }


        private static void AddTableCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER; // Default alignment, adjust if needed
            cell.VerticalAlignment = Element.ALIGN_MIDDLE; // Center text vertically
            table.AddCell(cell);
        }
    }
}