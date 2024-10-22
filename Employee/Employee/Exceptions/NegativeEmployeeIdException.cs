namespace Employee.Exceptions
{
    public class NegativeEmployeeIdException : Exception
    {
        public NegativeEmployeeIdException() : base("Id cannot be negative")
        {
        }
    }
}
