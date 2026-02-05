using Microsoft.Extensions.DependencyInjection;
using residence.application.Interfaces;
using residence.application.Services;

namespace residence.application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register all application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IResidenceService, ResidenceService>();
        services.AddScoped<IHouseService, HouseService>();
        services.AddScoped<IResidentService, ResidentService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IIncidentService, IncidentService>();
        services.AddScoped<IPostService, PostService>();

        return services;
    }
}

