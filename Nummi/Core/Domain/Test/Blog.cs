using System.ComponentModel.DataAnnotations.Schema;
using KSUID;
using Microsoft.EntityFrameworkCore;

namespace Nummi.Core.Domain.Test; 

[PrimaryKey("Id")]
public class Blog {
    public string Id { get; }
    public string Name { get; set; }
    
    [ForeignKey("PostId")]
    public Post? Post { get; set; }

    public Blog(string name) {
        Id = Ksuid.Generate().ToString();
        Name = name;
    }
}