using Microsoft.AspNetCore.Mvc;
using SeniorPomidor;
using SeniorPomidor.Core;
using Timer = System.Timers.Timer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/async-when-all", async (HttpContext httpContext, [FromServices] ITradingService tradingService) =>
    {
        var timer = new Timer();
        timer.Start();
        var requestAborted = httpContext.RequestAborted;
        var tasks = new List<Task<dynamic>>
        {
            Task.Run(() => tradingService.GetUserAccountsAsync(requestAborted)),
            Task.Run(() => tradingService.GetTariffDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetUserInfoAsync(requestAborted))
        };

        var results = await Task.WhenAll(tasks);
        timer.Stop();
        Console.WriteLine($"----- Milliseconds elapsed: {timer.Interval}");
        foreach (var result in results)
        {
            Console.WriteLine($"---- Result: {result}");
        }
    })
    .WithName("GetInvestData")
    .WithOpenApi();

app.Run();
