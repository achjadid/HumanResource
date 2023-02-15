using Dapper;
using HumanResourceAPI.Contexts;
using HumanResourceAPI.Models;
using HumanResourceAPI.VirtualModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Dapper.SqlMapper;

namespace HumanResourceAPI.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<AppDbContext, Employee, string>
    {
        public IConfiguration _configuration;
        private readonly AppDbContext context;
        public EmployeeRepository(IConfiguration configuration, AppDbContext context) : base(context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public async Task<IEnumerable<AccountEmployeeVM>> GetEmployeeAccount()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                var spGetEmployee = "SP_EmployeesAccountGetAll";
                var res = await connection.QueryAsync<AccountEmployeeVM>(spGetEmployee, commandType: CommandType.StoredProcedure);

                var spGetEmplyeeRole = "SP_AccountRolesGetByEmployee";
                foreach (var employee in res)
                {
                    parameters.Add("@NIK", employee.NIK);
                    employee.Role = (List<Role>?) await connection.QueryAsync<Role>(spGetEmplyeeRole, parameters, commandType: CommandType.StoredProcedure);
                }
                return res;
            }
        }

        public async Task<AccountEmployeeVM> GetEmployeeAccountByNIK(string NIK)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                var spName = "SP_EmployeesAccountGetByNIK";
                parameters.Add("@NIK", NIK);
                var res = await connection.QuerySingleOrDefaultAsync<AccountEmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);

                var spGetEmplyeeRole = "SP_AccountRolesGetByEmployee";
                parameters.Add("@NIK", res.NIK);
                res.Role = (List<Role>?) await connection.QueryAsync<Role>(spGetEmplyeeRole, parameters, commandType: CommandType.StoredProcedure);

                return res;
            }
        }

        public async Task<int> InsertEmployeeAccount(AccountEmployeeVM entity)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                string generatedNIK = GenerateNIK();
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password);

                var spName = "SP_EmployeesCreate";
                parameters.Add("@NIK", generatedNIK);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@FirstName", entity.FirstName);
                parameters.Add("@LastName", entity.LastName);
                parameters.Add("@Phone", entity.Phone);
                parameters.Add("@BirthDate", entity.BirthDate);
                parameters.Add("@Email", entity.Email);
                parameters.Add("@Salary", entity.Salary);
                parameters.Add("@Gender", entity.Gender);
                parameters.Add("@Manager_Id", entity.Manager_Id);
                parameters.Add("@Department_Id", entity.Department_Id);
                parameters.Add("@Role_Id", 3);
                int insert = await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        public async Task<int> UpdateEmployeeAccount(AccountEmployeeVM entity)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                var spName = "SP_EmployeesUpdate";
                parameters.Add("@NIK", entity.NIK);
                string password = entity.Password;
                if(password != null)
                {
                    password = BCrypt.Net.BCrypt.HashPassword(password);
                }
                parameters.Add("@Password", password);
                parameters.Add("@FirstName", entity.FirstName);
                parameters.Add("@LastName", entity.LastName);
                parameters.Add("@Phone", entity.Phone);
                parameters.Add("@BirthDate", entity.BirthDate);
                parameters.Add("@Email", entity.Email);
                parameters.Add("@Salary", entity.Salary);
                parameters.Add("@Gender", entity.Gender);
                parameters.Add("@Manager_Id", entity.Manager_Id);
                parameters.Add("@Department_Id", entity.Department_Id);
                int insert = await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        public async Task<int> Delete(string NIK)
        {
            var findKey = await context.Employees.FindAsync(NIK);
            if (findKey != null)
            {
                context.Remove(findKey);
                return context.SaveChanges();
            }
            return 404;
        }

        private string GenerateNIK()
        {
            var lastId = context.Accounts.FromSqlRaw(
                "SELECT TOP 1 * " +
                "FROM Accounts " +
                "WHERE len(NIK) = 12 " +
                "ORDER BY RIGHT(NIK, 4) desc"
                ).ToList();
            int highestId = 0;
            if (lastId.Any())
            {
                var newId = lastId[0].NIK;
                newId = newId.Substring(newId.Length - 4);
                highestId = Convert.ToInt32(newId);
            }

            int increamentId = highestId + 1;
            string generatedNIK = increamentId.ToString().PadLeft(4, '0');
            DateTime today = DateTime.Today;
            var dateNow = today.ToString("yyyyddMM");
            generatedNIK = dateNow + generatedNIK;

            return generatedNIK;
        }
    }
}
