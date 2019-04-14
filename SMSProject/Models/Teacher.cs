using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SMSProject.Models.HelperModels;
using SMSProject.Models.ModelEnums;

namespace SMSProject.Models
{
    /// <summary>
    /// A Teacher is a staff member who teaches in the school
    /// </summary>
    public class Teacher : Staff
    {
        public Teacher(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Teacher(string name, string cnic, string address, MobileNumber number, decimal salary, Genders gender, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "AddTeacher";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
                cmd.Parameters.Add(new SqlParameter("@CNIC", System.Data.SqlDbType.NChar)).Value = cnic;
                cmd.Parameters.Add(new SqlParameter("@address", System.Data.SqlDbType.VarChar)).Value = address;
                cmd.Parameters.Add(new SqlParameter("@mCountryCode", System.Data.SqlDbType.NChar)).Value = number.CountryCode;
                cmd.Parameters.Add(new SqlParameter("@mCompanyCode", System.Data.SqlDbType.NChar)).Value = number.CompanyCode;
                cmd.Parameters.Add(new SqlParameter("@mNumber", System.Data.SqlDbType.NChar)).Value = number.Number;
                cmd.Parameters.Add(new SqlParameter("@salary", System.Data.SqlDbType.Money)).Value = salary;
                cmd.Parameters.Add(new SqlParameter("@gender", System.Data.SqlDbType.Bit)).Value = (int)gender;
                cmd.Parameters.Add(new SqlParameter("@joiningDate", System.Data.SqlDbType.Date)).Value = DateTime.Now;
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:240", ex);
                throw e;
            }
            this.name = name;
            this.number = number;
            this.salary = salary;
            this.address = address;
            this.cnic = cnic;
        }
        public List<Qualification> Qualifications
        {
            get
            {
                List<Qualification> lst = new List<Qualification>();
                try
                {
                    query = "SELECT * FROM TEACHERS_QUALIFICATIONS WHERE StaffId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lst.Add(new Qualification()
                        {
                            Id = (int)reader[0],
                            Degree = (string)reader[2],
                            Year = (short)reader[3]
                        });
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:241", ex);
                    throw e;
                }
                return lst;
            }
        }
        public TeacherAccount Account
        {
            get
            {
                string accoutnId = "";
                try
                {
                    query = "select AccountId from TEACHER_HAS_ACCOUNT where StaffId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    accoutnId = (string)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:160", ex);
                    throw e;
                }
                return new TeacherAccount(accoutnId, con.ConnectionString);
            }
        }
        public ModelRoles Role
        {
            get
            {
                return ModelRoles.Teacher;
            }
        }
        public bool AddQualification(Qualification qualification)
        {
            bool flag = false;
            try
            {
                query = "INSERT INTO [TEACHERS_QUALIFICATIONS] ([StaffId] ,[Degree] ,[Year]) VALUES (" + id + " ,'" + qualification.Degree + "' ," + qualification.Year + ")";
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    flag = true;
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:242", ex);
                throw e;
            }
            return flag;
        }
        public bool RemoveQualification(Qualification qualification)
        {
            bool flag = false;
            if (qualification.Id != 0)
            {
                try
                {
                    query = "DELETE TEACHERS_QUALIFICATIONS WHERE StaffId=" + id + " AND QId=" + qualification.Id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        flag = true;
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:243", ex);
                    throw e;
                }
            }
            return flag;
        }
        /// <summary>
        /// Method to assign a section to this teacher
        /// </summary>
        /// <param name="sectionId">section id to which this teacher is going to assign</param>
        /// <param name="subjectId">subject id of which this teacher is going to teach</param>
        /// <returns></returns>
        public bool AssignSection(int sectionId, int subjectId)
        {
            bool flag = false;
            try
            {
                query = "INSERT INTO [TEACHER_TEACHES_SUBJECT_OF_A_SECTION] ([StaffId] ,[SubjectId] ,[SectionId]) VALUES (" + id + " ," + subjectId + " ," + sectionId + ")";
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    flag = true;
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return false;
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:245", ex);
                throw e;
            }
            return flag;
        }
        public bool RemoveSection(int sectionId, int subjectId)
        {
            bool flag = false;
            try
            {
                query = "DELETE FROM [TEACHER_TEACHES_SUBJECT_OF_A_SECTION] WHERE [StaffId]=" + id + " AND [SubjectId]=" + subjectId + " AND [SectionId]=" + sectionId;
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    flag = true;
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return false;
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:273", ex);
                throw e;
            }
            return flag;
        }
        /// <summary>
        /// Gets a list of sections which have been assigned to this teacher
        /// </summary>
        /// <returns></returns>
        public List<AssignedSection> GetAssignedSections()
        {
            List<AssignedSection> lst = new List<AssignedSection>();
            try
            {
                query = "SELECT * FROM TEACHER_TEACHES_SUBJECT_OF_A_SECTION WHERE StaffId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new AssignedSection((int)reader[1], (int)reader[2], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:246", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets a list of the classes which have been incharged by this teacher
        /// </summary>
        /// <returns></returns>
        public List<Class> GetInchargedClasses()
        {
            List<Class> lst = new List<Class>();
            try
            {
                query = "SELECT ClassId FROM CLASSES WHERE InchargeId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Class((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:247", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets all the notifications sent to this taecher
        /// </summary>
        /// <returns></returns>
        public List<Notification> GetAllReceivedNotifications()
        {
            List<Notification> lst = new List<Notification>();
            try
            {
                query = "select NotificationId  from TEACHER_RECEIVES_NOTIFICATIONS where StaffId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Notification((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:248", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Gets all notifications sent to this teacher on web
        /// </summary>
        /// <returns></returns>
        public List<WebNotification> GetAllWebNotifications()
        {
            List<WebNotification> lst = new List<WebNotification>();
            try
            {
                query = "Select tn.NotificationId, n.Message, tn.IsRead from TEACHER_RECEIVES_NOTIFICATIONS tn, NOTIFICATIONS n where tn.NotificationId=n.NotificationId and IsWeb=1 and tn.StaffId=" + id + " order by n.Date DESC";
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:249", ex);
                throw e;
            }
            return lst;
        }
        /// <summary>
        /// Sets the status of a particular notification to read
        /// </summary>
        /// <param name="notification"></param>
        public void ReadNotification(Notification notification)
        {
            try
            {
                query = "update TEACHER_RECEIVES_NOTIFICATIONS set IsRead=1 where StaffId=" + id + " and NotificationId=" + notification.NotificationId;
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:250", ex);
                throw e;
            }
        }
        /// <summary>
        /// Method to send notification to this teacher
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public bool SendNotification(Notification notification)
        {
            bool check = false;
            if (notification.IsSMS && notification.ForTeacher)
            {
                string messageBody = "Dear " + Name + ",\n" + notification.Message;
                if (ServiceModules.SMSService.SendSMS(messageBody, PhoneNumber.GetInternationalFormat()))
                    check = true;
            }
            if (notification.IsWeb && notification.ForTeacher)
            {
                try
                {
                    query = "insert into TEACHER_RECEIVES_NOTIFICATIONS (StaffId, NotificationId) values (" + id + "," + notification.NotificationId + ")";
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    int flag = cmd.ExecuteNonQuery();
                    con.Close();
                    if (flag != 0)
                    {
                        check = true;
                    }
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:251", ex);
                    throw e;
                }
            }
            return check;
        }
        /// <summary>
        /// Number of Notifications which are sent to this teacher but are not read by the teacher
        /// </summary>
        public int UnreadNotificationsCount
        {
            get
            {
                int count = 0;
                try
                {
                    query = "select COUNT(*) from TEACHER_RECEIVES_NOTIFICATIONS where IsRead=0 and StaffId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:252", ex);
                    throw e;
                }
                return count;
            }
        }
        public static List<Teacher> GetAllTeachers(string connectionString, string matchName = "")
        {
            List<Teacher> lst = new List<Teacher>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT s.StaffId FROM STAFF s , TEACHING_STAFF ts WHERE s.StaffId=ts.StaffId AND s.Name LIKE '%" + matchName + "%'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Teacher((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:254", ex);
                throw e;
            }
            return lst;
        }
    }
}