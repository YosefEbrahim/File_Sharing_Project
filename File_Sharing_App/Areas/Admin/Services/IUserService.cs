using File_Sharing_App.Areas.Admin.Models;

namespace File_Sharing_App.Areas.Admin.Services
{
    public interface IUserService
    {
        IQueryable<AdminUserViewModel> GetAll();
        IQueryable<AdminUserViewModel> GetBlockedUsers();
        IQueryable<AdminUserViewModel> Search(string term);
        Task<OperationResult> ToggeleBlockUserAsync(string UserId);
        Task<int> GetUserRegisterationCountAsync();
        Task<int> GetUserRegisterationCountAsync(int Month);
        Task IntializeAsync();
    }
}
