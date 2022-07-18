using File_Sharing_App.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace File_Sharing_App.Areas.Admin.Controllers
{
    public class ContactController : AdminBaseController
    {
        private readonly IContactUS _contact;

        public ContactController(IContactUS contact)
        {
            this._contact = contact;
        }
        public async Task<IActionResult> Index()
        {
            var result=await _contact.GetAll().ToListAsync();
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int Id,bool IsClosed)
        {
            await  _contact.ChangeStatusAysnc(Id, IsClosed);
            return RedirectToAction("Index");
        }
    }
}
