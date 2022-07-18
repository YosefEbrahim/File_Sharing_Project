using Microsoft.AspNetCore.Identity;
using System;

namespace File_Sharing_App.Data
{
    public class Uploads
    {
        public Uploads()
        {
                id=Guid.NewGuid().ToString();
                UploadDate=DateTime.Now;    
        }
        public string id { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public decimal Size { get; set; }
        public string UserId { get; set; }
        public DateTime UploadDate { get; set; }

        public ApplicationUser User { get; set; }
        public long DownloadCount { get; set; }
    }
}
