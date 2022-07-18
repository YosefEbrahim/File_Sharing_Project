using AutoMapper;
using AutoMapper.QueryableExtensions;
using File_Sharing_App.Data;
using File_Sharing_App.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Sharing_App.Repository
{
    public class UploadService : IUploadService
    {
        private readonly ApplicationDbcontext _db;
        private readonly IMapper _mapper;

        public UploadService(ApplicationDbcontext db,IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }
        public async Task CreateAsync(InputUpload model)
        {
           var objmap= _mapper.Map<Uploads>(model);
            await _db.Uploads.AddAsync(objmap);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id, string UserId)
        {
            var selectedUpload = await _db.Uploads.FirstOrDefaultAsync(u => u.id == id && u.UserId == UserId);
            if (selectedUpload != null)
            {
                _db.Uploads.Remove(selectedUpload);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<UploadViewModel> FindAsync(string id, string UserId)
        {
            var selectedUpload = await _db.Uploads.FirstOrDefaultAsync(u => u.id == id && u.UserId == UserId);
            if (selectedUpload != null)
            {
                return _mapper.Map<UploadViewModel>(selectedUpload);
            }
            return null;
        }

        public async Task<UploadViewModel> FindAsync(string id)
        {
            var selectedUpload = await _db.Uploads.FindAsync(id);
            if (selectedUpload != null)
            {

                return _mapper.Map<UploadViewModel>(selectedUpload);
                /*return new UploadViewModel
                {
                    id = selectedUpload.id,
                    FileName = selectedUpload.FileName,
                    Size = selectedUpload.Size,
                    ContentType = selectedUpload.ContentType,
                    UploadDate = selectedUpload.UploadDate,
                    OriginalFileName = selectedUpload.OriginalFileName,
                    DownloadCount = selectedUpload.DownloadCount,
                };*/
            }
            return null;
        }

        public IQueryable<UploadViewModel> GetbyId(string userId)
        {
            var result = _db.Uploads
                                .OrderByDescending(n => n.DownloadCount)
                                .Where(u => u.UserId == userId)
                                .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);   
                                /*.Select(u => new UploadViewModel
                                {

                                    FileName = u.FileName,
                                    Size = u.Size,
                                    ContentType = u.ContentType,
                                    UploadDate = u.UploadDate,
                                    OriginalFileName = u.OriginalFileName,
                                    DownloadCount = u.DownloadCount,
                                });*/
            return result;
        }

        public IQueryable<UploadViewModel> GetUploads()
        {
            var result = _db.Uploads
                                .OrderByDescending(n => n.DownloadCount)
                                .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public async Task<int> GetUploadsCount()
        {

               return await _db.Uploads.CountAsync();
    
        }

        public async Task IncrementDownloadCount(string id)
        {
            var selectedUpload = await _db.Uploads.FindAsync(id);
            if (selectedUpload != null)
            {
                selectedUpload.DownloadCount++;
                _db.Update(selectedUpload);
                await _db.SaveChangesAsync();
            }
        }

        public IQueryable<UploadViewModel> Search(string term)
        {
            var result = _db.Uploads.Where(u => u.OriginalFileName.Contains(term))
                                .OrderByDescending(n => n.DownloadCount)
                                .ProjectTo<UploadViewModel>(_mapper.ConfigurationProvider);
            return result;
        }
    }
}
