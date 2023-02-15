using HumanResourceAPI.Base;
using HumanResourceAPI.Models;
using HumanResourceAPI.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HumanResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department, DepartmentRepository, int>
    {
        private readonly DepartmentRepository repository;

        public DepartmentsController(DepartmentRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            var get = await repository.GetDepartment();
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }
    }
}
