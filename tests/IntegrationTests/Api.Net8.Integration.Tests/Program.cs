using _7xLabs.Kafka;
using _7xLabs.OpenTelemetry.Tracing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _ = OpenTelemetryTracingExtensions.CreateTracerProvider(builder =>
{
    builder.AddKafkaInstrumentation();
}, true, true);

builder.Services.AddKafkaProducer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
