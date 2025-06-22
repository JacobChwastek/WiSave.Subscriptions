using Scalar.AspNetCore;
using WiSave.Shared.MassTransit;
using WiSave.Subscriptions.Contracts.Queries;
using WiSave.Subscriptions.MassTransit;
using WiSave.Subscriptions.WebApi.Endpoints;
using WiSave.Subscriptions.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSwaggerUI", policy =>
    {
        policy
            .WithOrigins("http://localhost:5050", "https://localhost:5051")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5050", 
                "https://localhost:5051",
                "http://localhost:4200",
                "http://localhost:4201",
                "http://localhost:3000",
                "https://localhost:4200",
                "https://localhost:4201"
            )
            .AllowAnyMethod()  
            .AllowAnyHeader()          
            .AllowCredentials();          
    });
    
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var rabbitMqConfiguration = builder.Configuration
    .GetSection("Messaging:Subscriptions")
    .Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();

builder.Services.AddMessaging<ISubscriptionBus>(rabbitMqConfiguration, configureMassTransit: x =>
{
    x.AddRequestClient<GetSubscriptionsQuery>();
    x.AddRequestClient<GetSubscriptionQuery>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference((options, _) =>
    {
        options
            .WithTitle("WiSave Subscriptions API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
    
    app.UseCors("DevelopmentCors");
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapSubscriptionEndpoints();
app.MapHealthEndpoints();

app.Run();
