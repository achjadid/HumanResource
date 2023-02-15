using Microsoft.AspNetCore.Mvc;

namespace HumanResourceClient.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
