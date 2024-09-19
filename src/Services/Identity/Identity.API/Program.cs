var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMessageBroker(builder.Configuration, assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();
app.MapGroup("/api")
    .AddEndpointFilter<StandardResponseFilter>()
    .MapCarter();

app.UseExceptionHandler(options => { });
app.Run();