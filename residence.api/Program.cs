
using Microsoft.EntityFrameworkCore;
using residence.api.Endpoints;
using residence.application.Extensions;
using residence.infrastructure.Data;
using residence.infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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


// Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();


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

