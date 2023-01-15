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
        return AppDb.Blogs.FindById(blog.Id);
    }

    public Post CreatePost(string content) {
        var post = new Post(content);
        AppDb.Posts.Add(post);
        AppDb.SaveChanges();
        return AppDb.Posts.FindById(post.Id);
    }

    public Blog GetBlogById(string id) {
        var blog = AppDb.Blogs
            .Include(b => b.Post)
            .FirstOrDefault(b => b.Id == id);
        
        return blog!;
    }

    public Post GetPostById(string id) {
        var post = AppDb.Posts.FindById(id);
        return post;
    }

    public Blog AssociateBlogWithPost(string blogId, string postId) {
        var blog = AppDb.Blogs.FindById(blogId);
        var post = AppDb.Posts.FindById(postId);

        blog.Post = post;
        
        AppDb.SaveChanges();
        return AppDb.Blogs.FindById(blogId);
    }
}