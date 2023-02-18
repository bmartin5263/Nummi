using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Domain.Test; 

[PrimaryKey("Id")]
public class Blog {
    public Ksuid Id { get; }
    public string Name { get; set; }
    
    [ForeignKey("PostId")]
    public Post? Post { get; set; }

    public Blog(string name) {
        Id = Ksuid.Generate();
        Name = name;
    }
}