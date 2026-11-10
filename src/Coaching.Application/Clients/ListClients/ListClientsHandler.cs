using Coaching.Domain;

namespace Coaching.Application.Clients;

public sealed class ListClientsHandler(IStore store, ICairoTime time)
{
    public async Task<Page<AccountResponse>> Handle(int page, int size, ClientStatus? status, string? search, CancellationToken ct)
    {
        var now = time.UtcNow;
        var q = store.Query<ClientProfile>();
        if (status == ClientStatus.Expired)
            q = q.Where(x => x.Status == ClientStatus.Active && x.AccessEndsAt <= now);
        else if (status == ClientStatus.Active)
            q = q.Where(x => x.Status == ClientStatus.Active && x.AccessEndsAt > now);
        else if (status is not null)
            q = q.Where(x => x.Status == status);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(x => x.FullName.Contains(search) || x.Email.Contains(search));
        var total = await store.Count(q, ct);
        var items = await store.List(q.OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size), ct);
        return new(items.Select(x => x.Response(now)).ToList(), total, page, size);
    }
}
