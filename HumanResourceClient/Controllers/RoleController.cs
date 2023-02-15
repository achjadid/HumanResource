using Microsoft.AspNetCore.Mvc;

namespace HumanResourceClient.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
