using System.ComponentModel.DataAnnotations;

namespace File_Sharing_App.Models
{
    public class InputFile
    {
        [Required]
        public IFormFile File { get; set; }
    }
    public class InputUpload
    {
                 public string UserId {set;get;}
                 public string OriginalFileName { set;get;}
                 public string FileName   {set;get;}
                 public string ContentType {set;get;}
                 public decimal Size { set; get; }
    }
    public class UploadViewModel
    {
        public string id { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; }
        public decimal Size { get; set; }
        [Display(Name = "Content Type")]
        public string ContentType{ get; set; }
        [Display(Name = "Upload Date")]
        public DateTime UploadDate { get; set; }
        [Display(Name = "Download Count")]
        public long DownloadCount { get; set; }

        public string UserId { get; set; }
    }
}
