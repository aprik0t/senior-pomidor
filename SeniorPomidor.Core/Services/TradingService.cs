using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace SeniorPomidor.Core.Services;

public sealed class TradingService(InvestApiClient investApiClient) : ITradingService
{
    public object GetMarginDataAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = investApiClient.Users.GetAccounts(new GetAccountsRequest());

        var result = new List<object>();
        foreach (var account in accountsInfo.Accounts)
        {
            var marginAttributes = investApiClient.Users.GetMarginAttributes(
                new GetMarginAttributesRequest
                {
                    AccountId = account.Id
                });
            result.Add(marginAttributes);
        }

        return result.ToArray();
    }
    
    public async Task<object> GetTariffsDataAsync(CancellationToken cancellationToken)
    {
        var accountsInfo = await investApiClient.Users.GetAccountsAsync(new GetAccountsRequest(), 
            cancellationToken: cancellationToken);

        var result = new List<object>();
        foreach (var account in accountsInfo.Accounts)
        {
            var marginAttributes = await investApiClient.Users.GetUserTariffAsync(
                new GetUserTariffRequest(), cancellationToken: cancellationToken);
            result.Add(marginAttributes);
        }

        return result.ToArray();
    }
}