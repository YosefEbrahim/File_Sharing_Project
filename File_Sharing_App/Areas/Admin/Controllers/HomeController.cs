using Microsoft.AspNetCore.Mvc;

namespace File_Sharing_App.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
