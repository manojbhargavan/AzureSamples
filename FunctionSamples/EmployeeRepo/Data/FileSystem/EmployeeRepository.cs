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
        private List<Employee> Employees { get; set; }

        public EmployeeRepository()
        {
            string employeeJsonString = ReadFile();
            Employees = JsonConvert.DeserializeObject<List<Employee>>(employeeJsonString);
        }

        public Employee GetEmployee(long Id)
        {
            return Employees.Where(e => e.Id == Id)?.FirstOrDefault();
        }

        public List<Employee> GetEmployees()
        {
            return Employees;
        }

        public bool DeleteEmployee(long Id)
        {
            Employees = Employees.Where(e => e.Id != Id).ToList();
            UpdateFile();
            return true;
        }

        public bool UpdateEmployee(Employee employee)
        {
            var empUpdate = Employees.Where(e => e.Id == employee.Id)?.First();
            if (empUpdate != null)
            {
                empUpdate = employee;
                UpdateFile();
                return true;
            }
            return false;
        }

        private string ReadFile()
        {
            return File.ReadAllText(_employeeFileName);
        }

        private void UpdateFile()
        {
            File.WriteAllText(_employeeFileName, JsonConvert.SerializeObject(Employees));
        }
    }
}
