using System;
using System.IO;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Big12MemoryApp.Application.Configuration;
using Big12MemoryApp.Application.Services;
using Big12MemoryApp.Domain.Repositories;
using Big12MemoryApp.Infrastructure.Configuration;
using Big12MemoryApp.Infrastructure.Configuration.Jobs;
using Big12MemoryApp.Infrastructure.Persistence;
using Big12MemoryApp.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Big12MemoryApp API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
        // Description = "Enter 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.Configure<GmailConfig>(
    builder.Configuration.GetSection(GmailConfig.GmailOptionKey));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services
    .AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(o => o.Key.Length >= 32, "JWT Key must be at least 32 characters")
    .ValidateOnStart();

Console.WriteLine($"ENV: {builder.Environment.EnvironmentName}");
Console.WriteLine($"JWT KEY: '{builder.Configuration["Jwt:Key"]}'");

var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtIssuer = jwtSection.GetValue<string>("Issuer");
var jwtAudience = jwtSection.GetValue<string>("Audience");
var jwtKey = jwtSection.GetValue<string>("Key");

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT Key is missing");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });


builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IMemoryAttachmentRepository, MemoryAttachmentRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


builder.Services.Configure<B2StorageOptions>(
    builder.Configuration.GetSection("B2Storage"));

// ✅ AWS S3 CLIENT (B2 S3-COMPATIBLE)
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var options = sp.GetRequiredService<IOptions<B2StorageOptions>>().Value;

    var config = new AmazonS3Config
    {
        ServiceURL = options.ServiceUrl,
        ForcePathStyle = true
    };

    var client = new AmazonS3Client(
        options.AccessKey,
        options.SecretKey,
        config
    );
    PutObjectRequest request = new PutObjectRequest();
    var stream = new MemoryStream();

    request.InputStream = stream;
    request.BucketName = options.BucketName;
    request.Key = "data/";
    request.CannedACL = S3CannedACL.PublicRead;

    client.PutObjectAsync(request);

    return client;
});

builder.Services.AddScoped<IFileStorageService, B2StorageService>();


builder.Services.AddScoped<B2StorageService>();
builder.Services.AddScoped<MemoryService>();
builder.Services.AddScoped<IMemoryRepository, MemoryRepository>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});
builder.Services.AddHostedService<TimerService>();
builder.Services.AddScoped<MailJob>();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(
            serviceName: "Big12MemoryApp.API",
            serviceVersion: "1.0.0"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.EnrichWithIDbCommand = (activity, command) =>
                {
                    var stateDisplayName = $"{command.CommandType} main";
                    activity.DisplayName = stateDisplayName;
                    activity.SetTag("db.name", stateDisplayName);
                };
            })
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(otlp =>
            {
                // Aspire Dashboard OTLP gRPC
                otlp.Endpoint = new Uri("http://localhost:18889");
            });
    });
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yeni API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.MapControllers();
app.Run();