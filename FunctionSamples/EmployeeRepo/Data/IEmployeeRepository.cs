using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetEmployeesAsync();
        public Employee GetEmployeeAsync(long Id);
    }
}
