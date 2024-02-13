using Microsoft.AspNetCore.Mvc;
using SeniorPomidor;
using SeniorPomidor.Core.Services.TradingService;
using Timer = System.Timers.Timer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
const string asyncTag = "async";

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/async/when-all", async (HttpContext httpContext, [FromServices] ITradingService tradingService) =>
    {
        var timer = new Timer();
        timer.Start();
        var requestAborted = httpContext.RequestAborted;
        var tasks = new List<Task<object>>
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
    .WithName("GetInvestDataWhenAll")
    .WithOpenApi()
    .WithTags(asyncTag);

app.MapGet("/async/when-any", async (HttpContext httpContext, [FromServices] ITradingService tradingService) =>
    {
        var timer = new Timer();
        timer.Start();
        var requestAborted = httpContext.RequestAborted;
        var tasks = new List<Task<object>>
        {
            Task.Run(() => tradingService.GetUserAccountsAsync(requestAborted)),
            Task.Run(() => tradingService.GetTariffDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetUserInfoAsync(requestAborted))
        };

        while (tasks.Any(t => t.Status < TaskStatus.RanToCompletion))
        {
            var completedTask = await Task.WhenAny(tasks);
            Console.WriteLine($"----- Milliseconds elapsed: {timer.Interval}");
            Console.WriteLine($"----- Result: {completedTask.Result}");
            tasks.Remove(completedTask);
        }
        timer.Stop();
        Console.WriteLine("----- All tasks are completed");
    })
    .WithName("GetInvestDataWhenAny")
    .WithOpenApi()
    .WithTags(asyncTag);

app.MapGet("/async/task-completion-source", async (HttpContext httpContext,
        [FromServices] ITradingService tradingService) =>
    {
        var completionSource = new TaskCompletionSource<object>();

    })
    .WithName("GetInvestDataWithCompletionSource")
    .WithOpenApi()
    .WithTags(asyncTag);

app.MapGet("/async/semaphore-slim", async (HttpContext httpContext,
        [FromKeyedServices("SemaphoredTradingService")] ITradingService tradingService) =>
    {
        var timer = new Timer();
        timer.Start();
        var requestAborted = httpContext.RequestAborted;
        var tasks = new List<Task<object>>
        {
            Task.Run(() => tradingService.GetUserAccountsAsync(requestAborted)),
            Task.Run(() => tradingService.GetTariffDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetOperationsDataAsync(requestAborted)),
            Task.Run(() => tradingService.GetUserInfoAsync(requestAborted))
        };

        var allData = Task.WhenAll(tasks);
    })
    .WithName("GetInvestDataWithSemaphoreSlim")
    .WithOpenApi()
    .WithTags(asyncTag);

app.Run();
