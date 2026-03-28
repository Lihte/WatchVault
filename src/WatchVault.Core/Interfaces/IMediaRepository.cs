
public interface IMediaRepository
{
    Task<Media?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Media?> GetByExternalIdAsync(string source, string externalId, CancellationToken ct = default);
    Task<Media?> AddAsync(Media media, CancellationToken ct = default);
    Task UpdateAsync(Media media, CancellationToken ct = default);
}

