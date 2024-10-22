namespace Employee.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(int id) : base($"Employee with id: {id} was not found")
        {
        }
    }
}
