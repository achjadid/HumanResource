using Dapper;
using HumanResourceAPI.Contexts;
using HumanResourceAPI.Models;
using HumanResourceAPI.Repositories.Interface;
using HumanResourceAPI.VirtualModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HumanResourceAPI.Repositories.Data
{
    public class AccountRepository : GeneralRepository<AppDbContext, Account, string>
    {
        public IConfiguration _configuration;
        private readonly AppDbContext context;
        public AccountRepository(IConfiguration configuration, AppDbContext context) : base(context)
        {
            _configuration = configuration;
            this.context = context;
        }

        DynamicParameters parameters = new DynamicParameters();

        public async Task<UserTokenVM> Login(LoginVM loginVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                var spCheckPassword = "SP_AccountsCheckPassword";
                parameters.Add("@Email", loginVM.Email);
                var userData = await connection.QueryFirstOrDefaultAsync<AccountEmployeeVM>(spCheckPassword, parameters, commandType: CommandType.StoredProcedure);
                if (userData == null)
                {
                    return null;
                }

                bool verified = BCrypt.Net.BCrypt.Verify(loginVM.Password, userData.Password);
                if (!verified)
                {
                    return null;
                }

                var userToken = new UserTokenVM();
                userToken.NIK = userData.NIK;
                userToken.Email = userData.Email;
                userToken.FirstName = userData.FirstName;
                userToken.LastName = userData.LastName;
                userToken.Department_Id = userData.Department_Id;
                userToken.DepartmentName = userData.DepartmentName;

                var spGetEmplyeeRole = "SP_AccountRolesGetByEmployee";
                parameters = new DynamicParameters();
                parameters.Add("@NIK", userData.NIK);
                userToken.Role = (List<Role>?) await connection.QueryAsync<Role>(spGetEmplyeeRole, parameters, commandType: CommandType.StoredProcedure);

                //parameters = new DynamicParameters();
                //var spUserToken = "SP_AccountsTokenData";
                //parameters.Add("@NIK", userData.NIK);
                //var userToken = await connection.QueryFirstOrDefaultAsync<UserTokenVM>(spUserToken, parameters, commandType: CommandType.StoredProcedure);
                //if (userToken == null)
                //{
                //    return null;
                //}

                string token = GenerateJwtToken(userToken);
                userToken.Token = token;

                return userToken;
            }
        }

        public async Task<int> InsertAccountEmployee(AccountEmployeeVM entity)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:HRAPI"]))
            {
                string generatedNIK = GenerateNIK();
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password);

                var spName = "SP_AccountsCreate";
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

        private string GenerateJwtToken(UserTokenVM userToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, userToken.Username),
                //new Claim(ClaimTypes.NameIdentifier, userToken.NIK),
                //new Claim(ClaimTypes.Name, userToken.Name),
                new Claim("NIK", userToken.NIK),
                new Claim("Name", userToken.FirstName + " " + userToken.LastName),
                new Claim("Email", userToken.Email),
                new Claim("Role", JsonSerializer.Serialize(userToken.Role)),
                //new Claim("RoleId", userToken.RoleId.ToString()),
                //new Claim("RoleName", userToken.RoleName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
