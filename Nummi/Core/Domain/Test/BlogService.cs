using Microsoft.EntityFrameworkCore;
using Nummi.Core.Database.EFCore;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Domain.Test; 

public class BlogService {
    
    private EFCoreContext AppDb { get; }

    public BlogService(EFCoreContext appDb) {
        AppDb = appDb;
    }

    public Blog CreateBlog(string name) {
        var blog = new Blog(name);
        AppDb.Blogs.Add(blog);
        AppDb.SaveChanges();
        return AppDb.Blogs.GetById(blog.Id);
    }

    public Post CreatePost(string content) {
        var post = new Post(content);
        AppDb.Posts.Add(post);
        AppDb.SaveChanges();
        return AppDb.Posts.GetById(post.Id);
    }


    public Post UpdatePost(string id, int num) {
        var post = AppDb.Posts.GetById(id);
        post.Meta.FavoriteNumber = num;
        AppDb.SaveChanges();
        return AppDb.Posts.GetById(post.Id);
    }

    public Blog GetBlogById(string id) {
        var blog = AppDb.Blogs
            .Include(b => b.Post)
            .FirstOrDefault(b => b.Id == Ksuid.FromString(id));
        
        return blog!;
    }


    public Blog UpdateBlogPostText(string blogId, string text) {
        var blog = AppDb.Blogs
            .Include(b => b.Post)
            .FirstOrDefault(b => b.Id == Ksuid.FromString(blogId));

        blog!.Post!.Content = text;
        AppDb.SaveChanges();
        
        return blog;
    }

    public Post GetPostById(string id) {
        var post = AppDb.Posts.GetById(id);
        return post;
    }

    public Blog AssociateBlogWithPost(string blogId, string postId) {
        var blog = AppDb.Blogs.GetById(blogId);
        var post = AppDb.Posts.GetById(postId);

        blog.Post = post;
        
        AppDb.SaveChanges();
        return AppDb.Blogs.GetById(blogId);
    }
}