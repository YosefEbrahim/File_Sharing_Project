using File_Sharing_App.Models;

namespace File_Sharing_App.Repository
{
    public interface IUploadService
    {
        IQueryable<UploadViewModel> GetUploads();
        IQueryable<UploadViewModel> GetbyId(string userId);
        IQueryable<UploadViewModel> Search(string term);
        Task CreateAsync(InputUpload input);
        Task<UploadViewModel> FindAsync(string id);
        Task<UploadViewModel> FindAsync(string id,string UserId);
        Task DeleteAsync(string id,string UserId);
        Task IncrementDownloadCount(string id);
        Task<int> GetUploadsCount();
    }
}
