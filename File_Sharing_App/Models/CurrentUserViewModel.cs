using File_Sharing_App.Resources;
using System.ComponentModel.DataAnnotations;

namespace File_Sharing_App.Models
{
    public class CurrentUserViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "FristName", ResourceType = typeof(_SharedResource))]
        public string FristName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "LastName", ResourceType = typeof(_SharedResource))]
        public string LastName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]

        public string Email { get; set; }
        public bool HasPassword { get; set; }
    }
}
