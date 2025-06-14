using Amazon.S3;
using ImageConverter.WebApi.BackgroundServices;
using ImageConverter.WebApi.Configuration;
using ImageConverter.WebApi.Database;
using ImageConverter.WebApi.Endpoints;
using ImageConverter.WebApi.UseCases.ConvertImage;
using ImageConverter.WebApi.UseCases.DownloadImage;
using ImageConverter.WebApi.UseCases.GetTaskStatus;
using ImageConverter.WebApi.UseCases.UploadImage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Сервис для выполнения миграций
builder.Services.AddHostedService<MigrationRunner>();

// Регистрация обработчиков запросов
builder.Services.AddScoped<IConvertImageHandler, ConvertImageHandler>();
builder.Services.AddScoped<IUploadImageHandler, UploadImageHandler>();
builder.Services.AddScoped<IDownloadImageHandler, DownloadImageHandler>();
builder.Services.AddScoped<IGetTaskStatusHandler, GetTaskStatusHandler>();

// Фоновый сервис для конвертации изображений
builder.Services.AddHostedService<ConversionBackgroundService>();

// Регистрация конфигурации для S3
builder.Services
    .AddOptions<S3Config>()
    .Bind(builder.Configuration.GetSection("S3"))
    .ValidateDataAnnotations();

// Регистрация клиента для S3
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var configuration = sp.GetRequiredService<IOptions<S3Config>>().Value;

    return new AmazonS3Client(
        configuration.AccessKey,
        configuration.SecretKey,
        new AmazonS3Config
        {
            ServiceURL = configuration.ServiceUrl,
            ForcePathStyle = true,
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapConversionEndpoints();

app.Run();