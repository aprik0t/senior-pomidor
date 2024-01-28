using Microsoft.AspNetCore.Mvc;
using SeniorPomidor;
using SeniorPomidor.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/async-whenall", async ([FromServices] ITradingService tradingService) =>
    {
        var tasks = new List<Task<object>>();
        tasks.Add(new Task<object>(() => tradingService.GetMarginDataAsync()));
        tasks.Add(new Task<object>(() => tradingService.GetTariffsDataAsync()));
        
        var results = await Task.WhenAll(tasks);
        foreach (var result in results)
        {
            Console.WriteLine($"Result: {result}");
        }
    })
    .WithName("GetInvestData")
    .WithOpenApi();

app.Run();
