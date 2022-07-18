namespace File_Sharing_App.Areas.Admin.Models
{
    public class AdminUserViewModel { 


        public string UserId { get; set; }
        public string FristName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
