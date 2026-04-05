using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

public class JikanClient
{
    private readonly HttpClient _http;
    private readonly IDatabase _redis;
    private const int CacheTtlMinutes = 60;

    public JikanClient(HttpClient http, IConnectionMultiplexer redis)
    {
        _http = http;
        _redis = redis.GetDatabase();
    }

    public async Task<AnimeSearchResponse?> SearchAsync(string query, CancellationToken ct = default)
    {
        var cacheKey = $"jikan:search:{Uri.EscapeDataString(query)}";
        var cached = await _redis.StringGetAsync(cacheKey);

        if (cached.HasValue)
            return JsonSerializer.Deserialize<AnimeSearchResponse>(cached);

        var response = await _http.GetAsync($"anime?q={Uri.EscapeDataString(query)}", ct);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct);

        await _redis.StringSetAsync(cacheKey, json, TimeSpan.FromMinutes(CacheTtlMinutes));

        return JsonSerializer.Deserialize<AnimeSearchResponse>(json);
    }

}

// public class JikanAnimeSearchResponse
// {
//     [JsonPropertyName("data")]
//     public List<Anime> Data { get; set; } = [];
// }

public class AnimeSearchResponse
{
    // [JsonPropertyName("pagination")]
    // public Pagination Pagination { get; set; } = new();

    [JsonPropertyName("data")]
    public List<Anime> Data { get; set; } = [];
}

// ── Pagination ─────────────────────────────────────────────────────────────────

public class Pagination
{
    [JsonPropertyName("last_visible_page")]
    public int LastVisiblePage { get; set; }

    [JsonPropertyName("has_next_page")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("items")]
    public PaginationItems Items { get; set; } = new();
}

public class PaginationItems
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }
}

// ── Anime ──────────────────────────────────────────────────────────────────────

public class Anime
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("images")]
    public AnimeImages Images { get; set; } = new();

    // [JsonPropertyName("trailer")]
    // public Trailer Trailer { get; set; } = new();

    [JsonPropertyName("approved")]
    public bool Approved { get; set; }

    [JsonPropertyName("titles")]
    public List<Title> Titles { get; set; } = [];

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("title_english")]
    public string? TitleEnglish { get; set; }

    [JsonPropertyName("title_japanese")]
    public string? TitleJapanese { get; set; }

    [JsonPropertyName("title_synonyms")]
    public List<string> TitleSynonyms { get; set; } = [];

    [JsonPropertyName("type")]
    public string? Type { get; set; } // TV, Movie, OVA, ONA, Special, Music

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("episodes")]
    public int? Episodes { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; } // "Finished Airing", "Currently Airing", "Not yet aired"

    [JsonPropertyName("airing")]
    public bool Airing { get; set; }

    [JsonPropertyName("aired")]
    public AiredRange Aired { get; set; } = new();

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("rating")]
    public string? Rating { get; set; } // G, PG, PG-13, R, R+, Rx

    [JsonPropertyName("score")]
    public double? Score { get; set; }

    [JsonPropertyName("scored_by")]
    public int? ScoredBy { get; set; }

    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }

    [JsonPropertyName("members")]
    public int? Members { get; set; }

    [JsonPropertyName("favorites")]
    public int? Favorites { get; set; }

    [JsonPropertyName("synopsis")]
    public string? Synopsis { get; set; }

    [JsonPropertyName("background")]
    public string? Background { get; set; }

    [JsonPropertyName("season")]
    public string? Season { get; set; } // spring, summer, fall, winter

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    // [JsonPropertyName("broadcast")]
    // public Broadcast Broadcast { get; set; } = new();

    // [JsonPropertyName("producers")]
    // public List<MalUrl> Producers { get; set; } = [];

    // [JsonPropertyName("licensors")]
    // public List<MalUrl> Licensors { get; set; } = [];

    // [JsonPropertyName("studios")]
    // public List<MalUrl> Studios { get; set; } = [];

    [JsonPropertyName("genres")]
    public List<MalUrl> Genres { get; set; } = [];

    [JsonPropertyName("explicit_genres")]
    public List<MalUrl> ExplicitGenres { get; set; } = [];

    [JsonPropertyName("themes")]
    public List<MalUrl> Themes { get; set; } = [];

    // [JsonPropertyName("demographics")]
    // public List<MalUrl> Demographics { get; set; } = [];
}

// ── Supporting types ───────────────────────────────────────────────────────────

public class AnimeImages
{
    [JsonPropertyName("jpg")]
    public ImageSet Jpg { get; set; } = new();

    [JsonPropertyName("webp")]
    public ImageSet Webp { get; set; } = new();
}

public class ImageSet
{
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("small_image_url")]
    public string? SmallImageUrl { get; set; }

    [JsonPropertyName("large_image_url")]
    public string? LargeImageUrl { get; set; }
}

// public class Trailer
// {
//     [JsonPropertyName("youtube_id")]
//     public string? YoutubeId { get; set; }

//     [JsonPropertyName("url")]
//     public string? Url { get; set; }

//     [JsonPropertyName("embed_url")]
//     public string? EmbedUrl { get; set; }

//     [JsonPropertyName("images")]
//     public TrailerImages? Images { get; set; }
// }

// public class TrailerImages
// {
//     [JsonPropertyName("image_url")]
//     public string? ImageUrl { get; set; }

//     [JsonPropertyName("small_image_url")]
//     public string? SmallImageUrl { get; set; }

//     [JsonPropertyName("medium_image_url")]
//     public string? MediumImageUrl { get; set; }

//     [JsonPropertyName("large_image_url")]
//     public string? LargeImageUrl { get; set; }

//     [JsonPropertyName("maximum_image_url")]
//     public string? MaximumImageUrl { get; set; }
// }

public class Title
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } // Default, Synonym, Japanese, English, etc.

    [JsonPropertyName("title")]
    public string? Value { get; set; }
}

public class AiredRange
{
    [JsonPropertyName("from")]
    public string? From { get; set; } // ISO8601

    [JsonPropertyName("to")]
    public string? To { get; set; } // ISO8601

    [JsonPropertyName("prop")]
    public AiredProp Prop { get; set; } = new();

    [JsonPropertyName("string")]
    public string? Display { get; set; } // Human-readable e.g. "Oct 3, 2009 to Mar 31, 2013"
}

public class AiredProp
{
    [JsonPropertyName("from")]
    public DateComponents From { get; set; } = new();

    [JsonPropertyName("to")]
    public DateComponents To { get; set; } = new();
}

public class DateComponents
{
    [JsonPropertyName("day")]
    public int? Day { get; set; }

    [JsonPropertyName("month")]
    public int? Month { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }
}

// public class Broadcast
// {
//     [JsonPropertyName("day")]
//     public string? Day { get; set; } // "Sundays"

//     [JsonPropertyName("time")]
//     public string? Time { get; set; } // "17:00"

//     [JsonPropertyName("timezone")]
//     public string? Timezone { get; set; } // "Asia/Tokyo"

//     [JsonPropertyName("string")]
//     public string? Display { get; set; } // "Sundays at 17:00 (JST)"
// }

public class MalUrl
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}