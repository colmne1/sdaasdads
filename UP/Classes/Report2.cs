using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using ClassConnection;
using ClassModules;
using System.Linq; // Убедитесь, что вы включили это!

public class Report2
{
    public class StudentInfo
    {
        public string FIO { get; set; }
        public string Pol { get; set; }
        public string DataRozhdeniya { get; set; }
        public string Sovershennoletie { get; set; }
        public string NomerTelefona { get; set; }
        public string Obrazovanie { get; set; }
        public string Gruppa { get; set; }
        public string Otdelenie { get; set; }
        public string Finansirovanie { get; set; }
        public string GodPostupleniya { get; set; }
        public string GodOkonchaniya { get; set; }
        public string Otchislenie { get; set; }
        public string DataOtchisleniya { get; set; }
        public string Primechanie { get; set; }

        public string SirotaPrikaz { get; set; }
        public string SirotaNachalo { get; set; }
        public string SirotaKonec { get; set; }
        public string SirotaPrimechanie { get; set; }

        public string InvalidPrikaz { get; set; }
        public string InvalidVid { get; set; }
        public string InvalidNachalo { get; set; }
        public string InvalidKonec { get; set; }
        public string InvalidPrimechanie { get; set; }

        public string OVZPrikaz { get; set; }
        public string OVZNachalo { get; set; }
        public string OVZKonec { get; set; }
        public string OVZPrimechanie { get; set; }

        public string ObshagaKomnata { get; set; }
        public string ObshagaDataZaseleniya { get; set; }
        public string ObshagaDataVyiselenia { get; set; }
        public string ObshagaPrimechanie { get; set; }

        public string GruppaRiskaTip { get; set; }
        public string GruppaRiskaDataPostanovki { get; set; }
        public string GruppaRiskaOsnovaniePostanovki { get; set; }
        public string GruppaRiskaPrichinaPostanovki { get; set; }
        public string GruppaRiskaDataSnyatiya { get; set; }
        public string GruppaRiskaOsnovanieSnyatiya { get; set; }
        public string GruppaRiskaPrichinaSnyatiya { get; set; }
        public string GruppaRiskaPrimechanie { get; set; }

        public string SpppData { get; set; }
        public string SpppSotrudniki { get; set; }
        public string SpppPredstaviteli { get; set; }
        public string SpppOsnovanie { get; set; }
        public string SpppPrichina { get; set; }
        public string SpppReshenie { get; set; }
        public string SpppPrimechanie { get; set; }

        public string SvoDokument { get; set; }
        public string SvoNachalo { get; set; }
        public string SvoKonec { get; set; }

        public string SocialStependiaDokument { get; set; }
        public string SocialStependiaNachalo { get; set; }
        public string SocialStependiaKonec { get; set; }

        public string Vziskania { get; set; } // Взыскания
    }

