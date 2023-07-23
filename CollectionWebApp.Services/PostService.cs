using CollectionWebApp.Data;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollectionWebApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public PostService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Post? GetPost(int postId)
        {
            return applicationDbContext.Posts
                .Include(post => post.Creator)
                .Include(post => post.Comments)
                    .ThenInclude(comment => comment.Author)
                .Include(post => post.Comments)
                    .ThenInclude(comment => comment.Comments)
                .FirstOrDefault(post => post.Id == postId);
        }


        public IEnumerable<Post> GetPostsByTitle(string searchString)
        {
            return applicationDbContext.Posts
                .OrderByDescending(post => post.UpdatedOn)
                .Include(post => post.Creator)
                .Include(post => post.Comments)
                .Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));
        }

        public IEnumerable<Post> GetPostsByUser(ApplicationUser applicationUser)
        {
            return applicationDbContext.Posts
                .Include(post => post.Creator)
                .Include(post => post.Approver)
                .Include(post => post.Comments)
                .Where(post => post.Creator == applicationUser);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return applicationDbContext.Posts
                .Include(post => post.Creator)
                .Include(post => post.Approver)
                .Include(post => post.Comments);
        }

        public Comment GetComment(int commentId)
        {
            return applicationDbContext.Comments
                .Include(comment => comment.Author)
                .Include(comment => comment.Post)
                .Include(comment => comment.Parent)
                .SingleOrDefault(comment => comment.Id == commentId);
        }

        public async Task<Post> Add(Post post)
        {
            applicationDbContext.Add(post);
            await applicationDbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Comment> Add(Comment comment)
        {
            applicationDbContext.Add(comment);
            await applicationDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Post> Update(Post post)
        {
            applicationDbContext.Update(post);
            await applicationDbContext.SaveChangesAsync();
            return post;
        }

        public async Task Delete(Post post)
        {
            var comments = applicationDbContext.Comments.Where(c => c.Post.Id == post.Id);
            applicationDbContext.Comments.RemoveRange(comments);

         
            applicationDbContext.Posts.Remove(post);

            await applicationDbContext.SaveChangesAsync();

            string webRootPath = ""; 
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";
            if (File.Exists(pathToImage))
            {
                File.Delete(pathToImage);
            }
        }
    }
}
