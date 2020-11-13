using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data.FileSystem
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public Employee GetEmployeeAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetEmployeesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
