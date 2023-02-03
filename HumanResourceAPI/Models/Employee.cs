using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceAPI.Models
{
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("ManagerEmployee")]
        public string? Manager_Id { get; set; }
        public Employee ManagerEmployee { get; set; }
        public List<Employee> EmployeeManager { get; set; }
        public Account Account { get; set; }
        public int? Departments_Id { get; set; }
        [ForeignKey("Departments_Id")]
        public Department Department { get; set; }
        [InverseProperty("Manager")]
        public Department Manager { get; set; }
    }

    public enum Gender
    {
        Male, Female
    }
}
