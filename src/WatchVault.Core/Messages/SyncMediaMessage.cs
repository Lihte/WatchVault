public class SyncMediaMessage
{
    public int MediaId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public MediaType Type { get; set; }
}