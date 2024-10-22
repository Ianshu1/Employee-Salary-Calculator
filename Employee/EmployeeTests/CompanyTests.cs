using Employee.Exceptions;
using Employee;
using FluentAssertions;

namespace EmployeeTests
{
    [TestClass]
    public class CompanyTests
    {
        private Company _company;
        private const string Name = "Test";

        [TestInitialize]
        public void Setup()
        {
            _company = new Company(Name);
        }

        [TestMethod]
        public void Name_CreatingCompanyName_NameCreated()
        {
            _company.Name.Should().Be(Name);
        }

        [TestMethod]
        public void Employees_WhenCompanyIsCreated_ShouldReturnEmptyEmployeeArray()
        {
            var employees = _company.Employees;

            employees.Should().BeEmpty();
        }

        [TestMethod]
        public void AddEmployee_WhenNewEmployeeIsAdded_ShouldAppearInEmployeesArray()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };

            _company.AddEmployee(employee, DateTime.Now);

            _company.Employees.Length.Should().Be(1);
        }

        [TestMethod]
        public void AddEmployee_AddsSameId_ShouldThrowIdExistException()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            Action action = () => _company.AddEmployee(employee, DateTime.Now);

            action.Should().Throw<EmployeeIdExistException>();
        }

        [TestMethod]
        public void AddEmployee_AddsNegativeEmployeeId_ShouldThrowNegativeIdEmployeeException()
        {
            var employee = new Employee.Employee { Id = -1, FullName = "John Doe", HourlySalary = 25m };

            Action action = () => _company.AddEmployee(employee, DateTime.Now);

            action.Should().Throw<NegativeEmployeeIdException>();
        }

        [TestMethod]
        public void RemoveEmployee_RemovesEmployee_ShouldReturnSmallerSizeListOfEmployees()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            var employee2 = new Employee.Employee { Id = 2, FullName = "Alex Trump", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);
            _company.AddEmployee(employee2, DateTime.Now);

            _company.RemoveEmployee(2, DateTime.Now);

            _company.Employees.Length.Should().Be(1);
        }

        [TestMethod]
        public void RemoveEmployee_RemovesNoneExistingEmployee_ShouldThrowEmployeeNotFoundException()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            Action action = () => _company.RemoveEmployee(2, DateTime.Now);

            action.Should().Throw<EmployeeNotFoundException>();
        }

        [TestMethod]
        public void ReportHours_NonExistentEmployee_ShouldThrowEmployeeNotFoundException()
        {
            Action action = () => _company.ReportHours(999, DateTime.Now, 8, 0);

            action.Should().Throw<EmployeeNotFoundException>();
        }

        [TestMethod]
        public void ReportHours_InvalidMinutes_ShouldThrowInvalidWorkTimeException()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            Action action = () => _company.ReportHours(employee.Id, DateTime.Now, 8, 70);

            action.Should().Throw<InvalidWorkTimeException>();
        }

        [TestMethod]
        public void ReportHours_NegativeHours_ShouldThrowInvalidWorkTimeException()
        {

            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            Action action = () => _company.ReportHours(employee.Id, DateTime.Now, -1, 30);

            action.Should().Throw<InvalidWorkTimeException>();
        }

        [TestMethod]
        public void ReportHours_ValidInput_ShouldRecordHours()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            _company.ReportHours(employee.Id, DateTime.Now, 8, 30);

            var workEntries = _company.GetWorkEntries(employee.Id);
            workEntries.Count.Should().Be(1);
            workEntries.First().Hours.Should().Be(8);
            workEntries.First().Minutes.Should().Be(30);
        }

        [TestMethod]
        public void GetMonthlyReport_ValidEntries_ReturnsCorrectReports()
        {
            var employee = new Employee.Employee { Id = 1, FullName = "John Doe", HourlySalary = 25m };
            _company.AddEmployee(employee, DateTime.Now);

            _company.ReportHours(employee.Id, new DateTime(2024, 10, 1), 2, 30);
            _company.ReportHours(employee.Id, new DateTime(2024, 10, 2), 1, 45);

            var reports = _company.GetMonthlyReport(new DateTime(2024, 10, 1), new DateTime(2024, 10, 31));

            reports.Should().HaveCount(1);
            reports[0].EmployeeId.Should().Be(1);
            reports[0].Salary.Should().Be((2.5m + 1.75m) * 25m);
        }

    }
}