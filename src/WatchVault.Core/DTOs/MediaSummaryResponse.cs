public class MediaSummaryResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public MediaStatus Status { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? PosterPath { get; set; }
    public List<string> Genres { get; set; } = new();
    public bool MetadataSynced { get; set; }
    public List<ExternalIdResponse> ExternalIds { get; set; } = new();
    // Note: NO WatchEntries collection here — that's what breaks the cycle
}