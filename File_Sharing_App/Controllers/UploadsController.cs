using File_Sharing_App.Data;
using File_Sharing_App.Models;
using File_Sharing_App.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace File_Sharing_App.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment env;

        public UploadsController(IUploadService uploadService, IWebHostEnvironment env)
        {
            this._uploadService = uploadService;
            this.env = env;
        }
        private string userId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
        public IActionResult Index()
        {
            var result = _uploadService.GetbyId(userId);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(InputFile model)
        {
            if (ModelState.IsValid)
            {
                var newName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(model.File.FileName);
                var fileName = string.Concat(newName, extension);
                var root = env.WebRootPath;
                var path = Path.Combine(root, "Uploads", fileName);
                using (var fs = System.IO.File.Create(path))
                {
                    await model.File.CopyToAsync(fs);
                }
                await _uploadService.CreateAsync(new InputUpload
                {
                    UserId = userId,
                    OriginalFileName = model.File.FileName,
                    FileName = fileName,
                    ContentType = model.File.ContentType,
                    Size = model.File.Length
                });
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var selectedUpload = await _uploadService.FindAsync(id,userId);
            if (selectedUpload == null)
            {
                return NotFound();
            }
            return View(selectedUpload);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var selectedUpload = await _uploadService.FindAsync(id, userId);
            if (selectedUpload == null)
            {
                return NotFound();
            }
                await _uploadService.DeleteAsync(id, userId);
            return RedirectToAction("Index");

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Results(string term, int RequiredPage = 1)
        {
            var result = _uploadService.Search(term);
            var model = await GetPageData(result, RequiredPage);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int RequiredPage = 1)
        {
            var result = _uploadService.GetUploads();
            var model= await GetPageData(result,RequiredPage);
            return View(model);

        }
        public async Task<List<UploadViewModel>> GetPageData(IQueryable<UploadViewModel> result, int RequiredPage = 1)
        {
            const int pagesize = 5;
            decimal rowcount = await _uploadService.GetUploadsCount();
            decimal pagecount = Math.Ceiling(rowcount / pagesize);
            if (RequiredPage > pagecount)
            {
                RequiredPage = 1;
            }
            RequiredPage = RequiredPage <= 0 ? 1 : RequiredPage;
            int skipcount = (RequiredPage - 1) * pagesize;
            var model = await result.Skip(skipcount)
                                .Take(pagesize)
                                .ToListAsync(); ;
            ViewBag.CurrentPage = RequiredPage;
            ViewBag.pagecount = pagecount;
            ViewBag.pagesize = pagesize;
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var selectedFile = await _uploadService.FindAsync(id);
            if (selectedFile == null)
            {
                return NotFound();
            }
            await _uploadService.IncrementDownloadCount(id);
            var path = "~/Uploads/" + id;
            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control", "no-cache");
            return File(path, selectedFile.ContentType, selectedFile.OriginalFileName);
        }
    }
}