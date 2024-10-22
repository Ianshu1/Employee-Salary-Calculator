namespace Employee.Exceptions
{
    public class InvalidEmployeeMonthlyReportException : Exception
    {
        public InvalidEmployeeMonthlyReportException() : base("Start date cannot be after end date")
        {
        }
    }
}
