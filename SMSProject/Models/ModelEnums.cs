namespace SMSProject.Models.ModelEnums
{
    public enum Genders
    {
        Male = 1,
        Female = 0
    }
    public enum ApplicationStatuses
    {
        Accepted=1,
        Rejected=2,
        Pending=0
    }
    public enum NotificationTypes
    {
        SMS=3,
        Web=2,
        All=1
    }
    public enum NotificationStatuses
    {
        ForTeacher=3,
        ForParent=2,
        ForAll=1
    }
    public enum MonthNames
    {
        January=1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public enum ModelRoles
    {
        Admin,
        Teacher,
        Parent
    }
}