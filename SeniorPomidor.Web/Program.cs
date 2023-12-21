using Microsoft.AspNetCore.Mvc;
using SeniorPomidor.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile("appsettings.Local.json");
builder.Services.AddTradingStuff(builder.Configuration);

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
