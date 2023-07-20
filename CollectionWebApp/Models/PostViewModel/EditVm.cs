using CollectionWebApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.Models.PostViewModel
{
    public class EditVm
    {
        [Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }
        public Post Post { get; set; }
    }
}
