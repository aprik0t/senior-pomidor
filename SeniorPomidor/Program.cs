using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", ([FromServices] IHttpContextAccessor accessor) =>
    {
        return "Hello world";
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

static void ThreadProc(object? state)
{
    Console.WriteLine("TheadProc callback relays hi");
    if (state is not null)
    {
        Console.WriteLine(state.GetType());
    }
    else
    {
        Console.WriteLine("state is null");
    }
}
