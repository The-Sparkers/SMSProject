using SMSProject.Models;
using SMSProject.Models.ModelEnums;
using SMSProject.ServiceModules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.ViewModels.AdminViewModels
{
    public class DashboardViewModel
    {
        string conString;
        public DashboardViewModel(string conString)
        {
            this.conString = conString;
        }
        public int ParentCount
        {
            get
            {
                return Parent.GetAllParents(conString).Count;
            }
        }
        public int StudentCount
        {
            get
            {
                return Student.GetAllStudents(conString).Count;
            }
        }
        public int StaffCount
        {
            get
            {
                return Staff.GetAllStaff(conString).Count;
            }
        }
        public int StudentPresent
        {
            get
            {
                int count = 0;
                try
                {
                    SqlConnection con = new SqlConnection(conString);
                    string query = "SELECT COUNT(a.AttandanceId) FROM ATTANDANCES a, STUDENT_GIVES_DAILY_ATTANDANCE st WHERE a.AttandanceId=st.AttandanceId AND a.IsAbsent=0 AND a.Date='" + DateTime.Now + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException)
                {

                }
                catch (Exception)
                {

                }
                return count;
            }
        }
        public int StaffPresent
        {
            get
            {
                int count = 0;
                try
                {
                    SqlConnection con = new SqlConnection(conString);
                    string query = "SELECT COUNT(a.AttandanceId) FROM ATTANDANCES a, STAFF_GIVES_DAILY_ATTANDANCE st WHERE a.AttandanceId=st.AttandanceId AND a.IsAbsent=0 AND a.Date='" + DateTime.Now + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException)
                {

                }
                catch (Exception)
                {

                }
                return count;
            }
        }
        public decimal CollectedDues
        {
            get
            {
                return decimal.Round(ParentFeeRecord.GetTotalCollection(DateTime.Now, conString));

            }
        }
        public decimal RemainingDues
        {
            get
            {
                return decimal.Round(decimal.Subtract(ParentFeeRecord.GetTotalDue(DateTime.Now, conString), CollectedDues));
            }
        }
        public int UnpaidParentCount
        {
            get
            {
                return Parent.GetParentsWithUnpaidDues(conString, DateTime.Now).Count;
            }
        }
        public decimal TotalToday
        {
            get
            {
                decimal total = 0;
                try
                {
                    SqlConnection con = new SqlConnection(conString);
                    string query = "SELECT SUM(AmountPaid) FROM PARENT_MONTHLY_FEE WHERE [DatePaid]='" + DateTime.Now + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        total = (decimal)cmd.ExecuteScalar();
                    }
                    catch (InvalidCastException)
                    {

                    }
                    con.Close();
                }
                catch (SqlException)
                {

                }
                return decimal.Round(total);
            }
        }
        public List<MonthlyCollection> MonthlyCollections
        {
            get
            {
                List<MonthlyCollection> lst = new List<MonthlyCollection>();
                for (int i = 1; i <= 12; i++)
                {
                    lst.Add(new MonthlyCollection()
                    {
                        Collection = ParentFeeRecord.GetTotalCollection(new DateTime(DateTime.Now.Year, i, 1), conString),
                        MonthName = new DateTime(2018, i, 1).ToString("MMMM")
                    });
                }
                return lst;
            }
        }
        public SoFar SoFar
        {
            get
            {
                SoFar s = new SoFar();
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                try
                {
                    SqlConnection con = new SqlConnection(conString);
                    string query = "MonthSoFar";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@dateStart", System.Data.SqlDbType.Date)).Value = startDate;
                    cmd.Parameters.Add(new SqlParameter("@dateEnd", System.Data.SqlDbType.Date)).Value = endDate;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        s.StudentAdmit = (int)reader[0];
                        s.TestTaken = (int)reader[1];
                        s.ItemsSold = (int)reader[2];
                        s.NotificationsSent = (int)reader[3];
                        s.ApplicationsReceived = (int)reader[4];
                        s.PaymentsReceived = (int)reader[5];
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    s.StudentAdmit = 0;
                    s.TestTaken = 0;
                    s.ItemsSold = 0;
                    s.NotificationsSent = 0;
                    s.ApplicationsReceived = 0;
                    s.PaymentsReceived = 0;
                }
                return s;
            }
        }
        public List<RecentTest> RecentTests
        {
            get
            {
                List<RecentTest> lst = new List<RecentTest>();
                try
                {
                    SqlConnection con = new SqlConnection(conString);
                    string query = "SELECT DISTINCT (result.TestId),  result.result, result.StudentId, result.SubjectId FROM( SELECT(SUM(st.ObtainedMarks) / SUM(t.TotalMarks)) * 100 as result, st.TestId, MAX(st.StudentId) as StudentId, t.SubjectId as SubjectId FROM STUDENT_GET_TEST_RESULTS st, TESTS t WHERE st.TestId = t.TestId GROUP BY st.TestId, t.SubjectId) result , ( SELECT TOP 9 TestId FROM TESTS ORDER BY HeldDate DESC) topr WHERE topr.TestId = result.TestId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Test t = new Test((long)reader[0], con.ConnectionString);
                        Student s = new Student((int)reader[2], con.ConnectionString);
                        Subject sub = new Subject((int)reader[3], con.ConnectionString);
                        try
                        {
                            lst.Add(new RecentTest()
                            {
                                Class = sub.Class.Name,
                                Subject = sub.Name,
                                HeldDate = t.HeldDate.ToShortDateString(),
                                Result = (int)decimal.Round((decimal)reader[1]),
                                Section = s.Section.Name
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                    con.Close();
                }
                catch (SqlException)
                {

                }
                return lst;
            }
        }
    }
    public struct MonthlyCollection
    {
        public string MonthName { get; set; }
        public decimal Collection { get; set; }
    }
    public struct SoFar
    {
        public int StudentAdmit { get; set; }
        public int TestTaken { get; set; }
        public int ItemsSold { get; set; }
        public int NotificationsSent { get; set; }
        public int ApplicationsReceived { get; set; }
        public int PaymentsReceived { get; set; }
    }
    public struct RecentTest
    {

        public string Class { get; internal set; }
        public string Subject { get; internal set; }
        public string HeldDate { get; internal set; }
        public int Result { get; internal set; }
        public string Section { get; internal set; }
    }
    public class AddStudent1ViewModel
    {
        [Display(Name = "Father's Name")]
        public string SearchName { get; set; }
        [Display(Name = "Father's CNIC")]
        [StringLength(15,MinimumLength =15)]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The CNIC number should be in the given format")]
        public string SearchCNIC { get; set; }
        public List<AddStudent1SearchResultViewModel> SearchResult { get; set; }
    }
    public struct AddStudent1SearchResultViewModel
    {
        public int ParentId { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherCNIC { get; set; }
    }
    public class AddStudent2ViewModel
    {
        [Display(Name = "Full Name*")]
        [RegularExpression(@"^([A-Z][a-z]+)(\s[A-Z][a-z]+)*$", ErrorMessage = "Please Write Name in the given format")]
        [Required]
        public string Name { get; set; }
        [Display(Name ="B-Form Number")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The B-Form number should be in the given format")]
        [StringLength(15, MinimumLength = 15)]
        public string BForm { get; set; }
        [Required]
        [Display(Name ="Gender*")]
        public Genders Gender { get; set; }
        [Required]
        [Display(Name ="Admission Number*")]
        [RegularExpression(@"^(([A-Z]|[0-9])+(([-]*)([A-Z]|[0-9])+)*)*$", ErrorMessage ="Only Uppercase letters, Numbers are acceptable. Use '-' in between the letters")]
        [StringLength(50)]
        public string AddmissionNumber { get; set; }
        [Required]
        [Display(Name ="Class*")]
        public int Class { get; set; }
        [Required]
        [Display(Name ="Date of Birth*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        [Required]
        [Display(Name ="Monthly Fee*")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]+([\,\.][0-9]+)?$", ErrorMessage ="Please Enter a Valid Fee")]
        public decimal MonthlyFee { get; set; }
        [Display(Name ="Previous Institute")]
        public string Prevnst { get; set; }
        public int ParentId { get; set; }
        
    }
    public class AddStudent3ViewModel
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string BForm { get; set; }
        public int Class { get; set; }
        public Genders Gender { get; set; }
        public string AddmissionNumber { get; set; }
        public DateTime DOB { get; set; }
        public decimal MonthlyFee { get; set; }
        public string Prevnst { get; set; }
        [Required]
        [Display(Name ="Section*")]
        public int Section { get; set; }
    }
    public class StudentDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Student Name")]
        public string Name { get; set; }
        [Display(Name = "B-Form Number")]
        public string BForm { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Date Of Birth")]
        public string DOB { get; set; }
        [Display(Name = "Father Name")]
        public string FName { get; set; }
        [Display(Name = "Class")]
        public string Class { get; set; }
        [Display(Name = "Section")]
        public string Section { get; set; }
        [Display(Name = "Class Roll Number")]
        public string RollNumber { get; set; }
        [Display(Name = "Monthly Fee")]
        public string Fee { get; set; }
        [Display(Name = "Admission Number")]
        public string AdmissionNumber { get; set; }
        [Display(Name = "Date of Addmission")]
        public string DOA { get; set; }
        [Display(Name = "Previous Institute")]
        public string PrevInst { get; set; }

    }
}