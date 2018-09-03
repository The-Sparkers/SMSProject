using SMSProject.Models.HelperModels;
using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Student
    {
        string name, bFormNo, adNo, prevInst;
        Genders gender;
        int id, parentId, sectionId, rollNo;
        DateTime doa, dob;
        decimal monthlyFee;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Student(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Student(string name, string admissionNo, DateTime dateOfBirth, DateTime dateOfAddmission, decimal monthlyFee, Genders gender, int parentId, int sectionId, string connectionString, string bFormNumber="", string prevInst="")
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "AddStudent";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
                cmd.Parameters.Add(new SqlParameter("@bForm", System.Data.SqlDbType.NChar)).Value = bFormNumber;
                cmd.Parameters.Add(new SqlParameter("@addNo", System.Data.SqlDbType.VarChar)).Value = admissionNo;
                cmd.Parameters.Add(new SqlParameter("@dob", System.Data.SqlDbType.Date)).Value = dateOfBirth;
                cmd.Parameters.Add(new SqlParameter("@doa", System.Data.SqlDbType.Date)).Value = dateOfAddmission;
                cmd.Parameters.Add(new SqlParameter("@prevInst", System.Data.SqlDbType.VarChar)).Value = prevInst;
                cmd.Parameters.Add(new SqlParameter("@monthlyFee", System.Data.SqlDbType.Money)).Value = monthlyFee;
                cmd.Parameters.Add(new SqlParameter("@parentId", System.Data.SqlDbType.Int)).Value = parentId;
                cmd.Parameters.Add(new SqlParameter("@sectionId", System.Data.SqlDbType.Int)).Value = sectionId;
                cmd.Parameters.Add(new SqlParameter("@gender", System.Data.SqlDbType.Bit)).Value = (int)gender;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader[0];
                    rollNo = (int)reader[1];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Field(s) Admission Number exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:144", ex);
                throw e;
            }
            this.name = name;
            bFormNo = bFormNumber;
            adNo = admissionNo;
            dob = dateOfBirth;
            doa = dateOfAddmission;
            this.prevInst = prevInst;
            this.monthlyFee = monthlyFee;
            this.gender = gender;
            this.parentId = parentId;
            this.sectionId = sectionId;
        }
        private void SetValues()
        {
            try
            {
                query = "select * from STUDENTS where StudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bFormNo = (string)reader[1];
                    name = (string)reader[2];
                    adNo = (string)reader[3];
                    dob = (DateTime)reader[4];
                    doa = (DateTime)reader[5];
                    prevInst = (string)reader[6];
                    monthlyFee = (decimal)reader[7];
                    parentId = (int)reader[8];
                    sectionId = (int)reader[9];
                    rollNo = (int)reader[10];
                    gender = (Genders)Convert.ToInt16(reader[11]);
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:142", ex);
                throw e;
            }
        }
        public int StudentId
        {
            get
            {
                return id;
            }
        }
        public Parent Parent
        {
            get
            {
                return new Parent(parentId, con.ConnectionString);
            }
        }
        public Section Section
        {
            get
            {
                return new Section(sectionId, con.ConnectionString);
            }
            set
            {
                try
                {
                    query = "UpdateStudentSection";
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = id;
                    cmd.Parameters.Add(new SqlParameter("@sectionId", System.Data.SqlDbType.Int)).Value = value.SectionId;
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    sectionId = value.SectionId;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:161", ex);
                    throw e;
                }
            }
        }
        public int RollNumber
        {
            get
            {
                return rollNo;
            }
        }
        public Genders Gender
        {
            get
            {
                return gender;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set Gender=" + (int)value + " where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    gender = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:145", ex);
                    throw e;
                }
            }
        }
        public string PreviousInstitute
        {
            get
            {
                return prevInst;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set PrevInstitute='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    prevInst = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:146", ex);
                    throw e;
                }
            }
        }
        public DateTime DateOfBirth
        {
            get
            {
                return dob;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set DOB='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    dob = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:147", ex);
                    throw e;
                }
            }
        }
        public DateTime DateOfAdmission
        {
            get
            {
                return doa;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set DOA='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    doa = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:148", ex);
                    throw e;
                }
            }
        }
        public decimal MonthlyFee
        {
            get
            {
                return monthlyFee;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set MonthlyFee=" + value + " where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    monthlyFee = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:149", ex);
                    throw e;
                }
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set Name='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    name = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:150", ex);
                    throw e;
                }
            }
        }
        public string BFormNumber
        {
            get
            {
                return bFormNo;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set BFormNumber='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    bFormNo = value;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601)
                    {
                        throw new Exception("The data already exists. CodeIndex:143", ex);
                    }
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:151", ex);
                    throw e;
                }
            }
        }
        public string AdmissionNumber
        {
            get
            {
                return adNo;
            }
            set
            {
                try
                {
                    query = "update STUDENTS set AdmissionNo='" + value + "' where StudentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    adNo = value;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601)
                    {
                        throw new Exception("The data already exists. CodeIndex:143", ex);
                    }
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:151", ex);
                    throw e;
                }
            }
        }
        public int Age
        {
            get
            {
                int age = 0;
                age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                    age = age - 1;
                return age;
            }
        }
        public StudentMonthlyFee GetMonthlyFee(DateTime month)
        {
            StudentMonthlyFee st;
            try
            {
                query = "select SFeeId from STUDENT_MONTHLY_FEE where [Month]=" + month.Month + " and [Year]=" + month.Year + " and StudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                try
                {
                    st = new StudentMonthlyFee((long)cmd.ExecuteScalar(), con.ConnectionString);
                }
                catch (NullReferenceException)
                {
                    st = null;
                }
                con.Close();
                return st;
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:152", ex);
                throw e;
            }
        }
        public List<PurchasedItemsDetails> GetPurchasedItems(DateTime month)
        {
            DateTime monthStart = new DateTime(month.Year, month.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            List<PurchasedItemsDetails> lst = new List<PurchasedItemsDetails>();
            try
            {
                query = "select * from STUDENT_PURCHASES_ITEMS where Date between '" + monthStart + "' and '" + monthEnd + "' and StudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new PurchasedItemsDetails()
                    {
                        Date = (DateTime)reader[3],
                        Item = new InventoryItem((int)reader[1], con.ConnectionString),
                        Quantity = (int)reader[2]
                    });
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:153", ex);
                throw e;
            }
            return lst;
        }
        public ExamResult GetExamResult(int examinationId)
        {
            ExamResult result = new ExamResult();
            result.ResultList = new List<SubjectResult>();
            try
            {
                query = "StudentExamResult";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@examId", System.Data.SqlDbType.Int)).Value = examinationId;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.ResultList.Add(new SubjectResult()
                    {
                        Subject = new Subject((int)reader[0], con.ConnectionString),
                        MarksObtained = (decimal)reader[1],
                        TotalMarks = (decimal)reader[2],
                        TeacherRemarks = (string)reader[3]
                    });
                    result.PositionInClass = (long)reader[4];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:154", ex);
                throw e;
            }
            return result;
        }
        public TestResult GetTestResult(Test test)
        {
            TestResult result = new TestResult();
            try
            {
                query = "Select * From STUDENT_GET_TEST_RESULTS where TestId=" + test.TestId + " and StudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.MarksObtained = (decimal)reader[2];
                    result.TeacherRemakrs = (string)reader[3];
                    result.TotalMarks = test.TotalMarks;
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:155", ex);
                throw e;
            }
            return result;
        }
        public List<Test> GetGivenTests()
        {
            List<Test> lst = new List<Test>();
            try
            {
                query = "SELECT DISTINCT st.TestId FROM STUDENT_GET_TEST_RESULTS st, STUDENTS s, SUBJECTS sb WHERE sb.ClassId=(SELECT ClassId FROM SECTIONS WHERE SectionId=s.SectionId) AND st.StudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Test((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:156", ex);
                throw e;
            }
            return lst;
        }
        public Dairy GetDairy(DateTime date)
        {
            Dairy dairy = new Dairy();
            dairy.SubjectsDairy = new List<SubjectDiary>();
            try
            {
                query = "GetStudentDairy";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = date;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dairy.SubjectsDairy.Add(new SubjectDiary()
                    {
                        Content = (string)reader[0],
                        Subject = new Subject((int)reader[1], con.ConnectionString)
                    });
                }
                dairy.Date = date;
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:157", ex);
                throw e;
            }
            return dairy;
        }
        public void SetAttandance(DateTime date, bool isAbsent = false)
        {
            try
            {
                query = "SetStudentAttandance";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = date;
                cmd.Parameters.Add(new SqlParameter("@isAbsent", System.Data.SqlDbType.Bit)).Value = isAbsent;
                con.Open();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Nothing has updated in the database. CodeIndex:108");
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:158", ex);
                throw e;
            }
        }
        public int GetAbsents(DateTime month)
        {
            int absents = 0;
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                query = "SELECT COUNT(*) FROM STUDENT_GIVES_DAILY_ATTANDANCE sa, ATTANDANCES a WHERE sa.AttandanceId=a.AttandanceId AND sa.StudentId=" + id + " AND (a.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "') AND a.IsAbsent=1";
                cmd = new SqlCommand(query, con);
                con.Open();
                absents = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:159", ex);
                throw e;
            }
            return absents;
        }
        public List<Attendance> GetMonthAttendances(DateTime month)
        {
            List<Attendance> lst = new List<Attendance>();
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                query = "SELECT a.AttandanceId FROM ATTANDANCES a,STUDENT_GIVES_DAILY_ATTANDANCE st WHERE st.AttandanceId=a.AttandanceId AND st.StudentId=" + id + " AND (a.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Attendance((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:160", ex);
                throw e;
            }
            return lst;
        }
        public static List<Student> GetAllStudents(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<Student> lst = new List<Student>();
            try
            {
                string query = "SELECT StudentId FROM STUDENTS";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Student((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:162", ex);
                throw e;
            }
            return lst;
        }
        public static List<Student> Search(string matchName, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<Student> lst = new List<Student>();
            try
            {
                string query = "SELECT StudentId FROM STUDENTS WHERE Name LIKE '" + matchName + "%'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Student((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:163", ex);
                throw e;
            }
            return lst;
        }
    }
}