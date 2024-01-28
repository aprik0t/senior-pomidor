namespace SeniorPomodor.Core;

public interface ITradingService
{
    object GetMarginDataAsync(CancellationToken cancellationToken = default);
    Task<object> GetTariffsDataAsync(CancellationToken cancellationToken = default);
}