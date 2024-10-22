namespace Employee.Exceptions
{
    public class InvalidWorkTimeException : Exception
    {
        public InvalidWorkTimeException() : base("Hours or minutes are invalid.")
        {
        }
    }
}
