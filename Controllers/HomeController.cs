using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserServices _userServices;

        public HomeController(UserServices userServices)
        { 
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userServices.GetUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var listRoles = Common.LIST_ROLE;
            ViewBag.ListRoles = listRoles.Select(x => new SelectListItem()
            {
                Text = x,
                Value = x
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                await _userServices.CreateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userServices.GetUserAsync(id);
            var listRoles = Common.LIST_ROLE;
            ViewBag.ListRoles = listRoles.Select(x => new SelectListItem()
            {
                Text = x,
                Value = x,
                Selected = x == user.Role
            });
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, User user)
        {
            try
            {
                await _userServices.UpdateUserAsync(id, user);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View();
            }
            
        }
        public async Task<IActionResult> ChangeStatus(string id)
        {
            try
            {
                await _userServices.ChangeUserStatus(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}