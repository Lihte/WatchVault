using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WatchVault.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchEntryRepository _entires;
        private readonly IMediaRepository _media;

        public WatchListController(IWatchEntryRepository entires, IMediaRepository media)
        {
            _entires = entires;
            _media = media;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var entires = await _entires.GetAllAsync(ct);

            var response = entires.Select(e => new WatchEntryResponse
            {
                Id = e.Id,
                Status = e.Status,
                Rating = e.Rating,
                StartedAt = e.StartedAt,
                CompletedAt = e.CompletedAt,
                RewatchCount = e.RewatchCount,
                ProgressEpisode = e.ProgressEpisode,
                Notes = e.Notes,
                CreatedAt = e.CreatedAt,
                Media = e.Media is null ? null : new MediaSummaryResponse
                {
                    Id = e.Media.Id,
                    Title = e.Media.Title,
                    Type = e.Media.Type,
                    Status = e.Media.Status,
                    PosterPath = e.Media.PosterPath,
                    Genres = e.Media.Genres
                }
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToWatchlistRequest request, CancellationToken ct)
        {
            // Check if media item already exist in repository
            var existing = await _media.GetByExternalIdAsync(request.Source, request.ExternalId, ct);

            Media media;
            if (existing == null)
            {
                media = new Media
                {
                    Title = request.Title,
                    Type = request.Type,
                    MetadataSynced = false
                };

                media = await _media.AddAsync(media, ct);

                // Todo: publish sync job to RabbitMQ
            }
            else
            {
                media = existing;
            }

            var entry = new WatchEntry
            {
                MediaId = media.Id,
                Status = WatchStatus.Planning
            };

            var created = await _entires.AddAsync(entry, ct);

            var response = new WatchEntryResponse
            {
                Id = created.Id,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                Media = new MediaSummaryResponse
                {
                    Id = media.Id,
                    Title = media.Title,
                    Type = media.Type,
                    Status = media.Status,
                    PosterPath = media.PosterPath,
                    Genres = media.Genres
                }
            };

            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, response);
        }
    }
}

public record AddToWatchlistRequest(
    string Title,
    MediaType Type,
    string Source,
    string ExternalId);