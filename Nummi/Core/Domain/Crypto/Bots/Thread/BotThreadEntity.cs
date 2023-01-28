using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Nummi.Core.Domain.Crypto.Bots.Thread; 

[PrimaryKey(nameof(Id))]
[Table("BotThread")]
public class BotThreadEntity {
    
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public uint Id { get; }
    
    [ForeignKey("BotId")]
    public Bot? Bot { get; set; }

    public BotThreadEntity(uint id) {
        Id = id;
    }
}