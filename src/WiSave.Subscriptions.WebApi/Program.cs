using Scalar.AspNetCore;
using WiSave.Shared.MassTransit;
using WiSave.Subscriptions.MassTransit;
using WiSave.Subscriptions.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();

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

var rabbitMqConfiguration = builder.Configuration
    .GetSection("Messaging:Subscriptions")
    .Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();

builder.Services.AddMessaging<ISubscriptionBus>(rabbitMqConfiguration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference((options, httpContext) =>
    {
        options
            .WithTitle("WiSave Subscriptions API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
    app.UseCors("AllowSwaggerUI");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints();

app.Run();
