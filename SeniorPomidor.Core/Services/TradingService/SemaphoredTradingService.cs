using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services.TradingService;

internal class SemaphoredTradingService(InvestApiClient investApiClient) : TradingService(investApiClient)
{
    private const int MaxSemaphoreCount = 3;
    private readonly SemaphoreSlim _semaphoreSlim = new(MaxSemaphoreCount, MaxSemaphoreCount);

    public override async Task<object> GetOperationsDataAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken)
    {
        return await SemaphoredAsync(async () => await base.GetOperationsDataAsync(accounts, cancellationToken), cancellationToken);
    }

    public override async Task<object> GetOperationsDataAsync(CancellationToken cancellationToken = default)
    {
        return await SemaphoredAsync(async () => await base.GetOperationsDataAsync(cancellationToken), cancellationToken);
    }

    public override async Task<object> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        return await SemaphoredAsync(async () => await base.GetUserInfoAsync(cancellationToken), cancellationToken);
    }

    public override async Task<object> GetTariffDataAsync(CancellationToken cancellationToken)
    {
        return await SemaphoredAsync(async () => await base.GetTariffDataAsync(cancellationToken), cancellationToken);
    }

    public override async Task<object> GetUserAccountsAsync(CancellationToken cancellationToken)
    {
        return await SemaphoredAsync(async () => await base.GetUserAccountsAsync(cancellationToken), cancellationToken);
    }

    private async Task<object> SemaphoredAsync(Func<Task<object>> action, CancellationToken cancellationToken)
    {
        Console.WriteLine($"--// SemaphoreCount: {_semaphoreSlim.CurrentCount}");
        await _semaphoreSlim.WaitAsync(cancellationToken);
        
        try
        {
            return action();
        }
        finally
        {
            _semaphoreSlim.Release();
            Console.WriteLine($"--// SemaphoreCount: {_semaphoreSlim.CurrentCount}");
        }
    }
}