using ClosedXML.Excel;
using File_Sharing_App.Areas.Admin.Models;
using File_Sharing_App.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace File_Sharing_App.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly IXLWorkbook _workbook;

        public UsersController(IUserService userService, IXLWorkbook workbook)
        {
            this._userService = userService;
            this._workbook = workbook;
        }
        public async Task<IActionResult> Index()
        {
            var result= await _userService.GetAll().ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Blocked()
        {
            var result = await _userService.GetBlockedUsers().ToListAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
           if(ModelState.IsValid)
            {
                var result = await _userService.Search(model.Term).ToListAsync();
                return View("Index",result);
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Block(string UserId)
        {
            if(!String.IsNullOrEmpty(UserId))
            {
                var result =await _userService.ToggeleBlockUserAsync(UserId);
                if(result.Success)
                {
                    TempData["massege"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;

                }
                return RedirectToAction("Index");
            }
            TempData["Error"] = "User is not found";

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> UserCount()
        {
            var UserRegisterationCount = await _userService.GetUserRegisterationCountAsync();
            var month = DateTime.Today.Month;
            var UserRegisterationCountMonthly = await _userService.GetUserRegisterationCountAsync(month);

            return Json(new {total=UserRegisterationCount,month= UserRegisterationCountMonthly });
        }
        public async Task<IActionResult> ExportExcel()
        {
            var ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "User Data.xlsx";
            var result = await _userService.GetAll().ToListAsync();
            var worksheet = _workbook.Worksheets.Add("All Users");
                worksheet.Cell(1, 1).Value = "First Name";
                worksheet.Cell(1, 2).Value = "Last Name";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Created Date";

            for (int i = 1; i < result.Count; i++)
            {
                var row = i+1;
                worksheet.Cell(row, 1).Value =result[i-1].FristName ;
                worksheet.Cell(row, 2).Value = result[i - 1].LastName;
                worksheet.Cell(row, 3).Value = result[i - 1].Email;
                worksheet.Cell(row, 4).Value = result[i - 1].CreatedDate;
            }
            using(var ms =new MemoryStream())
            {
                _workbook.SaveAs(ms);
                return File(ms.ToArray(),ContentType,fileName);
            }
           
        }
    }
}
