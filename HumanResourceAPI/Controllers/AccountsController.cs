using HumanResourceAPI.Models;
using HumanResourceAPI.Repositories.Data;
using HumanResourceAPI.VirtualModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HumanResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository repository;

        public AccountsController(AccountRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var login = await repository.Login(loginVM);
            if (login != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Berhasil Login", Data = login });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Login, username atau password salah.", Data = login });
            }
        }

        [HttpPost("create")]
        public ActionResult Create(AccountEmployeeVM accountEmployeeVM)
        {
            var insert = repository.InsertAccountEmployee(accountEmployeeVM);
            if (insert >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dimasukkan", Data = insert });
            }

            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memasukkan Data", Data = insert });
        }

        //[HttpGet("old")]
        //public virtual ActionResult GetOld()
        //{
        //    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Ditemukan", Data = 0 });
        //}
    }
}
