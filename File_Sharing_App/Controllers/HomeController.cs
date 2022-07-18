using File_Sharing_App.Data;
using File_Sharing_App.Helper.Mail;
using File_Sharing_App.Hubs;
using File_Sharing_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace File_Sharing_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbcontext _db;
        private readonly IMailHelper _mailHelper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger
            ,ApplicationDbcontext db
            ,IMailHelper mailHelper
            ,IHubContext<NotificationHub> hubContext
            ,UserManager<ApplicationUser> userManager
            )
        {
            _logger = logger;
            this._db = db;
            this._mailHelper = mailHelper;
            this._hubContext = hubContext;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var highestDownloads = _db.Uploads.OrderByDescending(u => u.DownloadCount)
                .Select(u => new UploadViewModel
                { 
                FileName = u.FileName,
                Size = u.Size,
                ContentType = u.ContentType,
                UploadDate = u.UploadDate,
                OriginalFileName = u.OriginalFileName,
                DownloadCount = u.DownloadCount,
            })
                .Take(3);
            ViewBag.Popular = highestDownloads;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    _db.Contact.Add(new Contact
                    {
                        Email = model.Email,
                        Message = model.Message,
                        Name = model.Name,
                        Subject = model.Subject,
                        UserId = UserId
                    });
                    await _db.SaveChangesAsync();
                    TempData["Message"] = "Message has been sent successfully!.";
                    //Send Mail
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<h1>File Sharing - Unread Message</h1>");
                    sb.AppendFormat("Name : {0} \n", model.Name);
                    sb.AppendFormat("Email : {0} \n", model.Email);
                    sb.AppendLine();
                    sb.AppendFormat("Subject : {0} \n", model.Subject);
                    sb.AppendFormat("Message : {0} \n", model.Message);

                    _mailHelper.SendMailToAdmin(new InputMailMessage
                    {
                        Subject = model.Subject,
                        Email = model.Email,
                        Body = sb.ToString()
                    });
                    var adminUsers =await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
                    var adminIds = adminUsers.Select(n => n.Id);
                    if(adminIds.Any())
                    {
                   await _hubContext.Clients.Users(adminIds).SendAsync("RecievedNotification", "You have unread contact us message");
                    }
                    return RedirectToAction("Contact");
                }
                TempData["Error"] = "You must Login First";
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult About()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SetCulture(string lang,string? returnUrl)
        {
            if(!string.IsNullOrEmpty(lang))
            {
                Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index"); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}