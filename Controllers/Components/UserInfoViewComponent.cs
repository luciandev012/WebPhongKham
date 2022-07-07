using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly UserServices _userService;

        public UserInfoViewComponent(UserServices userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var user = await _userService.GetUserAsync(userId);
            return View("Default", user);

        }
    }
}
