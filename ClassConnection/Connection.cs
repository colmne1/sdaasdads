using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassModules;

namespace ClassConnection
{
    public class Connection
    {
        public static bool IsConnected = false;
        public static string ConnectionString;
        public enum Tables
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
            users
        }
        public static List<Rooms> Rooms = new List<Rooms>();
        public static List<SocialScholarships> SocialScholarships = new List<SocialScholarships>();
        public static List<Statuses_RiskGroup> StatusesRiskGroups = new List<Statuses_RiskGroup>();
        public static List<Statuses_Invalid> StatusesInvalids = new List<Statuses_Invalid>();
        public static List<SPPP_Meetings> SpppMeetings = new List<SPPP_Meetings>();
        public static List<Students> Students = new List<Students>();
        public static List<Departments> Departments = new List<Departments>();
        public static List<Obshaga> Obshagas = new List<Obshaga>();
        public static List<Statuses_OVZ> StatusesOvzs = new List<Statuses_OVZ>();
        public static List<Statuses_SVO> StatusesSvos = new List<Statuses_SVO>();
        public static List<Statuses_Sirots> StatusesSirots = new List<Statuses_Sirots>();
        public static List<Users> users = new List<Users>();

        public static void Connect()
        {
            try
            {
                string Path = $@"Server=student.permaviat.ru;Database=base1_ISP_21_4_2;User=ISP_21_4_2;Password=Rd4jS7u7I#";
                SqlConnection connection = new SqlConnection(Path);
                connection.Open();
                IsConnected = true;
                ConnectionString = Path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsConnected = false;
            }
        }

