using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models.Paging;

namespace WebPhongKham.Controllers.Components
{
    public class PagingViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
