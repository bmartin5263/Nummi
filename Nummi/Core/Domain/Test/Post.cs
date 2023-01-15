using KSUID;
using Microsoft.EntityFrameworkCore;

namespace Nummi.Core.Domain.Test; 

[PrimaryKey("Id")]
public class Post {
    public string Id { get; }
    public string Content { get; set; }

    public Post(string content) {
        Id = Ksuid.Generate().ToString();
        Content = content;
    }
}