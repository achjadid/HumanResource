using HumanResourceAPI.Repositories.Data;
using HumanResourceAPI.VirtualModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HumanResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository repository;

        public EmployeesController(EmployeeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var get = await repository.GetEmployeeAccount();
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.NotFound, message = get.Count() + " Data Ditemukan", Data = get });
            }
        }

        [HttpGet("{NIK}")]
        public virtual async Task<IActionResult> GetByNIK(string NIK)
        {
            var get = await repository.GetEmployeeAccountByNIK(NIK);
            if (get != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Ditemukan", Data = get });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountEmployeeVM accountEmployeeVM)
        {
            var insert = await repository.InsertEmployeeAccount(accountEmployeeVM);
            if (insert >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dimasukkan", Data = insert });
            }

            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memasukkan Data", Data = insert });
        }

        [HttpPut]
        public async Task<IActionResult> Update(AccountEmployeeVM accountEmployeeVM)
        {
            var update = await repository.UpdateEmployeeAccount(accountEmployeeVM);
            if (update >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dimasukkan", Data = update });
            }

            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Memasukkan Data", Data = update });
        }

        [HttpDelete("{NIK}")]
        public async Task<IActionResult> Delete(string NIK)
        {
            var delete = await repository.Delete(NIK);
            if (delete >= 1)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data Berhasil Dihapus", Data = delete });
            }
            else if (delete == 0)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data dengan NIK " + NIK + "Tidak Ditemukan", Data = delete });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Terjadi Kesalahan", Data = delete });
            }
        }
    }
}
