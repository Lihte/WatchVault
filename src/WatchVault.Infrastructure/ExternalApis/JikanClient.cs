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

    

}

public class JikanAnimeSearchResponse
{
    
}