namespace SMSProject.Common.HelperModels
{
    public struct SectionProgess
    {
        public decimal ObtainedMarks { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ResultPercentage
        {
            get
            {
                return decimal.Multiply(decimal.Divide(ObtainedMarks, TotalMarks), 100);
            }
        }
    }
}