    public static StudentInfo GetStudentInfo(int studentId)
    {
        // Создайте экземпляр класса Connection
        Connection dbConnection = new Connection(); //Создаем экземпляр

        //Загружаем все справочные таблицы
        Connection.Connect();
        dbConnection.LoadData(Connection.Tables.Students);
        dbConnection.LoadData(Connection.Tables.Obshaga);
        dbConnection.LoadData(Connection.Tables.Statuses_Sirots);
        dbConnection.LoadData(Connection.Tables.Statuses_Invalid);
        dbConnection.LoadData(Connection.Tables.Statuses_OVZ);
        dbConnection.LoadData(Connection.Tables.SPPP_Meetings);
        dbConnection.LoadData(Connection.Tables.SocialScholarships);
        dbConnection.LoadData(Connection.Tables.Statuses_RiskGroup);
        dbConnection.LoadData(Connection.Tables.Rooms);

        // Находим нужного студента по StudentID
        Students student = Connection.Students.FirstOrDefault(s => s.StudentID == studentId);
        if (student == null) return null;

        // Заполняем информацию о студенте
        StudentInfo studentInfo = new StudentInfo
        {
            FIO = $"{student.LastName} {student.FirstName} {student.MiddleName}",
            Pol = student.Gender,
            DataRozhdeniya = student.BirthDate.ToString("dd.MM.yyyy"),
            Sovershennoletie = "нет", // TODO: Рассчитать совершеннолетие
            NomerTelefona = student.ContactNumber,
            Obrazovanie = student.Obrazovanie,
            Gruppa = student.Groups,
            Otdelenie = (Connection.Departments.FirstOrDefault(d => d.DepartmentID == student.Otdelenie) != null) ? Connection.Departments.FirstOrDefault(d => d.DepartmentID == student.Otdelenie).DepartmentName : "N/A",
            Finansirovanie = student.Finance,
            GodPostupleniya = student.YearPostup.ToString(),
            GodOkonchaniya = student.YearOkonch.ToString(),
            Otchislenie = student.InfoOtchiz,
            DataOtchisleniya = student.DateOtchiz.ToString("dd.MM.yyyy"),
            Primechanie = student.Note,

            // Заполняем информацию о сиротстве
            SirotaPrikaz = (Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId).OrderNumber : "",
            SirotaNachalo = (Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId).StartDate.ToString("dd.MM.yyyy") : "",
            SirotaKonec = (Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId).EndDate.ToString("dd.MM.yyyy") : "",
            SirotaPrimechanie = (Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSirots.FirstOrDefault(s => s.StudentID == studentId).Note : "",

            // Заполняем информацию об инвалидности
            InvalidPrikaz = (Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId).OrderNumber : "",
            InvalidVid = (Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId).DisabilityType : "",
            InvalidNachalo = (Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId).StartDate.ToString("dd.MM.yyyy") : "",
            InvalidKonec = (Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId).EndDate.ToString("dd.MM.yyyy") : "",
            InvalidPrimechanie = (Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesInvalids.FirstOrDefault(s => s.StudentID == studentId).Note : "",

            // Заполняем информацию о ОВЗ
            OVZPrikaz = (Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId).Prikaz : "",
            OVZNachalo = (Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId).StartDate.ToString("dd.MM.yyyy") : "",
            OVZKonec = (Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId).EndDate.ToString("dd.MM.yyyy") : "",
            OVZPrimechanie = (Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesOvzs.FirstOrDefault(s => s.StudentID == studentId).Note : "",

            // Заполняем информацию об общежитии
            ObshagaKomnata = (Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId) != null) ? Connection.Rooms.FirstOrDefault(r => r.RoomID == Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId).RoomNumber).RoomName : "",
            ObshagaDataZaseleniya = (Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId) != null) ? Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId).CheckInDate.ToString("dd.MM.yyyy") : "",
            ObshagaDataVyiselenia = (Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId) != null) ? Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId).CheckOutDate.ToString("dd.MM.yyyy") : "",
            ObshagaPrimechanie = (Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId) != null) ? Connection.Obshagas.FirstOrDefault(o => o.StudentID == studentId).Note : "",

            // Заполняем информацию о группе риска
            GruppaRiskaTip = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).RiskGroupType : "",
            GruppaRiskaDataPostanovki = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).DateStart.ToString("dd.MM.yyyy") : "",
            GruppaRiskaOsnovaniePostanovki = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).OsnPost : "",
            GruppaRiskaPrichinaPostanovki = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).PrichinaPost : "",
            GruppaRiskaDataSnyatiya = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).DateEnd.ToString("dd.MM.yyyy") : "",
            GruppaRiskaOsnovanieSnyatiya = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).OsnSnat : "",
            GruppaRiskaPrichinaSnyatiya = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).PrichinaSnat : "",
            GruppaRiskaPrimechanie = (Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesRiskGroups.FirstOrDefault(s => s.StudentID == studentId).Note : "",

            // Заполняем информацию о СППП
            SpppData = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).Date.ToString("dd.MM.yyyy") : "",
            SpppSotrudniki = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).Sotrudniki : "",
            SpppPredstaviteli = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).Predstaviteli : "",
            SpppOsnovanie = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).OsnVizov : "",
            SpppPrichina = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).ReasonForCall : "",
            SpppReshenie = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).Reshenie : "",
            SpppPrimechanie = (Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SpppMeetings.FirstOrDefault(s => s.StudentID == studentId).Note : "",

            // Заполняем информацию о СВО
            SvoDokument = (Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId).DocumentOsnov : "",
            SvoNachalo = (Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId).StartDate.ToString("dd.MM.yyyy") : "",
            SvoKonec = (Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.StatusesSvos.FirstOrDefault(s => s.StudentID == studentId).EndDate.ToString("dd.MM.yyyy") : "",

            // Заполняем информацию о Социальной стипендии
            SocialStependiaDokument = (Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId).DocumentReason : "",
            SocialStependiaNachalo = (Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId).StartDate.ToString("dd.MM.yyyy") : "",
            SocialStependiaKonec = (Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId) != null) ? Connection.SocialScholarships.FirstOrDefault(s => s.StudentID == studentId).EndDate.ToString("dd.MM.yyyy") : "",

            // Заполняем информацию о Взысканиях
            Vziskania = student.Vziskanie
        };

        return studentInfo;
    }

    public static void GenerateReport(string filePath, int studentId)
    {
        Document document = new Document(PageSize.A4);
        try
        {
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
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

            // Get student information
            StudentInfo studentInfo = GetStudentInfo(studentId);
            if (studentInfo == null)
            {
                document.Add(new Paragraph("Студент не найден", bodyFont));
                document.Close();
                return;
            }

            // Create table for student information
            PdfPTable table = CreateStudentInfoTable(studentInfo, baseFont);
            document.Add(table);

            // Summary Information
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

    private static PdfPTable CreateStudentInfoTable(StudentInfo studentInfo, BaseFont baseFont)
    {
        Font boldFont = new Font(baseFont, 10, Font.BOLD);
        Font bodyFont = new Font(baseFont, 10, Font.NORMAL);

        PdfPTable table = new PdfPTable(2);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 0.4f, 0.6f }); // Adjust column widths

        AddTableCell(table, "ФИО", boldFont, studentInfo.FIO, bodyFont);
        AddTableCell(table, "Пол", boldFont, studentInfo.Pol, bodyFont);
        AddTableCell(table, "Дата рождения", boldFont, studentInfo.DataRozhdeniya, bodyFont);
        AddTableCell(table, "Совершеннолетие", boldFont, studentInfo.Sovershennoletie, bodyFont);
        AddTableCell(table, "Номер телефона", boldFont, studentInfo.NomerTelefona, bodyFont);
        AddTableCell(table, "Образование", boldFont, studentInfo.Obrazovanie, bodyFont);
        AddTableCell(table, "Группа", boldFont, studentInfo.Gruppa, bodyFont);
        AddTableCell(table, "Отделение", boldFont, studentInfo.Otdelenie, bodyFont);
        AddTableCell(table, "Финансирование", boldFont, studentInfo.Finansirovanie, bodyFont);
        AddTableCell(table, "Год поступления", boldFont, studentInfo.GodPostupleniya, bodyFont);
        AddTableCell(table, "Год окончания", boldFont, studentInfo.GodOkonchaniya, bodyFont);
        AddTableCell(table, "Отчисление", boldFont, studentInfo.Otchislenie, bodyFont);
        AddTableCell(table, "Дата отчисления", boldFont, studentInfo.DataOtchisleniya, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.Primechanie, bodyFont);

        AddSectionHeader(table, "Сирота", boldFont);
        AddTableCell(table, "Приказ о присвоении статуса", boldFont, studentInfo.SirotaPrikaz, bodyFont);
        AddTableCell(table, "Начало статуса", boldFont, studentInfo.SirotaNachalo, bodyFont);
        AddTableCell(table, "Конец статуса", boldFont, studentInfo.SirotaKonec, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.SirotaPrimechanie, bodyFont);

        AddSectionHeader(table, "Инвалид", boldFont);
        AddTableCell(table, "Приказ о присвоении статуса", boldFont, studentInfo.InvalidPrikaz, bodyFont);
        AddTableCell(table, "Вид инвалидности", boldFont, studentInfo.InvalidVid, bodyFont);
        AddTableCell(table, "Начало статуса", boldFont, studentInfo.InvalidNachalo, bodyFont);
        AddTableCell(table, "Конец статуса", boldFont, studentInfo.InvalidKonec, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.InvalidPrimechanie, bodyFont);

        AddSectionHeader(table, "ОВЗ", boldFont);
        AddTableCell(table, "Приказ о присвоении статуса", boldFont, studentInfo.OVZPrikaz, bodyFont);
        AddTableCell(table, "Начало статуса", boldFont, studentInfo.OVZNachalo, bodyFont);
        AddTableCell(table, "Конец статуса", boldFont, studentInfo.OVZKonec, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.OVZPrimechanie, bodyFont);

        AddSectionHeader(table, "Общежитие", boldFont);
        AddTableCell(table, "Комната", boldFont, studentInfo.ObshagaKomnata, bodyFont);
        AddTableCell(table, "Дата первого заселения", boldFont, studentInfo.ObshagaDataZaseleniya, bodyFont);
        AddTableCell(table, "Дата окончательного выселения", boldFont, studentInfo.ObshagaDataVyiselenia, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.ObshagaPrimechanie, bodyFont);

        AddSectionHeader(table, "Группа риска/СОП", boldFont);
        AddTableCell(table, "Тип", boldFont, studentInfo.GruppaRiskaTip, bodyFont);
        AddTableCell(table, "Дата постановления на учёт", boldFont, studentInfo.GruppaRiskaDataPostanovki, bodyFont);
        AddTableCell(table, "Основания постановления на учёт", boldFont, studentInfo.GruppaRiskaOsnovaniePostanovki, bodyFont);
        AddTableCell(table, "Причина постановления", boldFont, studentInfo.GruppaRiskaPrichinaPostanovki, bodyFont);
        AddTableCell(table, "Дата снятия с учёта", boldFont, studentInfo.GruppaRiskaDataSnyatiya, bodyFont);
        AddTableCell(table, "Основания снятия с учёта", boldFont, studentInfo.GruppaRiskaOsnovanieSnyatiya, bodyFont);
        AddTableCell(table, "Причина снятия с учёта", boldFont, studentInfo.GruppaRiskaPrichinaSnyatiya, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.GruppaRiskaPrimechanie, bodyFont);

        AddSectionHeader(table, "СППП", boldFont);
        AddTableCell(table, "Дата", boldFont, studentInfo.SpppData, bodyFont);
        AddTableCell(table, "Присутствовали сотрудники", boldFont, studentInfo.SpppSotrudniki, bodyFont);
        AddTableCell(table, "Присутствовали представители", boldFont, studentInfo.SpppPredstaviteli, bodyFont);
        AddTableCell(table, "Основание вызова", boldFont, studentInfo.SpppOsnovanie, bodyFont);
        AddTableCell(table, "Причина вызова", boldFont, studentInfo.SpppPrichina, bodyFont);
        AddTableCell(table, "Решение", boldFont, studentInfo.SpppReshenie, bodyFont);
        AddTableCell(table, "Примечание", boldFont, studentInfo.SpppPrimechanie, bodyFont);

        AddSectionHeader(table, "СВО", boldFont);
        AddTableCell(table, "Документ основание", boldFont, studentInfo.SvoDokument, bodyFont);
        AddTableCell(table, "Начало статуса", boldFont, studentInfo.SvoNachalo, bodyFont);
        AddTableCell(table, "Конец статуса", boldFont, studentInfo.SvoKonec, bodyFont);

        AddSectionHeader(table, "Социальная степендия", boldFont);
        AddTableCell(table, "Документ основание", boldFont, studentInfo.SocialStependiaDokument, bodyFont);
        AddTableCell(table, "Начало выплаты", boldFont, studentInfo.SocialStependiaNachalo, bodyFont);
        AddTableCell(table, "Конец выплаты", boldFont, studentInfo.SocialStependiaKonec, bodyFont);

        AddSectionHeader(table, "Взыскания", boldFont);
        AddTableCell(table, "Взыскания", boldFont, studentInfo.Vziskania, bodyFont);

        return table;
    }

    private static void AddTableCell(PdfPTable table, string label, Font labelFont, string value, Font valueFont)
    {
        PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
        labelCell.Border = PdfPCell.NO_BORDER; // Убираем рамки
        table.AddCell(labelCell);

        PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
        valueCell.Border = PdfPCell.NO_BORDER;  // Убираем рамки
        table.AddCell(valueCell);
    }

    private static void AddSectionHeader(PdfPTable table, string headerText, Font headerFont)
    {
        PdfPCell headerCell = new PdfPCell(new Phrase(headerText, headerFont));
        headerCell.Colspan = 2;
        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
        headerCell.Border = PdfPCell.NO_BORDER; // Убираем рамки
        headerCell.PaddingTop = 10;  // Добавляем отступ сверху
        table.AddCell(headerCell);
    }
}