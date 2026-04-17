using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using residence.application.Repositories;
using residence.infrastructure.Data;
using residence.infrastructure.Repositories;

namespace residence.infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IResidenceRepository, ResidenceRepository>();

        // Register specific repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHouseRepository, HouseRepository>();
        services.AddScoped<IUserHouseRepository, UserHouseRepository>();
        services.AddScoped<IResidentRepository, ResidentRepository>();
        services.AddScoped<IResidenceSettingRepository, ResidenceSettingRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IRappelRepository, RappelRepository>();
        services.AddScoped<ITarifRepository, TarifRepository>();
        services.AddScoped<ITarifHistoryRepository, TarifHistoryRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IExpenseImageRepository, ExpenseImageRepository>();
        services.AddScoped<IIncidentRepository, IncidentRepository>();
        services.AddScoped<IIncidentCommentRepository, IncidentCommentRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostLikeRepository, PostLikeRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();

        return services;
    }
}
