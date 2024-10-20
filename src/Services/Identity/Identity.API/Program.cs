using Identity.API;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();