using System.ComponentModel.DataAnnotations;

namespace File_Sharing_App.Areas.Admin.Models
{
    public class SearchViewModel
    {
        [Required]
        [MinLength(3)]
        public string Term { get; set; }
    }
}
