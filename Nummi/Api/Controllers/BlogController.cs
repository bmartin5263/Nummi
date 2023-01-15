using Microsoft.AspNetCore.Mvc;
using Nummi.Api.Model;
using Nummi.Core.Domain.Test;

namespace Nummi.Api.Controllers; 

[Route("api")]
[ApiController]
public class BlogController : ControllerBase {

    private BlogService BlogService { get; }

    public BlogController(BlogService blogService) {
        BlogService = blogService;
    }

    [Route("/blog")]
    [HttpPost]
    public BlogDto CreateBlog(string name) {
        return BlogService
            .CreateBlog(name)
            .ToDto();
    }

    [Route("/post")]
    [HttpPost]
    public PostDto CreatePost(string content) {
        return BlogService
            .CreatePost(content)
            .ToDto();
    }
    
    [Route("/blog/{id}")]
    [HttpGet]
    public BlogDto GetBlogById(string id) {
        return BlogService
            .GetBlogById(id)
            .ToDto();
    }

    [Route("/post/{id}")]
    [HttpGet]
    public PostDto GetPostById(string id) {
        return BlogService
            .GetPostById(id)
            .ToDto();
    }

    [Route("/blog")]
    [HttpPatch]
    public BlogDto AssociateBlogWithPost(string blogId, string postId) {
        return BlogService
            .AssociateBlogWithPost(blogId, postId)
            .ToDto();
    }

    [Route("/blog/post")]
    [HttpPatch]
    public BlogDto UpdateBlogPostText(string blogId, string text) {
        return BlogService
            .UpdateBlogPostText(blogId, text)
            .ToDto();
    }
    
}