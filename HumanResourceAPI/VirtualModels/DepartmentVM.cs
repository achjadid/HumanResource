namespace HumanResourceAPI.VirtualModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Manager_Id { get; set; }
        public string? ManagerName { get; set; }
    }
}
