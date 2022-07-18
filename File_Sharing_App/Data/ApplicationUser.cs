using Microsoft.AspNetCore.Identity;

namespace File_Sharing_App.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
