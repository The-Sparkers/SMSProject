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
        SMS,
        Web,
        All
    }
    public enum NotificationStatuses
    {
        ForTeacher,
        ForParent,
        ForAll
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