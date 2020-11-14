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
        private string _originalEmployeeFileName = @".\Data\FileSystem\EmployeeData_Original.json";
        private List<Employee> Employees { get; set; }

        public EmployeeRepository()
        {
            RefreshEmployeeDataset();
        }

        private void RefreshEmployeeDataset()
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
            if (Employees.Exists(e => e.Id == employee.Id))
            {
                var empUpdate = Employees.Where(e => e.Id == employee.Id)?.First();
                if (empUpdate != null)
                {
                    var index = Employees.IndexOf(empUpdate);
                    if (index != -1)
                    {
                        Employees[index] = employee;
                        UpdateFile();
                        return true;
                    }
                }
                return false;
            }
            else
            {
                throw new InvalidDataException($"Employee with id {employee.Id} does not exits. Insert (PUT) first.");
            }

        }

        private string ReadFile()
        {
            return File.ReadAllText(_employeeFileName);
        }

        private void UpdateFile()
        {
            File.WriteAllText(_employeeFileName, JsonConvert.SerializeObject(Employees));
        }

        public bool RestoreEmployeeDataset()
        {
            File.Copy(_originalEmployeeFileName, _employeeFileName, true);
            RefreshEmployeeDataset();
            return true;
        }

        public bool InsertEmployee(Employee employee)
        {
            if (!Employees.Exists(e => e.Id == employee.Id))
            {
                Employees.Add(employee);
                UpdateFile();
                return true;
            }
            else
            {
                throw new InvalidDataException($"Employee with id {employee.Id} already exits.");
            }
        }
    }
}
