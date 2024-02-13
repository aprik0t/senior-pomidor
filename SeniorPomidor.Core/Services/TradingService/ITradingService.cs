using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services.TradingService;

public interface ITradingService
{
    Task<object> GetTariffDataAsync(CancellationToken cancellationToken = default);
    Task<object> GetOperationsDataAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken = default);
    Task<object> GetOperationsDataAsync(CancellationToken cancellationToken = default);
    Task<object> GetUserInfoAsync(CancellationToken cancellationToken = default);
    Task<object> GetUserAccountsAsync(CancellationToken cancellationToken = default);
}