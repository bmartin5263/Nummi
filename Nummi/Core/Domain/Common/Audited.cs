namespace Nummi.Core.Domain.Common; 

// https://dotnetcoretutorials.com/2022/03/16/auto-updating-created-updated-and-deleted-timestamps-in-entity-framework/
public interface Audited {
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt != null;
}