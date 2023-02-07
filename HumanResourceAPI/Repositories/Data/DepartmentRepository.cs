using HumanResourceAPI.Contexts;
using HumanResourceAPI.Models;

namespace HumanResourceAPI.Repositories.Data
{
    public class DepartmentRepository : GeneralRepository<AppDbContext, Department, int>
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
