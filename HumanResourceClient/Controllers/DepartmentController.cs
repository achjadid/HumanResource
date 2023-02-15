using Microsoft.AspNetCore.Mvc;

namespace HumanResourceClient.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
