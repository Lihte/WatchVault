using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WatchVault.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly TmdbClient _tmdb;
        private readonly JikanClient _jikan;

        public MediaController(TmdbClient tmdb, JikanClient jikan)
        {
            _tmdb = tmdb;
            _jikan = jikan;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] MediaType? type, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(q))
                return BadRequest("Query parameter 'q' is required:");

            if(type == MediaType.Anime)
            {
                var animeResults = await _jikan.SearchAsync(q, ct);
                return Ok(animeResults);
            }

            var results = await _tmdb.SearchAsync(q, ct);
            return Ok(results);
        }
    }
}
