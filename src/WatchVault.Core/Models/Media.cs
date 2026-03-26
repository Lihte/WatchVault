public class Media
{
    public int Id { get; set; }
    public MediaType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public MediaStatus Status { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? PosterPath { get; set; }
    public List<string> Genres { get; set; } = new();
    public bool MetadataSynced { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ExternalId> ExternalIds { get; set; } = new List<ExternalId>();
}

public enum MediaType { Movie, TvShow, Anime, Cartoon }
public enum MediaStatus { Unknown, Rumored, Planned, Ended, Airing, Released, InProduction, Cancelled }