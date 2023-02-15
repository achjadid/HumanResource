using Microsoft.AspNetCore.Mvc;

namespace HumanResourceClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
