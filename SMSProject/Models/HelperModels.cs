using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;

namespace SMSProject.Models.HelperModels
{
    public class AssignedSection
    {
        int subjectId, sectionId;
        string connectionString;
        public AssignedSection(int subjectId, int sectionId, string connectionString)
        {
            this.sectionId = sectionId;
            this.subjectId = subjectId;
            this.connectionString = connectionString;
        }
        public Subject Subject
        {
            get
            {
                return new Subject(subjectId, connectionString);
            }
        }
        public Section Section
        {
            get
            {
                return new Section(sectionId, connectionString);
            }
        }
    }
    public struct Qualification
    {
        public int Id { get; set; }
        public string Degree { get; set; }
        public short Year { get; set; }
    }
    public struct WebNotification
    {
        public long NotificationId { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
    }
    public struct PurchasedItemsDetails
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public InventoryItem Item { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return decimal.Multiply(Item.Price, Convert.ToDecimal(Quantity));
            }
        }
    }
    public class MobileNumber
    {
        string countryCode, companyCode, number;
        public MobileNumber(string countryCode, string companyCode, string number)
        {
            this.countryCode = countryCode;
            this.companyCode = companyCode;
            this.number = number;
        }
        public string CountryCode
        {
            get
            {
                return Convert.ToString(countryCode);
            }
        }
        public string CompanyCode
        {
            get
            {
                return Convert.ToString(companyCode);
            }
        }
        public string Number
        {
            get
            {
                return Convert.ToString(number);
            }
        }
        public string GetInternationalFormat()
        {
            return CountryCode + CompanyCode + Number;
        }
        public string GetLocalFormat()
        {
            return "0" + CompanyCode + Number;
        }
        public string GetLocalViewFormat()
        {
            return "0" + CompanyCode + "-" + Number;
        }
    }
    public class ExamResult
    {
        public List<SubjectResult> ResultList { get; set; }
        public long PositionInClass { get; set; }
        public decimal TotalMarks
        {
            get
            {
                decimal total = 0;
                foreach (var item in ResultList)
                {
                    total += item.TotalMarks;
                }
                return total;
            }
        }
        public decimal ObtainedMarks
        {
            get
            {
                decimal total = 0;
                foreach (var item in ResultList)
                {
                    total += item.MarksObtained;
                }
                return total;
            }
        }
    }
    public struct SubjectResult
    {
        public Subject Subject { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal MarksObtained { get; set; }
        public string TeacherRemarks { get; set; }
        public decimal Percentage
        {
            get
            {
                return decimal.Multiply(decimal.Divide(MarksObtained, TotalMarks), 100);
            }
        }
    }
    public class TestResult
    {
        decimal totalMarks;
        public decimal MarksObtained { get; set; }
        public decimal TotalMarks
        {
            set
            {
                totalMarks = value;
            }
        }
        public int Percentage
        {
            get
            {
                return Convert.ToInt32(decimal.Multiply(decimal.Divide(MarksObtained, totalMarks), 100));
            }
        }
        public string TeacherRemakrs { get; set; }
    }
    public struct SubjectDiary
    {
        public Subject Subject { get; set; }
        public string Content { get; set; }
    }
    public class Dairy
    {
        public List<SubjectDiary> SubjectsDairy { get; set; }
        public DateTime Date { get; set; }
    }
    public struct Month
    {
        public int Number { get; set; }
        public int Year { get; set; }
        public MonthNames Name
        {
            get
            {
                return (MonthNames)Number;
            }
        }
    }
}