using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .ConfigureResource( r => 
        r.AddService("servicename", "1.0"))
    .WithTracing( t => t //SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AppTraceResource", "1.0"))
        //.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .SetSampler(new AlwaysOnSampler())
        //.AddConsoleExporter()
        //.AddOtlpExporter()
    );

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
