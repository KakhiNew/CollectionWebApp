using CollectionWebApp.Data.Models;

namespace CollectionWebApp.Models.AdminViewModel
{
    public class AdminDashboardViewModel
    {
        public List<Post> PublishedPosts { get; set; }
        public List<Post> UnpublishedPosts { get; set; }
       
    }
}
