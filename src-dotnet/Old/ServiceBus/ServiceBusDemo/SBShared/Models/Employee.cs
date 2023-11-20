using LoremNET;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SBShared.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Title { get; set; }

        public Employee()
        {
            Id = Lorem.Integer(1, 10000);
            FirstName = Lorem.Words(1, true);
            LastName = Lorem.Words(1, true);
            Title = Lorem.Words(1, true);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
