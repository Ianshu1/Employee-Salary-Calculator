namespace Employee
{
    public class WorkEntry
    {
        public DateTime StartDateTime { get; set; }
        public int Hours { get; }
        public int EmployeeId { get; set; }
        public int Minutes { get; set; }

        public WorkEntry(int employeeId, DateTime startDateTime, int hours, int minutes)
        {
            EmployeeId = employeeId;
            StartDateTime = startDateTime;
            Hours = hours;
            Minutes = minutes;
        }
    }
}
