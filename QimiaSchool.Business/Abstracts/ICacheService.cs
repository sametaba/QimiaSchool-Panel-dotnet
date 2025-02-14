using System;
using System.Threading;
using System.Threading.Tasks;

namespace QimiaSchool.Business.Abstracts
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expirationDate = null, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
