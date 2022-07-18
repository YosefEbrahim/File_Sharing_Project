using File_Sharing_App.Areas.Admin.Models;

namespace File_Sharing_App.Areas.Admin.Services
{
    public interface IContactUS
    {
        IQueryable<ContactUSViewModel> GetAll();
        Task ChangeStatusAysnc(int Id, bool IsClosed);
    }
}
