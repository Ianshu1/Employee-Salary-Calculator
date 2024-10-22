namespace Employee.Exceptions
{
    public class EmployeeIdExistException : Exception
    {
        public EmployeeIdExistException(int id) : base($"Employee with id: {id} already exist")
        {
        }
    }
}
