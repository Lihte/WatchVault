using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

public class TmdbClient
{
    private readonly HttpClient _http;
    private readonly IDatabase _redis;
    private const int CacheTtlMinutes = 60;

    public TmdbClient(HttpClient http, IConnectionMultiplexer redis)
    {
        _http = http;
        _redis = redis.GetDatabase();
    }

    public async Task<TmdbMultiSearchResponse?> SearchAsync(string query, CancellationToken ct = default)
    {
        var cacheKey = $"tmdb:search:{Uri.EscapeDataString(query)}";
        var cached = await _redis.StringGetAsync(cacheKey);

        if (cached.HasValue)
            return JsonSerializer.Deserialize<TmdbMultiSearchResponse>(cached);

        var response = await _http.GetAsync($"search/multi?query={Uri.EscapeDataString(query)}&include_adult=false", ct);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct);

        await _redis.StringSetAsync(cacheKey, json, TimeSpan.FromMinutes(CacheTtlMinutes));

        return JsonSerializer.Deserialize<TmdbMultiSearchResponse>(json);
    }
}


public class TmdbMultiSearchResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("results")]
    public List<TmdbMultiSearchResult> Results { get; set; } = [];

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}

public class TmdbMultiSearchResult
{
    // --- Common fields ---
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("media_type")]
    public string MediaType { get; set; } = string.Empty; // "movie", "tv", or "person"

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("genre_ids")]
    public List<int>? GenreIds { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    // --- Movie-specific fields ---
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("video")]
    public bool? Video { get; set; }

    // --- TV-specific fields ---
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }

    [JsonPropertyName("origin_country")]
    public List<string>? OriginCountry { get; set; }
}