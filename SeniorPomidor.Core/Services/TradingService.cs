using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services;

public sealed class TradingService(InvestApiClient investApiClient) : ITradingService
{
    public async Task<object> GetUserAccountsAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest()
            , cancellationToken: cancellationToken);
        
        return accountsInfo.Accounts.Where(a => a.Status is AccountStatus.Open).ToArray();
    }
    public async Task<object> GetTariffDataAsync(CancellationToken cancellationToken)
    {
        var userTariffInfo = await investApiClient.Users.GetUserTariffAsync(new GetUserTariffRequest()
            , cancellationToken: cancellationToken);

        return userTariffInfo;
    }

    public async Task<object> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var userInfo = await investApiClient.Users.GetInfoAsync(new GetInfoRequest()
            , cancellationToken: cancellationToken);

        return userInfo;
    }
    
    public async Task<object> GetOperationsDataAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest(), 
            cancellationToken: cancellationToken);

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
            result.Add(operationsInfo);
        }

        return result.ToArray();
    }

    public async Task<object> ReturnException(CancellationToken cancellationToken)
    {
        var stream = investApiClient.Instruments.Currencies(new InstrumentsRequest()
                , cancellationToken: cancellationToken);

        return Task.FromException(new InvalidOperationException("Let's assume that this exception was thrown by gRPC"));
    }


    public async Task<object> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
        var currenciesResponse = await investApiClient.MarketData.GetCandlesAsync(new GetCandlesRequest{  }).Instruments.CurrenciesAsync(new InstrumentsRequest());
        var currencies = currenciesResponse.Instruments.ToArray();
        
    }
    
}
