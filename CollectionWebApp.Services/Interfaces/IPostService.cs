﻿using CollectionWebApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionWebApp.Services.Interfaces
{
    public interface IPostService
    {
        Post GetPost(int postId);
        IEnumerable<Post> GetPostsByTitle(string searchString);
        IEnumerable<Post> GetPostsByUser(ApplicationUser applicationUser);
        Comment GetComment(int commentId);
        Task<Post> Add(Post post);
        Task<Comment> Add(Comment comment);
        Task<Post> Update(Post post);
        Task Delete(Post post);
        IEnumerable<Post> GetAllPosts();
    }
}
