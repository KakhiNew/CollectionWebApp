using CollectionWebApp.Data.Models;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace CollectionWebApp.Models.PostViewModel
{
    public class CreateVm
    {
        [Required, Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }
        public Post Post { get; set; }
    }
}
