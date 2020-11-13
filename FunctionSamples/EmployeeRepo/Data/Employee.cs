using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepo.Data
{
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string About { get; set; }
        public Guid Token { get; set; }
        public object Country { get; set; }
        public string Location { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public DateTimeOffset? Dob { get; set; }
        public long Gender { get; set; }
        public long UserType { get; set; }
        public long UserStatus { get; set; }
        public string ProfilePicture { get; set; }
        public string CoverPicture { get; set; }
        public bool Enablefollowme { get; set; }
        public bool Sendmenotifications { get; set; }
        public bool SendTextmessages { get; set; }
        public bool Enabletagging { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public double Livelng { get; set; }
        public double Livelat { get; set; }
        public string LiveLocation { get; set; }
        public long CreditBalance { get; set; }
        public long MyCash { get; set; }
    }
}
