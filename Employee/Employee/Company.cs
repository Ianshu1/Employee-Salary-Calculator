using Employee;
using Employee.Exceptions;

namespace Employee
{
    public class Company : ICompany
    {
        private readonly List<WorkEntry> _workEntries = new List<WorkEntry>();
        private readonly List<Employee> _employees;
        public Company(string name)
        {
            _employees = new List<Employee>();
            Name = name;
        }
        public string Name { get; }

        public Employee[] Employees => _employees.ToArray();

        public void AddEmployee(Employee employee, DateTime contractStartDate)
        {
            if (_employees.Exists(e => e.Id == employee.Id))
            {
                throw new EmployeeIdExistException(employee.Id);
            }
            if (employee.Id < 0)
            {
                throw new NegativeEmployeeIdException();
            }

            _employees.Add(employee);
        }

        public EmployeeMonthlyReport[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate)
        {
            if (periodStartDate > periodEndDate)
            {
                throw new InvalidEmployeeMonthlyReportException();
            }
            var monthlyReports = new List<EmployeeMonthlyReport>();

            foreach (var employee in _employees)
            {
                var workEntries = GetWorkEntries(employee.Id);

                var relevantEntries = workEntries
                    .Where(entry => entry.StartDateTime >= periodStartDate && entry.StartDateTime <= periodEndDate)
                    .ToList();

                if (!relevantEntries.Any())
                {
                    continue;
                }

                var totalHours = relevantEntries.Sum(entry => entry.Hours);
                var totalMinutes = relevantEntries.Sum(entry => entry.Minutes);

                totalHours += totalMinutes / 60;
                totalMinutes %= 60;

                var totalSalary = totalHours * employee.HourlySalary + (totalMinutes / 60.0m) * employee.HourlySalary;

                var report = new EmployeeMonthlyReport
                {
                    EmployeeId = employee.Id,
                    Year = periodStartDate.Year,
                    Month = periodStartDate.Month,
                    Salary = totalSalary
                };

                monthlyReports.Add(report);
            }

            return monthlyReports.ToArray();
        }

        public void RemoveEmployee(int employeeId, DateTime contractEndDate)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee != null)
            {
                _employees.Remove(employee);
            }
            else
            {
                throw new EmployeeNotFoundException(employeeId);
            }

        }

        public void ReportHours(int employeeId, DateTime dateAndTime, int hours, int minutes)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(employeeId);
            }
            if (hours < 0 || minutes < 0 || minutes >= 60)
            {
                throw new InvalidWorkTimeException();
            }

            var workEntry = new WorkEntry(employeeId, dateAndTime, hours, minutes);
            _workEntries.Add(workEntry);
        }

        public List<WorkEntry> GetWorkEntries(int employeeId)
        {
            return _workEntries.Where(e => e.EmployeeId == employeeId).ToList();
        }
    }
}
