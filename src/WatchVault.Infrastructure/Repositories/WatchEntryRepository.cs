
using Microsoft.EntityFrameworkCore;

public class WatchEntryRepository : IWatchEntryRepository
{
    private readonly WatchVaultDbContext _context;

    public WatchEntryRepository(WatchVaultDbContext context)
    {
        _context = context;
    }

    public async Task<WatchEntry?> AddAsync(WatchEntry entry, CancellationToken ct = default)
    {
        _context.WatchEntries.Add(entry);
        await _context.SaveChangesAsync();
        return entry;
    }

    public async Task<IEnumerable<WatchEntry>> GetAllAsync(CancellationToken ct = default)
         => await _context.WatchEntries
            .Include(e => e.Media)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(ct);


    public async Task<WatchEntry?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.WatchEntries
            .Include(e => e.Media)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task UpdateAsync(WatchEntry entry, CancellationToken ct = default)
    {
        _context.WatchEntries.Update(entry);
        await _context.SaveChangesAsync(ct);
    }
}