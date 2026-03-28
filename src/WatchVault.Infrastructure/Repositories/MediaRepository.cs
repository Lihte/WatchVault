
using Microsoft.EntityFrameworkCore;

public class MediaRepository : IMediaRepository
{
    private readonly WatchVaultDbContext _context;

    public MediaRepository(WatchVaultDbContext context)
    {
        _context = context;
    }

    public async Task<Media?> AddAsync(Media media, CancellationToken ct = default)
    {
        _context.Media.Add(media);
        await _context.SaveChangesAsync(ct);
        return media;
    }

    public async Task<Media?> GetByExternalIdAsync(string source, string externalId, CancellationToken ct = default)
        => await _context.Media
            .Include(m => m.ExternalIds)
            .FirstOrDefaultAsync(m => m.ExternalIds
                .Any(e => e.Source == source && e.ExternalIdValue == externalId), ct);


    public async Task<Media?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Media
            .Include(m => m.ExternalIds)
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task UpdateAsync(Media media, CancellationToken ct = default)
    {
        _context.Media.Update(media);
        await _context.SaveChangesAsync(ct);
    }
}