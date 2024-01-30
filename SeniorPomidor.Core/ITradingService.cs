namespace SeniorPomidor.Core;

public interface ITradingService
{
    Task<object> GetTariffDataAsync(CancellationToken cancellationToken = default);
    Task<object> GetOperationsDataAsync(CancellationToken cancellationToken = default);
    Task<object> GetUserInfoAsync(CancellationToken cancellationToken = default);
    Task<object> GetUserAccountsAsync(CancellationToken cancellationToken = default);
}