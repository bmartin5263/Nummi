namespace Nummi.Core.Domain.Test; 

public class Metadata {
    public DateTime CreatedAt { get; init; }
    public string AuthorName { get; init; } = default!;
    public int FavoriteNumber { get; set; }

    private Metadata() {}

    public Metadata(DateTime createdAt, string authorName, int favoriteNumber) {
        CreatedAt = createdAt;
        AuthorName = authorName;
        FavoriteNumber = favoriteNumber;
    }
}