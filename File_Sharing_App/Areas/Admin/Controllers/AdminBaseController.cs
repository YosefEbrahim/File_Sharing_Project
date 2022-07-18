using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace File_Sharing_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =UserRoles.Admin)]
    public class AdminBaseController : Controller
    {
    }
}
