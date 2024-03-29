﻿using HumanResourceAPI.Models;

namespace HumanResourceAPI.VirtualModels
{
    public class UserTokenVM
    {
        public string? Token { get; set; }
        public string? NIK { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<Role>? Role { get; set; }
        public int? Department_Id { get; set; }
        public string? DepartmentName { get; set; }
    }
}
