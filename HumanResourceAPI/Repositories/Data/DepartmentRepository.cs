using Dapper;
using HumanResourceAPI.Contexts;
using HumanResourceAPI.Models;
using HumanResourceAPI.VirtualModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HumanResourceAPI.Repositories.Data
{
    public class DepartmentRepository : GeneralRepository<AppDbContext, Department, int>
    {
        public IConfiguration _configuration;
        private readonly AppDbContext context;
        public DepartmentRepository(IConfiguration configuration, AppDbContext context) : base(context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public async Task<IEnumerable<DepartmentVM>> GetDepartment()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                var spGetEmployee = "SP_DepartmentsGetAll";
                var res = await connection.QueryAsync<DepartmentVM>(spGetEmployee, commandType: CommandType.StoredProcedure);
                return res;
            }
        }


    }
}
