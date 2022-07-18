using AutoMapper;
using File_Sharing_App.Areas.Admin.Models;
using File_Sharing_App.Data;
using File_Sharing_App.Models;

namespace File_Sharing_App.AutoMapper
{
    public class UploadsProfile:Profile
    {
        public UploadsProfile()
        {
            CreateMap<Uploads,UploadViewModel>();
            CreateMap<InputUpload, Uploads>()
                .ForMember(u => u.id, op => op.Ignore())
                .ForMember(u => u.UploadDate, op => op.Ignore());
        }
    }
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, CurrentUserViewModel>()
                .ForMember(u=>u.HasPassword,op=>op.MapFrom(u=>u.PasswordHash != null));
            CreateMap<ApplicationUser, AdminUserViewModel>()
                .ForMember(u => u.UserId, op => op.MapFrom(u => u.Id));

        }
    }
    public class ContactUSProfile : Profile
    {
        public ContactUSProfile()
        {
            CreateMap<Contact, ContactUSViewModel>();
        }
    }
}
