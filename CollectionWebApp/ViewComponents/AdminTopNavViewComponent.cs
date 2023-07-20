using Microsoft.AspNetCore.Mvc;

namespace CollectionWebApp.ViewComponents
{
    public class AdminTopNavViewComponent:ViewComponent {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() => { return View(); });
        }
    }
}
