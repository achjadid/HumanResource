using HumanResourceAPI.Models;

namespace HumanResourceAPI.VirtualModels
{
    public class AccountEmployeeVM
    {
        public string? NIK { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Salary { get; set; }
        public string? Email { get; set; }
        public Gender? Gender { get; set; }
        public string? Manager_Id { get; set; }
        public string? ManagerName { get; set; }
        public int? Department_Id { get; set; }
        public string? DepartmentName { get; set; }
        public int? Role_Id { get; set; }
        public string? RoleName { get; set; }
        public List<Role>? Role { get; set; }
    }
}