        public SqlDataReader ExecuteQuery(string query)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                return reader;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public void LoadData(Tables table)
        {
            try
            {
                switch (table)
                {
                    case Tables.Rooms:
                        SqlDataReader reader = ExecuteQuery("SELECT * FROM Rooms ORDER BY RoomID");
                        Rooms.Clear();
                        while (reader.Read())
                        {
                            Rooms room = new Rooms
                            {
                                RoomID = Convert.ToInt32(reader.GetValue(0)),
                                RoomName = Convert.ToString(reader.GetValue(1)),
                                Vmestim = Convert.ToString(reader.GetValue(2))
                            };
                            Rooms.Add(room);
                        }
                        reader.Close();
                        break;

                    case Tables.SocialScholarships:
                        reader = ExecuteQuery("SELECT * FROM SocialScholarships ORDER BY ScholarshipID");
                        SocialScholarships.Clear();
                        while (reader.Read())
                        {
                            SocialScholarships scholarship = new SocialScholarships
                            {
                                ScholarshipID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                DocumentReason = Convert.ToString(reader.GetValue(2)),
                                StartDate = Convert.ToDateTime(reader.GetValue(3)),
                                EndDate = Convert.ToDateTime(reader.GetValue(4)),
                                Files = Convert.ToString(reader.GetValue(5))
                            };
                            SocialScholarships.Add(scholarship);
                        }
                        reader.Close();
                        break;

                    case Tables.Statuses_RiskGroup:
                        reader = ExecuteQuery("SELECT * FROM Statuses_RiskGroup ORDER BY RiskGroupID");
                        StatusesRiskGroups.Clear();
                        while (reader.Read())
                        {
                            Statuses_RiskGroup riskGroup = new Statuses_RiskGroup
                            {
                                RiskGroupID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                RiskGroupType = Convert.ToString(reader.GetValue(2)),
                                DateStart = Convert.ToDateTime(reader.GetValue(3)),
                                DateEnd = Convert.ToDateTime(reader.GetValue(4)),
                                OsnPost = Convert.ToString(reader.GetValue(5)),
                                OsnSnat = Convert.ToString(reader.GetValue(6)),
                                PrichinaPost = Convert.ToString(reader.GetValue(7)),
                                PrichinaSnat = Convert.ToString(reader.GetValue(8)),
                                Note = Convert.ToString(reader.GetValue(9)),
                                Files = Convert.ToString(reader.GetValue(10))
                            };
                            StatusesRiskGroups.Add(riskGroup);
                        }
                        reader.Close();
                        break;

                    case Tables.Statuses_Invalid:
                        reader = ExecuteQuery("SELECT * FROM Statuses_Invalid ORDER BY DisabilityStatusID");
                        StatusesInvalids.Clear();
                        while (reader.Read())
                        {
                            Statuses_Invalid invalid = new Statuses_Invalid
                            {
                                DisabilityStatusID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                OrderNumber = Convert.ToString(reader.GetValue(2)),
                                StartDate = Convert.ToDateTime(reader.GetValue(3)),
                                EndDate = Convert.ToDateTime(reader.GetValue(4)),
                                DisabilityType = Convert.ToString(reader.GetValue(5)),
                                Note = Convert.ToString(reader.GetValue(6)),
                                Files = Convert.ToString(reader.GetValue(7))
                            };
                            StatusesInvalids.Add(invalid);
                        }
                        reader.Close();
                        break;

                    case Tables.SPPP_Meetings:
                        reader = ExecuteQuery("SELECT * FROM SPPP_Meetings ORDER BY MeetingID");
                        SpppMeetings.Clear();
                        while (reader.Read())
                        {
                            SPPP_Meetings meeting = new SPPP_Meetings
                            {
                                MeetingID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                Date = Convert.ToDateTime(reader.GetValue(2)),
                                OsnVizov = Convert.ToString(reader.GetValue(3)),
                                Sotrudniki = Convert.ToString(reader.GetValue(4)),
                                Predstaviteli = Convert.ToString(reader.GetValue(5)),
                                ReasonForCall = Convert.ToString(reader.GetValue(6)),
                                Reshenie = Convert.ToString(reader.GetValue(7)),
                                Note = Convert.ToString(reader.GetValue(8)),
                                Files = Convert.ToString(reader.GetValue(9))
                            };
                            SpppMeetings.Add(meeting);
                        }
                        reader.Close();
                        break;

                    case Tables.Students:
                        reader = ExecuteQuery("SELECT * FROM Students ORDER BY StudentID");
                        Students.Clear();
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
                            Students.Add(student);
                        }
                        reader.Close();
                        break;

                    case Tables.Departments:
                        reader = ExecuteQuery("SELECT * FROM Departments ORDER BY DepartmentID");
                        Departments.Clear();
                        while (reader.Read())
                        {
                            Departments department = new Departments
                            {
                                DepartmentID = Convert.ToInt32(reader.GetValue(0)),
                                DepartmentName = Convert.ToString(reader.GetValue(1))
                            };
                            Departments.Add(department);
                        }
                        reader.Close();
                        break;

                    case Tables.Obshaga:
                        reader = ExecuteQuery("SELECT * FROM Obshaga ORDER BY DormitoryID");
                        Obshagas.Clear();
                        while (reader.Read())
                        {
                            Obshaga obshaga = new Obshaga
                            {
                                DormitoryID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                RoomNumber = Convert.ToInt32(reader.GetValue(2)),
                                CheckInDate = Convert.ToDateTime(reader.GetValue(3)),
                                CheckOutDate = Convert.ToDateTime(reader.GetValue(4)),
                                Note = Convert.ToString(reader.GetValue(5)),
                                Files = Convert.ToString(reader.GetValue(6))
                            };
                            Obshagas.Add(obshaga);
                        }
                        reader.Close();
                        break;

                    case Tables.Statuses_OVZ:
                        reader = ExecuteQuery("SELECT * FROM Statuses_OVZ ORDER BY OVZStatusID");
                        StatusesOvzs.Clear();
                        while (reader.Read())
                        {
                            Statuses_OVZ ovz = new Statuses_OVZ
                            {
                                OVZStatusID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                Prikaz = Convert.ToString(reader.GetValue(2)),
                                StartDate = Convert.ToDateTime(reader.GetValue(3)),
                                EndDate = Convert.ToDateTime(reader.GetValue(4)),
                                Note = Convert.ToString(reader.GetValue(5)),
                                Files = Convert.ToString(reader.GetValue(6))
                            };
                            StatusesOvzs.Add(ovz);
                        }
                        reader.Close();
                        break;

                    case Tables.Statuses_SVO:
                        reader = ExecuteQuery("SELECT * FROM Statuses_SVO ORDER BY SVOStatusID");
                        StatusesSvos.Clear();
                        while (reader.Read())
                        {
                            Statuses_SVO svo = new Statuses_SVO
                            {
                                SVOStatusID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                DocumentOsnov = Convert.ToString(reader.GetValue(2)),
                                StartDate = Convert.ToDateTime(reader.GetValue(3)),
                                EndDate = Convert.ToDateTime(reader.GetValue(4)),
                                Files = Convert.ToString(reader.GetValue(5))
                            };
                            StatusesSvos.Add(svo);
                        }
                        reader.Close();
                        break;

                    case Tables.Statuses_Sirots:
                        reader = ExecuteQuery("SELECT * FROM Statuses_Sirots ORDER BY OrphanStatusID");
                        StatusesSirots.Clear();
                        while (reader.Read())
                        {
                            Statuses_Sirots sirot = new Statuses_Sirots
                            {
                                OrphanStatusID = Convert.ToInt32(reader.GetValue(0)),
                                StudentID = Convert.ToInt32(reader.GetValue(1)),
                                OrderNumber = Convert.ToString(reader.GetValue(2)),
                                StartDate = Convert.ToDateTime(reader.GetValue(3)),
                                EndDate = Convert.ToDateTime(reader.GetValue(4)),
                                Note = Convert.ToString(reader.GetValue(5)),
                                Files = Convert.ToString(reader.GetValue(6))
                            };
                            StatusesSirots.Add(sirot);
                        }
                        reader.Close();
                        break;
                    case Tables.users:
                        reader = ExecuteQuery("SELECT * FROM users ORDER BY UsersID");
                        StatusesSirots.Clear();
                        while (reader.Read())
                        {
                            Users user = new Users
                            {
                                UsersID = Convert.ToInt32(reader.GetValue(0)),
                                Login = Convert.ToString(reader.GetValue(1)),
                                Password = Convert.ToString(reader.GetValue(2)),
                                Role = Convert.ToString(reader.GetValue(3))
                            };
                            users.Add(user);
                        }
                        reader.Close();
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public int SetLastId(Tables table)
        {
            try
            {
                LoadData(table);

                switch (table)
                {
                    case Tables.Rooms:
                        if (Rooms.Count >= 1)
                        {
                            return Rooms.Max(r => r.RoomID) + 1;
                        }
                        else return 1;

                    case Tables.SocialScholarships:
                        if (SocialScholarships.Count >= 1)
                        {
                            return SocialScholarships.Max(s => s.ScholarshipID) + 1;
                        }
                        else return 1;

                    case Tables.Statuses_RiskGroup:
                        if (StatusesRiskGroups.Count >= 1)
                        {
                            return StatusesRiskGroups.Max(s => s.RiskGroupID) + 1;
                        }
                        else return 1;

                    case Tables.Statuses_Invalid:
                        if (StatusesInvalids.Count >= 1)
                        {
                            return StatusesInvalids.Max(s => s.DisabilityStatusID) + 1;
                        }
                        else return 1;

                    case Tables.SPPP_Meetings:
                        if (SpppMeetings.Count >= 1)
                        {
                            return SpppMeetings.Max(s => s.MeetingID) + 1;
                        }
                        else return 1;

                    case Tables.Students:
                        if (Students.Count >= 1)
                        {
                            return Students.Max(s => s.StudentID) + 1;
                        }
                        else return 1;

                    case Tables.Departments:
                        if (Departments.Count >= 1)
                        {
                            return Departments.Max(d => d.DepartmentID) + 1;
                        }
                        else return 1;

                    case Tables.Obshaga:
                        if (Obshagas.Count >= 1)
                        {
                            return Obshagas.Max(o => o.DormitoryID) + 1;
                        }
                        else return 1;

                    case Tables.Statuses_OVZ:
                        if (StatusesOvzs.Count >= 1)
                        {
                            return StatusesOvzs.Max(s => s.OVZStatusID) + 1;
                        }
                        else return 1;

                    case Tables.Statuses_SVO:
                        if (StatusesSvos.Count >= 1)
                        {
                            return StatusesSvos.Max(s => s.SVOStatusID) + 1;
                        }
                        else return 1;

                    case Tables.Statuses_Sirots:
                        if (StatusesSirots.Count >= 1)
                        {
                            return StatusesSirots.Max(s => s.OrphanStatusID) + 1;
                        }
                        else return 1;
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }

        }

    }
}