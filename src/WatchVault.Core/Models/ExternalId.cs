public class ExternalId
{
    public int Id { get; set; }
    public int MediaId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalIdValue { get; set; } = string.Empty;

    public Media Media { get; set; } = null!;

}

public static class ExternalSources
{
    public const string Tmdb = "tmdb";
    public const string Jikan = "jikan";
}