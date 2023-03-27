
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using WhatsApp.Helper;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
               .WriteTo.File(
               path: "C:\\Temp\\PIZZAMTAANIAPILOGS-.txt",
               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm,ss.fff zzz}[{Level:u3}] {Message:lj}{NewLine}{Exception}",
               rollingInterval: RollingInterval.Day,
               restrictedToMinimumLevel: LogEventLevel.Information
               ).CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Whatsapp API", Version = "v1" ,Description="This API Provides endpoint for uploading contacts and adding them to whatsapp group"});

    c.OperationFilter<SwaggerFileOperationFilter>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();

app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
