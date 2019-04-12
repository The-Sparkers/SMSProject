using SMSProject.Models.HelperModels;
using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Parent whose student(s) are registered to the school
    /// One of the main user of the system
    /// </summary>
    public class Parent
    {
        string fatherName, motherName, fCNIC, homeAddress, emergencyContact;
        int id, elgibilityThreshold;
        MobileNumber fPhone, mPhone;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Parent(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Parent(string fatherCNIC, string connectionString)
        {
            try
            {
                con = new SqlConnection(connectionString);
                query = "SELECT ParentId FROM PARENTS WHERE FCNIC='" + fatherCNIC + "'";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
                SetValues();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:264", ex);
                throw e;
            }
        }
        public Parent(string fatherName, string motherName, string fCNIC, MobileNumber fPhone, MobileNumber mPhone, string homeAddress, string emergencyContact, int eligibiltyThreshold, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "AddNewParent";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fName", System.Data.SqlDbType.VarChar)).Value = fatherName;
                cmd.Parameters.Add(new SqlParameter("@motherName", System.Data.SqlDbType.VarChar)).Value = motherName;
                cmd.Parameters.Add(new SqlParameter("@fCNIC", System.Data.SqlDbType.NChar)).Value = fCNIC;
                cmd.Parameters.Add(new SqlParameter("@homeAddress", System.Data.SqlDbType.VarChar)).Value = homeAddress;
                cmd.Parameters.Add(new SqlParameter("@fmCountryCode", System.Data.SqlDbType.NChar)).Value = fPhone.CountryCode;
                cmd.Parameters.Add(new SqlParameter("@fmCompanyCode", System.Data.SqlDbType.NChar)).Value = fPhone.CompanyCode;
                cmd.Parameters.Add(new SqlParameter("@fmNumber", System.Data.SqlDbType.NChar)).Value = fPhone.Number;
                cmd.Parameters.Add(new SqlParameter("@mmCountryCode", System.Data.SqlDbType.NChar)).Value = mPhone.CountryCode;
                cmd.Parameters.Add(new SqlParameter("@mmCompanyCode", System.Data.SqlDbType.NChar)).Value = mPhone.CompanyCode;
                cmd.Parameters.Add(new SqlParameter("@mmNumber", System.Data.SqlDbType.NChar)).Value = mPhone.Number;
                cmd.Parameters.Add(new SqlParameter("@emergencyContact", System.Data.SqlDbType.VarChar)).Value = emergencyContact;
                cmd.Parameters.Add(new SqlParameter("@eThreshold ", System.Data.SqlDbType.Int)).Value = eligibiltyThreshold;
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Field CNIC exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:100", ex);
                throw e;
            }
            this.fatherName = fatherName;
            this.motherName = motherName;
            this.mPhone = mPhone;
            this.fCNIC = fCNIC;
            this.fPhone = fPhone;
            this.emergencyContact = emergencyContact;
            this.elgibilityThreshold = eligibiltyThreshold;
            this.homeAddress = homeAddress;
        }
        private void SetValues()
        {
            try
            {
                query = "select * from PARENTS where [ParentId]=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fatherName = (string)reader[1];
                    motherName = (string)reader[2];
                    fCNIC = (string)reader[3];
                    homeAddress = (string)reader[4];
                    fPhone = new MobileNumber((string)reader[5], (string)reader[6], (string)reader[7]);
                    mPhone = new MobileNumber((string)reader[8], (string)reader[9], (string)reader[10]);
                    emergencyContact = (string)reader[11];
                    elgibilityThreshold = (int)reader[12];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:102", ex);
                throw e;
            }
        }
        /// <summary>
        /// checks threshold to which the parent is allowed to use the system.
        /// Parent will have to clear the account according to the threshold set by the admin.
        /// </summary>
        public bool IsEligible
        {
            get
            {
                decimal total = 0;
                try
                {
                    query = "SELECT SUM(MonthlyFee) FROM STUDENTS WHERE ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    total = (decimal)cmd.ExecuteScalar(); 
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:266", ex);
                    throw e;
                }
                decimal eligibility = -(decimal.Multiply(total, decimal.Divide(elgibilityThreshold, 100))); 
                if (Balance < eligibility)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public string FatherName
        {
            get
            {
                return fatherName;
            }
            set
            {
                try
                {
                    query = "update PARENTS set FatherName='" + value + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    fatherName = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:101", ex);
                    throw e;
                }
            }
        }
        public int ParentId
        {
            get
            {
                return id;
            }
        }
        public string MotherName
        {
            get
            {
                return motherName;
            }
            set
            {
                try
                {
                    query = "update PARENTS set MotherName='" + value + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    motherName = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:103", ex);
                    throw e;
                }
            }
        }
        public string FatherCNIC
        {
            get
            {
                return fCNIC;
            }
            set
            {
                try
                {
                    query = "update PARENTS set FCNIC='" + value + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    fCNIC = value;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601)
                    {
                        throw new Exception("Field CNIC exists. CodeIndex:143", ex);
                    }
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:104", ex);
                    throw e;
                }
            }
        }
        public string HomeAddress
        {
            get
            {
                return homeAddress;
            }
            set
            {
                try
                {
                    query = "update PARENTS set HomeAddress='" + value + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    homeAddress = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:105", ex);
                }
            }
        }
        public int UnreadNotificationsCount
        {
            get
            {
                int count = 0;
                try
                {
                    query = "select COUNT(*) from PARENT_RECEIVES_NOTIFICATIONS where IsRead=0 and ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:131", ex);
                    throw e;
                }
                return count;
            }
        }
        public string EmergencyContact
        {
            get
            {
                return emergencyContact;
            }
            set
            {
                try
                {
                    query = "update PARENTS set EmergencyContact='" + value + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    emergencyContact = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:106", ex);
                    throw e;
                }
            }
        }
        public int EligibiltyThreshold
        {
            get
            {
                return elgibilityThreshold;
            }
            set
            {
                try
                {
                    query = "update PARENTS set EligibiltyThreshold=" + value + " where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. Code Index:108");
                    }
                    con.Close();
                    elgibilityThreshold = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:107", ex);
                    throw e;
                }
            }
        }
        public MobileNumber FatherMobile
        {
            get
            {
                return fPhone;
            }
            set
            {
                try
                {
                    query = "update PARENTS set FMCountryCode='" + value.CountryCode + "', FMCompanyCode='" + value.CompanyCode + "', FMNumber='" + value.Number + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. Code Index:108");
                    }
                    con.Close();
                    fPhone = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:107", ex);
                    throw e;
                }
            }
        }
        public MobileNumber MotherMobile
        {
            get
            {
                return mPhone;
            }
            set
            {
                try
                {
                    query = "update PARENTS set FMCountryCode='" + value.CountryCode + "', FMCompanyCode='" + value.CompanyCode + "', FMNumber='" + value.Number + "' where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. Code Index:108");
                    }
                    con.Close();
                    mPhone = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:109", ex);
                    throw e;
                }
            }
        }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                try
                {
                    query = "Select -(TotalAmountDue-Concession-AmountPaid) from PARENT_MONTHLY_FEE where PFeeId=(Select MAX(PFeeId) from PARENT_MONTHLY_FEE where ParentId=" + id + ")";
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        balance = (decimal)cmd.ExecuteScalar();
                    }
                    catch (NullReferenceException)
                    {
                        balance = 0;
                    }
                    con.Close();
                    return balance;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:111", ex);
                    throw e;
                }
            }
        }
        public ParentAccount Account
        {
            get
            {
                string accoutnId = "";
                try
                {
                    query = "select AccountId from PARENT_HAS_ACCOUNT where ParentId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    accoutnId = (string)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:119", ex);
                    throw e;
                }
                return new ParentAccount(accoutnId, con.ConnectionString);
            }
        }
        public ModelRoles Role
        {
            get
            {
                return ModelRoles.Parent;
            }
        }
        /// <summary>
        /// Gets the list of all applications submitted by this parent
        /// </summary>
        /// <returns></returns>
        public List<Application> GetAllApplications()
        {
            List<Application> lst = new List<Application>();
            try
            {
                query = "SELECT ApplicationId FROM APPLICATIONS WHERE ParentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Application((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:116", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets a list of all notifications sent to this parent by the admin
        /// </summary>
        /// <returns></returns>
        public List<Notification> GetAllReceivedNotifications()
        {
            List<Notification> lst = new List<Notification>();
            try
            {
                query = "select NotificationId from PARENT_RECEIVES_NOTIFICATIONS where ParentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Notification((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:117", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Method to send a notification to this parent
        /// </summary>
        /// <param name="notification">Notification to be sent</param>
        /// <returns>true is the notiification will be sent successfully</returns>
        public bool SendNotification(Notification notification)
        {
            bool check = false;
            if (notification.IsSMS && notification.ForParent)
            {
                string messageBody = "Dear " + FatherName + ",\n" + notification.Message;
                if (ServiceModules.SMSService.SendSMS(messageBody, FatherMobile.GetInternationalFormat()))
                    check = true;
                messageBody = "Dear " + motherName + ",\n" + notification.Message;
                if (ServiceModules.SMSService.SendSMS(messageBody, MotherMobile.GetInternationalFormat()))
                    check = true;
            }
            if (notification.IsWeb && notification.ForParent)
            {
                try
                {
                    query = "insert into PARENT_RECEIVES_NOTIFICATIONS (ParentId, NotificationId) values (" + id + "," + notification.NotificationId + ")";
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    int flag = cmd.ExecuteNonQuery();
                    con.Close();
                    if (flag != 0)
                    {
                        check = false;
                    }
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:118", ex);
                    throw e;
                }
            }
            return check;
        }
        /// <summary>
        /// Method to set the status to read, of a notification received by this parent.
        /// </summary>
        /// <param name="notification">notification received</param>
        public void ReadNotification(Notification notification)
        {
            try
            {
                query = "update PARENT_RECEIVES_NOTIFICATIONS set IsRead=1 where ParentId=" + id + " and NotificationId=" + notification.NotificationId;
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Nothing has updated in the database. Code Index:108");
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:118", ex);
                throw e;
            }
        }
        /// <summary>
        /// Generates the Fee Record for this parent for a particular month
        /// </summary>
        /// <param name="month">number of month(e.g. 1 for Jan, 2 for Feb, etc)</param>
        /// <param name="year">year to which the month belongs to (e.g. 2017)</param>
        /// <returns></returns>
        public List<ParentFeeRecord> GetFeeRecord(int month, int year)
        {
            List<ParentFeeRecord> lst = new List<ParentFeeRecord>();
            DateTime monthStart = new DateTime(year, month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            try
            {
                query = "select PFeeId from PARENT_MONTHLY_FEE where ParentId=" + id + " and (DatePaid between '" + monthStart + "' and '" + monthEnd + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new ParentFeeRecord((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:115", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Generates the record for the billing done by this parent
        /// </summary>
        /// <returns></returns>
        public List<ParentFeeRecord> BillingHistory()
        {
            List<ParentFeeRecord> lst = new List<ParentFeeRecord>();
            try
            {
                query = "select PFeeId from PARENT_MONTHLY_FEE where ParentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new ParentFeeRecord((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:114", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets the fee pending for a particular month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public decimal GetMonthFee(DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            decimal total = 0;
            try
            {
                query = "GetParentMonthFee";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month", System.Data.SqlDbType.SmallInt)).Value = month.Month;
                cmd.Parameters.Add(new SqlParameter("@year", System.Data.SqlDbType.SmallInt)).Value = month.Year;
                cmd.Parameters.Add(new SqlParameter("@parentId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@startdate", System.Data.SqlDbType.Date)).Value = startDate;
                cmd.Parameters.Add(new SqlParameter("@endDate", System.Data.SqlDbType.Date)).Value = endDate;
                con.Open();
                total = (decimal)cmd.ExecuteScalar();
                con.Close();
                return total;
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:110", ex);
                throw e;
            }
        }
        /// <summary>
        /// Gets the notifications received on the web
        /// </summary>
        /// <returns></returns>
        public List<WebNotification> GetAllWebNotifications()
        {
            List<WebNotification> lst = new List<WebNotification>();
            try
            {
                query = "Select pn.NotificationId, n.Message, pn.IsRead from PARENT_RECEIVES_NOTIFICATIONS pn, NOTIFICATIONS n where pn.NotificationId=n.NotificationId and IsWeb=1 and pn.ParentId=" + id + "order by n.Date DESC";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new WebNotification()
                    {
                        NotificationId = (long)reader[0],
                        Body = (string)reader[1],
                        IsRead = (bool)reader[2]
                    });
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:126", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets a list of students registered in the school
        /// </summary>
        /// <returns></returns>
        public List<Student> GetAllStudents()
        {
            List<Student> lst = new List<Student>();
            try
            {
                query = "select StudentId from STUDENTS where ParentId=" + id;
                cmd = new SqlCommand(query, con);
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:112", ex);
                throw e;
            }
            return lst;
        }
        public static List<Parent> GetAllParents(string connectionString, string matchName = "")
        {
            List<Parent> lst = new List<Parent>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "select ParentId from PARENTS where FatherName LIKE '" + matchName + "%'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Parent((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:113", ex);
                throw e;
            }
            return lst;
        }
        public static List<Parent> GetAllParentsByCNIC(string connectionString, string matchCNIC = "")
        {
            List<Parent> lst = new List<Parent>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "select ParentId from PARENTS where FCNIC ='" + matchCNIC + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Parent((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:113", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets a Lst of Parents who have not cleared their dues for a month.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Parent> GetParentsWithUnpaidDues(string connectionString, DateTime month)
        {
            List<Parent> lst = new List<Parent>();
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT p.ParentId FROM PARENTS p WHERE NOT EXISTS(SELECT pm.ParentId FROM PARENT_MONTHLY_FEE pm WHERE pm.ParentId=p.ParentId AND (pm.DatePaid BETWEEN '" + startDate + "' AND '" + endDate + "'))";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Parent((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:166", ex);
                throw e;
            }
            return lst;
        }
    }
}