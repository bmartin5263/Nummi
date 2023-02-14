using KSUID;
using Microsoft.EntityFrameworkCore;

namespace Nummi.Core.Domain.Test; 

[PrimaryKey("Id")]
public class Post {
    public Ksuid Id { get; }
    public string Content { get; set; }
    public Metadata Meta { get; set; }

    public Post(string content) {
        Id = Ksuid.Generate();
        Content = content;
        Meta = new Metadata(DateTime.UtcNow, "Johonas", 69);
    }
}