using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services.TradingService;

internal class TradingService(InvestApiClient investApiClient) : ITradingService
{
    private readonly Random _random = new(DateTime.UtcNow.Millisecond);
    private void SleepRandom() => Thread.Sleep(_random.Next(1000, 3000));

    public virtual async Task<object> GetUserAccountsAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();
        
        return accountsInfo.Accounts.Where(a => a.Status is AccountStatus.Open).ToArray();
    }

    public virtual async Task<object> GetTariffDataAsync(CancellationToken cancellationToken)
    {
        var userTariffInfo = await investApiClient.Users.GetUserTariffAsync(new GetUserTariffRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();

        return userTariffInfo;
    }

    public virtual async Task<object> GetOperationsDataAsync(CancellationToken cancellationToken = default)
    {
        var accounts = await GetUserAccountsAsync(cancellationToken) as Account[];
        return await GetOperationsDataAsync(accounts!, cancellationToken);
    }

    public virtual async Task<object> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var userInfo = await investApiClient.Users.GetInfoAsync(new GetInfoRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();

        return userInfo;
    }
    
    public virtual async Task<object> GetOperationsDataAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken)
    {
        var result = new List<object>();
        foreach (var account in accounts)
        {
            var now = DateTime.UtcNow;
            var operationsInfo = await investApiClient.Operations.GetOperationsAsync(
                new OperationsRequest
                {
                    AccountId = account.Id,
                    From = Timestamp.FromDateTime(now.AddYears(-10)),
                    To = Timestamp.FromDateTime(now),
                }
                , cancellationToken: cancellationToken);
            SleepRandom();
            result.Add(operationsInfo);
        }

        return result.ToArray();
    }
}
