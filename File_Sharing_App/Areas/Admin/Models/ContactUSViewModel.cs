using File_Sharing_App.Models;

namespace File_Sharing_App.Areas.Admin.Models
{
    public class ContactUSViewModel: ContactViewModel
    {
        public int Id { get; set; }
        public bool IsClosed { get; set; }
        public DateTime SentDate { get; set; }
    }
}
