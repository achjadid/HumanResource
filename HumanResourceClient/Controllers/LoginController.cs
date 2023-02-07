using Microsoft.AspNetCore.Mvc;

namespace HumanResourceClient.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
