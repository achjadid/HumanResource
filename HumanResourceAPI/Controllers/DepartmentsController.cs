using HumanResourceAPI.Base;
using HumanResourceAPI.Models;
using HumanResourceAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department, DepartmentRepository, int>
    {
        public DepartmentsController(DepartmentRepository repository) : base(repository)
        {

        }
    }
}
