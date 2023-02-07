using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Manager_Id { get; set; }
        [ForeignKey("Manager_Id")]
        [JsonIgnore]
        public Employee? Manager { get; set; }
        [InverseProperty("Department")]
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
