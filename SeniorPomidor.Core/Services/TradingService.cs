using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services;

public sealed class TradingService(InvestApiClient investApiClient) : ITradingService
{
    private readonly Random _random = new(DateTime.UtcNow.Millisecond);
    private void SleepRandom() => Thread.Sleep(_random.Next(1000, 3000));

    public async Task<object> GetUserAccountsAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();
        
        return accountsInfo.Accounts.Where(a => a.Status is AccountStatus.Open).ToArray();
    }
    public async Task<object> GetTariffDataAsync(CancellationToken cancellationToken)
    {
        var userTariffInfo = await investApiClient.Users.GetUserTariffAsync(new GetUserTariffRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();

        return userTariffInfo;
    }

    public async Task<object> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var userInfo = await investApiClient.Users.GetInfoAsync(new GetInfoRequest()
            , cancellationToken: cancellationToken);
        SleepRandom();

        return userInfo;
    }
    
    public async Task<object> GetOperationsDataAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest(), 
            cancellationToken: cancellationToken);
        SleepRandom();

        var result = new List<object>();
        foreach (var account in accountsInfo.Accounts)
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
