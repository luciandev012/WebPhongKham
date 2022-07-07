using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class UserController : Controller
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _userServices.LoginAsync(username, password);
            if(result == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session.SetString("UserId", result.Id);
                HttpContext.Session.SetString("Role", result.Role);
                switch (result.Role)
                {
                    case "Tiep nhan": return RedirectToAction("Index", "Patient");
                    case "Ke toan": return RedirectToAction("Index", "Accountant");
                    case "Xet nghiem": return RedirectToAction("Index", "Test");
                    case "X-quang": return RedirectToAction("Index", "Xray");
                    default: return RedirectToAction("Index", "Home");
                }
            }    
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        public async Task<IActionResult> ChangePass(string id)
        {
            var user = await _userServices.GetUserAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePass(string id, string passwordOld, string passwordNew)
        {
            var res = await _userServices.ChangePassAcsync(id, passwordOld, passwordNew);
            var user = await _userServices.GetUserAsync(id);
            if (res)
            {
                return RedirectToAction("Logout"); 
            }
            else
            {
                return View(user);
            }    
        }
    }
}
