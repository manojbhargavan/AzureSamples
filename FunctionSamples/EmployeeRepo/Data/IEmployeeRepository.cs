using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data
{
    public interface IEmployeeRepository
    {
        public List<Employees> GetEmployees();
        public Employees GetEmployee(long Id);
    }
}
