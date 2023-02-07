using HumanResourceAPI.Contexts;
using HumanResourceAPI.Models;

namespace HumanResourceAPI.Repositories.Data
{
    public class RoleRepository : GeneralRepository<AppDbContext, Role, int>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
