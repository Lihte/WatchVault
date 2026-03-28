using System.Net;
using System.Net.Http.Json;

namespace WatchVault.Tests;

public class WatchlistEndpointTests : IClassFixture<WatchVaultApiFactory>
{

    private readonly HttpClient _client;
    public WatchlistEndpointTests(WatchVaultApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostWAtchlist_WithValidRequest_Returns201()
    {
        var request = new AddToWatchlistRequest(
            Title: "The Boondock Saints",
            Type: MediaType.Movie,
            Source: "tmdb",
            ExternalId: "1"
        );

        var response = await _client.PostAsJsonAsync("/api/watchlist", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetWatchlist_AfterAdding_ReturnsEntry()
    {
        var request = new AddToWatchlistRequest(
            Title: "The Boondock Saints",
            Type: MediaType.Movie,
            Source: "tmdb",
            ExternalId: "129"
        );

        await _client.PostAsJsonAsync("/api/watchlist", request);
        var response = await _client.GetAsync("/api/watchlist");
        var entries = await response.Content
            .ReadFromJsonAsync<List<WatchEntryResponse>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(entries!, e => e.Media!.Title == "The Boondock Saints");
    }
}
