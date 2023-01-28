using System.Net;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database;

namespace Nummi.Core.Domain.Test; 

public class BlogService {
    
    private AppDb AppDb { get; }

    public BlogService(AppDb appDb) {
        AppDb = appDb;
    }

    public Blog CreateBlog(string name) {
        var blog = new Blog(name);
        AppDb.Blogs.Add(blog);
        AppDb.SaveChanges();
        return AppDb.Blogs.GetById(blog.Id, HttpStatusCode.BadRequest);
    }

    public Post CreatePost(string content) {
        var post = new Post(content);
        AppDb.Posts.Add(post);
        AppDb.SaveChanges();
        return AppDb.Posts.GetById(post.Id, HttpStatusCode.BadRequest);
    }


    public Post UpdatePost(string id, int num) {
        var post = AppDb.Posts.GetById(id, HttpStatusCode.BadRequest);
        post.Meta.FavoriteNumber = num;
        AppDb.SaveChanges();
        return AppDb.Posts.GetById(post.Id, HttpStatusCode.BadRequest);
    }

    public Blog GetBlogById(string id) {
        var blog = AppDb.Blogs
            .Include(b => b.Post)
            .FirstOrDefault(b => b.Id == id);
        
        return blog!;
    }


    public Blog UpdateBlogPostText(string blogId, string text) {
        var blog = AppDb.Blogs
            .Include(b => b.Post)
            .FirstOrDefault(b => b.Id == blogId);

        blog!.Post!.Content = text;
        AppDb.SaveChanges();
        
        return blog;
    }

    public Post GetPostById(string id) {
        var post = AppDb.Posts.GetById(id, HttpStatusCode.BadRequest);
        return post;
    }

    public Blog AssociateBlogWithPost(string blogId, string postId) {
        var blog = AppDb.Blogs.GetById(blogId, HttpStatusCode.BadRequest);
        var post = AppDb.Posts.GetById(postId, HttpStatusCode.BadRequest);

        blog.Post = post;
        
        AppDb.SaveChanges();
        return AppDb.Blogs.GetById(blogId, HttpStatusCode.InternalServerError);
    }
}