using PagedList;
using SMSProject.Common.DataValidators;
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
        [StringLength(15, MinimumLength = 15)]
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
        [Display(Name = "B-Form Number")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The B-Form number should be in the given format")]
        [StringLength(15, MinimumLength = 15)]
        public string BForm { get; set; }
        [Required]
        [Display(Name = "Gender*")]
        public Genders Gender { get; set; }
        [Required]
        [Display(Name = "Admission Number*")]
        [RegularExpression(@"^(([A-Z]|[0-9])+(([-]*)([A-Z]|[0-9])+)*)*$", ErrorMessage = "Only Uppercase letters, Numbers are acceptable. Use '-' in between the letters")]
        [StringLength(50)]
        public string AddmissionNumber { get; set; }
        [Required]
        [Display(Name = "Class*")]
        public int Class { get; set; }
        [Required]
        [Display(Name = "Date of Birth*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CurrentDateRange(ErrorMessage = "Please enter a valid date")]
        public DateTime DOB { get; set; }
        [Required]
        [Display(Name = "Monthly Fee*")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]+([\,\.][0-9]+)?$", ErrorMessage = "Please Enter a Valid Fee")]
        public decimal MonthlyFee { get; set; }
        [Display(Name = "Previous Institute")]
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
        [Display(Name = "Section*")]
        public int Section { get; set; }
    }
    public class StudentDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "B-Form Number")]
        public string BForm { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Age")]
        public string Age { get; set; }
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
    public class StruckOffStudentViewModel
    {
        [Display(Name = "Full Name*")]
        [Required]
        public string SearchName { get; set; }
        public List<StruckOffStudentSearchResult> Result { get; set; }
    }
    public struct StruckOffStudentSearchResult
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string AdmissionNumber { get; set; }
        public string Class { get; set; }
    }
    public class ViewStruckOffStudentViewModel
    {
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Display(Name = "B-Form #")]
        public string BFormNumber { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Last Class")]
        public string LastClass { get; set; }
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Display(Name = "Board Roll #")]
        public string BoardExam { get; set; }
        [Display(Name = "Date of Struck")]
        public string DOS { get; set; }
    }
    public class SearchStruckOffViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please type something to search")]
        public string SearchName { get; set; }
    }
    public class AddParentViewModel
    {
        [Required]
        [Display(Name = "Father Name", Description = "Enter the name in format: Muhammad Ali Ahmad")]
        [RegularExpression(@"^([A-Z][a-z]+)(\s[A-Z][a-z]+)*$", ErrorMessage = "Please Write Name in the given format")]
        public string FatherName { get; set; }
        [Required]
        [Display(Name = "Father CNIC", Description = "Enter the name in format: XXXXX-XXXXXXX_X")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The CNIC number should be in the given format")]
        public string FCNIC { get; set; }
        [Required]
        [Display(Name = "Country Code")]
        [RegularExpression("^[+][0-9]{2}$", ErrorMessage = "Coutry Code not correct.")]
        [StringLength(3, MinimumLength = 3)]
        public string FCountryCode { get; set; }
        [Required]
        [Display(Name = "Company Code")]
        [RegularExpression("^[3]([0-4][0-9])|[5][5]$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(3, MinimumLength = 3)]
        public string FCompanyCode { get; set; }
        [Required]
        [Display(Name = "Father Mobile Number")]
        [RegularExpression("^[0-9]{7}$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(7, MinimumLength = 7)]
        public string FNumber { get; set; }
        [Required]
        [Display(Name = "Mother Name", Description = "Enter the name in format: Alia Aslam")]
        [RegularExpression(@"^([A-Z][a-z]+)(\s[A-Z][a-z]+)*$", ErrorMessage = "Please Write Name in the given format")]
        public string MotherName { get; set; }
        [Required]
        [Display(Name = "Country Code")]
        [RegularExpression("^[+][0-9]{2}$", ErrorMessage = "Coutry Code not correct.")]
        [StringLength(3, MinimumLength = 3)]
        public string MCountryCode { get; set; }
        [Required]
        [Display(Name = "Company Code")]
        [RegularExpression("^[3]([0-4][0-9])|[5][5]$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(3, MinimumLength = 3)]
        public string MCompanyCode { get; set; }
        [Required]
        [Display(Name = "Mother Mobile Number")]
        [RegularExpression("^[0-9]{7}$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(7, MinimumLength = 7)]
        public string MNumber { get; set; }
        [Required]
        [Display(Name = "Home Address")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Emergency Contact")]
        [StringLength(15, MinimumLength = 7)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please Enter a correct Phone Number")]
        public string EmergencyContact { get; set; }
        [Required]
        [Display(Name = "Eligibility Threshold (%)")]
        [Range(0, 100, ErrorMessage = "Percentage (%) can't be greater than 100 or less than 0")]
        public int ElgibilityThreshold { get; set; }
    }
    public class ViewParentDetailsViewModel
    {
        public int ParentId { set; get; }
        [Display(Name = "Father Name", Description = "Enter the name in format: Muhammad Ali Ahmad")]
        public string FatherName { get; set; }
        [Display(Name = "Father CNIC", Description = "Enter the name in format: XXXXX-XXXXXXX_X")]
        public string FCNIC { get; set; }
        [Display(Name = "Father Mobile Number")]
        public string FNumber { get; set; }
        [Display(Name = "Mother Name", Description = "Enter the name in format: Alia Aslam")]
        public string MotherName { get; set; }
        [Display(Name = "Mother Mobile Number")]
        public string MNumber { get; set; }
        [Display(Name = "Home Address")]
        public string Address { get; set; }
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }
        [Display(Name = "Eligibility Threshold (%)")]
        public int ElgibilityThreshold { get; set; }
        public List<ParentStudent> StudentsList { get; set; }
    }
    public struct ParentStudent
    {
        public int StudentId { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Class")]
        public string Class { get; set; }
    }
    public class ViewParentsViewModel
    {
        public int ParentId { get; set; }
        [Display(Name = "Father Name")]
        public string Fname { get; set; }
        [Display(Name = "Father CNIC")]
        public string FCNIC { get; set; }
        [Display(Name = "Mother Name")]
        public string MName { get; set; }
        [Display(Name = "Balance")]
        public string Balance { get; set; }
    }
    public class _SearchParentByCNICPartialViewModel
    {
        [Required]
        [Display(Name = "CNIC", Description = "Enter the name in format: XXXXX-XXXXXXX-X")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The CNIC number should be in the given format")]
        public string searchCNIC { get; set; }
    }
    public class ViewUnPaidParentsViewModel
    {
        [Display(Name = "Father Name")]
        public string FName { get; set; }
        [Display(Name = "Amount Due")]
        public string Amount { get; set; }
        public int ParentId { get; set; }
    }
    public enum StaffTypes
    {
        Teacher = 1,
        NonTeaching = 2
    }
    public class AddStaffViewModel
    {
        [Required]
        [Display(Name = "Full Name", Description = "Enter the name in format: Muhammad Ali Ahmad")]
        [RegularExpression(@"^([A-Z][a-z]+)(\s[A-Z][a-z]+)*$", ErrorMessage = "Please Write Name in the given format")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "CNIC Number", Description = "Enter the name in format: XXXXX-XXXXXXX_X")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "The CNIC number should be in the given format")]
        public string CNIC { get; set; }
        [Required]
        [Display(Name = "Home Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Country Code")]
        [RegularExpression("^[+][0-9]{2}$", ErrorMessage = "Coutry Code not correct.")]
        [StringLength(3, MinimumLength = 3)]
        public string MCountryCode { get; set; }
        [Required]
        [Display(Name = "Company Code")]
        [RegularExpression("^[3]([0-4][0-9])|[5][5]$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(3, MinimumLength = 3)]
        public string MCompanyCode { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^[0-9]{7}$", ErrorMessage = "Please Enter a correct Phone Number.")]
        [StringLength(7, MinimumLength = 7)]
        public string MNumber { get; set; }
        [Required]
        [Display(Name = "Salary (Rs.)")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]+([\,\.][0-9]+)?$", ErrorMessage = "Please Enter a Valid Fee")]
        public decimal Salary { get; set; }
        [Required]
        [Display(Name = "Gender")]
        [EnumDataType(typeof(Genders))]
        public Genders Gender { get; set; }
        [Display(Name = "Job Type")]
        [StringLength(50)]
        [RegularExpression(@"^([A-Z]+([-]|[_]){0,1}[A-Z]*)+$", ErrorMessage = "Use only UPPERCASE Letters with ('-' or '_') in between.")]
        public string JobType { get; set; }
        [Required]
        [Display(Name = "Staff Type")]
        public StaffTypes StaffType { get; set; }
    }
    public class ViewTeacherDetailsViewModel
    {
        [Display(Name = "Full Name", Description = "Enter the name in format: Muhammad Ali Ahmad")]
        public string Name { get; set; }
        [Display(Name = "CNIC Number", Description = "Enter the name in format: XXXXX-XXXXXXX_X")]
        public string CNIC { get; set; }
        [Display(Name = "Home Address")]
        public string Address { get; set; }
        [Display(Name = "Mobile Number")]
        public string MNumber { get; set; }
        [Display(Name = "Salary (Rs.)")]
        public decimal Salary { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Joining Date")]
        public string JoiningDate { get; set; }
        public int Id { get; set; }
        public List<TeacherQualification> Qualifications { get; set; }
        public List<TeacherSection> Sections { get; set; }

    }
    public class TeacherQualification
    {
        [Display(Name = "Degree")]
        public string Degree { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        public int Id { get; set; }
        public int TeacherId { get; set; }
    }
    public class TeacherSection
    {
        [Display(Name = "Class")]
        public string Class { get; set; }
        [Display(Name = "Section")]
        public string Section { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        public int SubjectId { get; set; }
        public int SectionId { get; set; }
        public int TeacherId { get; set; }
    }
    public class ViewNonStaffDetailsViewModel
    {
        [Display(Name = "Full Name", Description = "Enter the name in format: Muhammad Ali Ahmad")]
        public string Name { get; set; }
        [Display(Name = "CNIC Number", Description = "Enter the name in format: XXXXX-XXXXXXX_X")]
        public string CNIC { get; set; }
        [Display(Name = "Home Address")]
        public string Address { get; set; }
        [Display(Name = "Mobile Number")]
        public string MNumber { get; set; }
        [Display(Name = "Salary (Rs.)")]
        public decimal Salary { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Job Type")]
        public string JobType { get; set; }
        [Display(Name = "Joining Date")]
        public string JoiningDate { get; set; }
        public int Id { get; set; }
    }
    public class AddQualificationViewModel
    {
        [Required]
        [Display(Name = "Degree")]
        public string Degree { get; set; }
        [Required]
        [Display(Name = "Year")]
        [RegularExpression("^([1][9][0-9]{2})|([2][0][0-9]{2})$", ErrorMessage = "Please enter a correct year.")]
        public string Year { get; set; }
    }
    public class AddTeacherSectionViewModel
    {
        [Required]
        [Display(Name = "Class")]
        public int Class { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public int Subject { get; set; }
        [Required]
        [Display(Name = "Section")]
        public int Section { get; set; }
    }
    public class ViewStaffViewModel
    {
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "CNIC Number")]
        public string CNIC { get; set; }
        [Display(Name = "Mobile Number")]
        public string PNumber { get; set; }
        public int Id { get; set; }
    }
    public class LoadSetSalariesViewModel
    {
        [Required]
        [Display(Name = "Month")]
        public MonthNames Month { get; set; }
        [Required]
        [Display(Name = "Year")]
        public short Year { get; set; }
    }
    public class SetSalariesViewModel
    {
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "CNIC Number")]
        public string CNIC { get; set; }
        [Display(Name = "Date of Joining")]
        public string JoiningDate { get; set; }
        public int Id { get; set; }
    }
    public class ViewSalariesViewModel
    {
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "CNIC Number")]
        public string CNIC { get; set; }
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }
        [Display(Name = "Absents")]
        public int Absents { get; set; }
        [Display(Name = "Per Absent Deduction")]
        public decimal PerAbsent { get; set; }
        [Display(Name = "Total Salary")]
        public decimal TSalary { get; set; }
        public int Id { get; set; }
    }
    public class LoadViewSalariesViewModel
    {
        [Required]
        [Display(Name = "Month")]
        public MonthNames Month { get; set; }
        [Required]
        [Display(Name = "Year")]
        public short Year { get; set; }
    }
    public class _MonthlySalariesChartPartialViewModel
    {
        public string Month { get; set; }
        public string TotalSalaries { get; set; }
    }
    public class AddClassViewModel
    {
        [Required]
        [Display(Name = "Class Name", Description = "Define the name of the class")]
        [StringLength(15, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please Enter a correct number")]
        [Display(Name = "Roll# Index", Description = "The roll number index is the number from which a class roll no for students starts.")]
        public int RollNo { get; set; }
        [Required]
        [Display(Name = "Class Incharge")]
        public int TeacherId { get; set; }
    }
    public class ViewClassDetailsViewModel
    {
        [Display(Name = "Class Name", Description = "Define the name of the class")]
        public string Name { get; set; }
        [Display(Name = "Roll# Index", Description = "The roll number index is the number from which a class roll no for students starts.")]
        public int RollNo { get; set; }
        [Display(Name = "Class Incharge")]
        public string Incharge { get; set; }
        [Display(Name = "Strength")]
        public int Strength { get; set; }
        public List<ClassSection> Sections { get; set; }
        public List<ClassSubject> Subjects { get; set; }
        public List<SectionMonthlyProgress> Progress { get; set; }
        public int Id { get; set; }

    }
    public struct SectionMonthlyProgress
    {
        public int Month { get; set; }
        public List<SectionProgress> SectionsProgres { get; set; }
    }
    public struct SectionProgress
    {
        public string SectionName { get; set; }
        public decimal ProgressPercent { get; set; }
    }
    public struct ClassSection
    {
        [Display(Name = "Section")]
        public string Name { get; set; }
        [Display(Name = "Strength")]
        public int Strength { get; set; }
        public int Id { get; set; }
    }
    public struct ClassSubject
    {
        [Display(Name = "Subject")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class ChangeClassInchargeViewModel
    {
        [Required]
        [Display(Name = "Choose Teacher")]
        public int TeacherId { get; set; }
    }
    public class AddSectionViewModel
    {
        [Required]
        [Display(Name = "Section Name")]
        [StringLength(5, MinimumLength = 1)]
        public string Name { get; set; }
    }
    public class AddSubjectViewModel
    {
        [Required]
        [Display(Name = "Subject Name")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
    public class ViewSectionViewModel
    {
        [Display(Name = "Section Name")]
        public string Name { get; set; }
        [Display(Name = "Class Name")]
        public string Class { get; set; }
        public int Id { get; set; }
        public List<SectionBrightStudent> BrightStudents { get; set; }
        public int StudentPresent { get; set; }
        public int StudentAbsent { get; set; }
        public List<SectionAssignedTeacher> AssignedTeachers { get; set; }
    }
    public class SectionAssignedTeacher
    {
        [Display(Name = "Teacher Name")]
        public string TName { get; set; }
        [Display(Name = "Subject Name")]
        public string SubName { get; set; }
        public int TId { get; set; }
        public int SubId { get; set; }
    }
    public class SectionStudent
    {
        [Display(Name = "Roll Number", ShortName = "Roll#")]
        public int RollNumber { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Addmission Number", ShortName = "Ad. Number")]
        public string AdNmbr { get; set; }
        [Display(Name = "Attendance")]
        public string Attendance { get; set; }
    }
    public class SectionBrightStudent
    {
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Roll Number", ShortName = "Roll#")]
        public int RollNumber { get; set; }
        [Display(Name = "Progress")]
        public decimal Progress { get; set; }
        [Display(Name = "Attendance")]
        public decimal Attendance { get; set; }
    }
    public class ViewStudentsViewModel
    {
        [Display(Name = "Addmision Number", ShortName = "Ad. #")]
        public string AddNmbr { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Father Name", ShortName = "Father")]
        public string FName { get; set; }
        [Display(Name = "CLass")]
        public string CName { get; set; }
        [Display(Name = "Roll Number", ShortName = "Roll#")]
        public int RollNumber { get; set; }
        [Display(Name = "Progress")]
        public int Progress { get; set; }
        public int Id { get; set; }
        public int ParentId { get; set; }

    }
    public class AssignTeacherViewModel
    {
        [Display(Name = "Choose Teacher")]
        [Required]
        public int TeacherId { get; set; }
    }
    public class ViewClassesViewModel
    {
        [Display(Name = "Class Name")]
        public string Name { get; set; }
        [Display(Name = "No. Of Sections")]
        public int Sections { get; set; }
        [Display(Name = "Strength")]
        public int Strength { get; set; }
        public int Id { get; set; }
    }
    public class LoadStudentsViewModel
    {
        [Required]
        [Display(Name = "Choose Class")]
        public int Class { get; set; }
        [Required]
        [Display(Name = "Choose Section")]
        public int Section { get; set; }
    }
    public class ViewStudentToPromoteViewModel
    {
        [Display(Name = "Roll#")]
        public int RollNo { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class ViewStudentAttendanceViewModel
    {
        [Display(Name = "Roll#")]
        public int RollNo { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Absent")]
        public bool IsAbsent { get; set; }
        public int Id { get; set; }
    }
    public class ViewStaffAttendanceViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }
        [Display(Name = "Absent")]
        public bool IsAbsent { get; set; }
        public int Id { get; set; }
    }
    public class LoadStudentAttendanceForViewModel
    {
        [Required]
        [Display(Name = "Choose Class")]
        public int Class { get; set; }
        [Required]
        [Display(Name = "Roll#")]
        public int RollNo { get; set; }
        [Required]
        [Display(Name = "Month")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CurrentMonthRange(ErrorMessage = "Please choose a valid month. (You can't choose the current month)")]
        public DateTime Month { get; set; }
    }
    public class LoadStaffAttendanceForViewModel
    {
        [Required]
        [Display(Name = "Staff Type")]
        public StaffTypes StaffType { get; set; }
        [Required]
        [Display(Name = "Choose Staff")]
        public int Staff { get; set; }
        [Required]
        [Display(Name = "Month")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CurrentMonthRange(ErrorMessage = "Please choose a valid month. (You can't choose the current month)")]
        public DateTime Month { get; set; }
    }
    public class ViewStudentAttendanceForViewModel
    {
        [Display(Name = "Roll#")]
        public int RollNo { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Attendance")]
        public decimal Attendance { get; set; }
        public List<MonthlyAttendanceViewModel> MonthlyAttendances { get; set; }
    }
    public class MonthlyAttendanceViewModel
    {
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Status")]
        public bool IsAbsent { get; set; }
        public long Id { get; set; }
    }
    public class ViewStaffAttendanceForViewModel
    {
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Attendance")]
        public decimal Attendance { get; set; }
        public List<MonthlyAttendanceViewModel> MonthlyAttendances { get; set; }
    }
    public class SendNotificationViewModel
    {
        [Required]
        [StringLength(500, ErrorMessage = "The body length shouldn't be greater than 500 and less than 3.", MinimumLength = 3)]
        [Display(Name = "Message Body")]
        public string Body { get; set; }
        [Required]
        [Display(Name = "Notification Type")]
        public NotificationTypes Type { get; set; }
        [Required]
        [Display(Name = "Notification Status")]
        public NotificationStatuses Status { get; set; }
        public IEnumerable<int> Parents { get; set; }
        public IEnumerable<int> Teachers { get; set; }
    }
    public class AddCategoryViewModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The length shouldn't be greater than 50 and less than 3.")]
        public string Name { get; set; }
    }

    public class AddItemViewModel
    {
        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Required]
        [DataType(DataType.Currency, ErrorMessage = "Enter a valid Price")]
        [Display(Name = "Price (Rs.)")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Choose Category")]
        public int Category { get; set; }
    }
    public class ViewItemDetailsViewModel
    {
        decimal price;
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Price")]
        public decimal Price
        {
            get
            {
                return decimal.Round(price, 2);
            }
            set
            {
                price = value;
            }
        }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
    public class ViewItemsViewModel
    {
        decimal price;
        [Display(Name = "Item Id")]
        public int Id { get; set; }
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public decimal Price
        {
            get
            {
                return decimal.Round(price, 2);
            }
            set
            {
                price = value;
            }
        }
    }
    public class SellItemViewModel
    {
        public IEnumerable<SellItem> ListSellItems { get; set; }
        public int Id { get; set; }
    }
    public struct SellItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}