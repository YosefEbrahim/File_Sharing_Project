using File_Sharing_App.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace File_Sharing_App.Areas.Admin.Controllers
{
    public class ReportsController : AdminBaseController
    {
        private readonly IUserService _userService;

        public ReportsController(IUserService userService)
        {
            this._userService = userService;
        }
        public IActionResult Users()
        {
            var result = _userService.GetAll().ToList();
            return new ViewAsPdf(result);
        }
    }
}
