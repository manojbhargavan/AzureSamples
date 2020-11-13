using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data.FileSystem
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _employeeFileName = @".\Data\FileSystem\EmployeeData.json";
        private List<Employees> Employees { get; }

        public EmployeeRepository()
        {
            string employeeJsonString = File.ReadAllText(_employeeFileName);
            Employees = JsonConvert.DeserializeObject<List<Employees>>(employeeJsonString);
        }

        public Employees GetEmployee(long Id)
        {
            return Employees.Where(e => e.Id == Id)?.FirstOrDefault();
        }

        public List<Employees> GetEmployees()
        {
            return Employees;
        }
    }
}
