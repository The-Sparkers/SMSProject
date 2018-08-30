using SMSProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
                return decimal.Round(ParentFeeRecord.GetTotalDue(DateTime.Now, conString));
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
}