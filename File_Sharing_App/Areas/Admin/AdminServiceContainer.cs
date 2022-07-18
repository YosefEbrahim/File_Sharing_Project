using ClosedXML.Excel;
using File_Sharing_App.Areas.Admin.Services;

namespace File_Sharing_App.Areas.Admin
{
    public static class AdminServiceContainer
    {
        public static IServiceCollection AddAdminToSC(this IServiceCollection Services)
        {
            Services.AddTransient<IUserService, UserService>();
            Services.AddTransient<IXLWorkbook, XLWorkbook>();
            Services.AddTransient<IContactUS, ContactUS>();
            return Services;

        }
    }
}
