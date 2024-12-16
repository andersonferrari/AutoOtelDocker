using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(r =>
//        r.AddService("servicename", "1.0"));

builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        t.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyAppTraceResouce2"));
        if (builder.Environment.IsDevelopment())
        {
            t.AddConsoleExporter();
            //other dev defaults
        }
        t.AddAspNetCoreInstrumentation();
        t.AddOtlpExporter();
        
    });

builder.Services.AddOpenTelemetry()
    .WithLogging(l =>
    {
        l.AddConsoleExporter();
        l.AddOtlpExporter();
    });
//    .WithMetrics()
//    .WithTracing();
// t => t //SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AppTraceResource", "1.0"))
//.AddAspNetCoreInstrumentation()
//.AddHttpClientInstrumentation()
//.SetSampler(new AlwaysOnSampler())
//.AddConsoleExporter()
//.AddOtlpExporter()
//);

//TODO: Test!!
//builder.Services.Configure<OtlpExporterOptions>(
//    builder.Configuration.GetSection("OpenTelemetry:Otlp"));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
