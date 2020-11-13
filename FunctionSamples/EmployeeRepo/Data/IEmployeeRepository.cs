using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetEmployees();
        public Employee GetEmployee(long Id);
        public bool DeleteEmployee(long Id);
        public bool UpdateEmployee(Employee emp);
    }
}
