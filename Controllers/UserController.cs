using Microsoft.AspNetCore.Mvc;

namespace WebPhongKham.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            return View();
        }
    }
}
