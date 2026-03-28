public class WatchEntryResponse
{
    public int Id { get; set; }
    public WatchStatus Status { get; set; }
    public int? Rating { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RewatchCount { get; set; }
    public int? ProgressEpisode { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public MediaSummaryResponse? Media { get; set; }
}