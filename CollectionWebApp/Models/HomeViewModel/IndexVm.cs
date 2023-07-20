using CollectionWebApp.Data.Models;
using PagedList.Core;

namespace CollectionWebApp.Models.HomeViewModel
{
    public class IndexVm
    {
        public IPagedList<Post> Posts { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
    }
}
