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
        public virtual ActionResult Login(LoginVM loginVM)
        {
            var insert = repository.Login(loginVM);
            if (insert != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Berhasil Login", Data = insert });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Login, username atau password salah.", Data = insert });
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
