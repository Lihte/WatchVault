using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WatchVault.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly TmdbClient _tmdb;

        public MediaController(TmdbClient tmdb)
        {
            _tmdb = tmdb;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] MediaType? type, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(q))
                return BadRequest("Query parameter 'q' is required:");

            if(type == MediaType.Anime)
            {
                // WIP Jikan
            }

            var results = await _tmdb.SearchAsync(q, ct);
            return Ok(results);
        }
    }
}
