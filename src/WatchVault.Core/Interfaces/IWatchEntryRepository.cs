public interface IWatchEntryRepository
{
    Task<IEnumerable<WatchEntry>> GetAllAsync(CancellationToken ct = default);
    Task<WatchEntry?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<WatchEntry?> AddAsync(WatchEntry entry, CancellationToken ct = default);
    Task UpdateAsync(WatchEntry entry, CancellationToken ct = default);
}