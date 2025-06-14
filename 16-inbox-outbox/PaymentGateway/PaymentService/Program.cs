using PaymentService.Database;
using PaymentService.UseCases.CancelTransaction;
using PaymentService.UseCases.ConfirmTransaction;
using PaymentService.UseCases.CreateAccount;
using PaymentService.UseCases.CreateTransaction;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using PaymentService.Common.SendNotification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentContext>((serviceProvider, options) =>
{
    options.UseNpgsql(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Default"));
});

builder.Services.AddHostedService<MigrationRunner>();

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = config.GetSection("Kafka:BootstrapServers").Value
    };
    return new ProducerBuilder<string, string>(producerConfig).Build();
});

builder.Services.AddHostedService(sp =>
{
    var producer = sp.GetRequiredService<IProducer<string, string>>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new NotificationSender(
        sp.GetRequiredService<IServiceScopeFactory>(),
        sp.GetRequiredService<ILogger<NotificationSender>>(),
        producer,
        configuration.GetSection("Kafka:Topic").Value
            ?? throw new InvalidOperationException("Kafka topic is not configured")
    );
});

builder.Services.AddScoped<ISendNotificationService, SendNotificationService>();
builder.Services.AddScoped<ICreateAccountService, CreateAccountService>();
builder.Services.AddScoped<ICreateTransactionService, CreateTransactionService>();
builder.Services.AddScoped<IConfirmTransactionService, ConfirmTransactionService>();
builder.Services.AddScoped<ICancelTransactionService, CancelTransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/v1/accounts", async (
        CreateAccountRequest request,
        ICreateAccountService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CreateAccountAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CreateAccount")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/hold", async (
        CreateTransactionRequest request,
        ICreateTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CreateTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CreateTransaction")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/commit", async (
        ConfirmTransactionRequest request,
        IConfirmTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.ConfirmTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("ConfirmTransaction")
    .WithOpenApi();

app.MapPost("/api/v1/transactions/cancel", async (
        CancelTransactionRequest request,
        ICancelTransactionService service,
        CancellationToken cancellationToken) =>
    {
        var result = await service.CancelTransactionAsync(request, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("CancelTransaction")
    .WithOpenApi();

app.Run();
