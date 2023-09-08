using System;
using System.Collections.Generic;
using ServerSideBasics;

namespace serverSideBasics.Data
{
    internal static class EmployeeMockData
    {
        internal static void MockEmployeeDataOutError(out employee emp1, out employee emp2, out employee emp3)
        {
            emp1 = new employee()
            {
                firstName = "Manoj",
                lastName = "test",
                department = "IT",
                location = "Hyderabad",
                id = Guid.NewGuid().ToString()
            };
            emp2 = new employee()
            {
                firstName = "Nandan",
                lastName = "test",
                department = "IT",
                location = "Hyderabad",
                id = Guid.NewGuid().ToString()
            };
            emp3 = new employee()
            {
                firstName = "ErrorKing",
                lastName = "test",
                department = "IT",
                location = "HyderabadErrorMock",
                id = Guid.NewGuid().ToString()
            };
        }

        internal static void MockEmployeeDataOutNoError(out employee emp1, out employee emp2, out employee emp3)
        {
            emp1 = new employee()
            {
                firstName = "Manoj",
                lastName = "test",
                department = "IT",
                location = "Hyderabad",
                id = Guid.NewGuid().ToString()
            };
            emp2 = new employee()
            {
                firstName = "Nandan",
                lastName = "test",
                department = "IT",
                location = "Hyderabad",
                id = Guid.NewGuid().ToString()
            };
            emp3 = new employee()
            {
                firstName = "ErrorKing",
                lastName = "test",
                department = "IT",
                location = "Hyderabad",
                id = Guid.NewGuid().ToString()
            };
        }

        internal static List<employee> MockEmployeeDataList()
        {
            return new List<employee>() {
                new employee()
                {
                    firstName = "Manoj",
                    lastName = "test",
                    department = "IT",
                    location = "Hyderabad",
                    id = Guid.NewGuid().ToString()
                },new employee()
                {
                    firstName = "Nandan",
                    lastName = "test",
                    department = "IT",
                    location = "Hyderabad",
                    id = Guid.NewGuid().ToString()
                }, new employee()
                {
                    firstName = "ErrorKing",
                    lastName = "test",
                    department = "IT",
                    location = "HyderabadErrorMock",
                    id = Guid.NewGuid().ToString()
                } };
        }
    }
}