
using Microsoft.EntityFrameworkCore;
using residence.api.Endpoints;
using residence.application.Extensions;
using residence.infrastructure.Data;
using residence.infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add infrastructure and application services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Apply migrations and seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // Apply pending migrations
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();

// Map health check endpoint
app.MapHealthChecks("/health");


app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Map endpoints
app.MapAuthEndpoints();
app.MapResidenceEndpointsCustom();
app.MapHouseEndpoints();
app.MapResidentEndpoints();
app.MapPaymentEndpoints();
app.MapExpenseEndpoints();
app.MapIncidentEndpoints();
app.MapPostEndpoints();
app.UseRouting();

app.Run();

