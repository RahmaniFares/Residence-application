using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Authentication endpoints
    /// </summary>
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/auth")
                .WithTags("Authentication")
                .WithOpenApi();

            group.MapPost("/login", Login)
                .WithName("Login")
                .WithSummary("User login")
                .Produces<AuthResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPost("/register", Register)
                .WithName("Register")
                .WithSummary("User registration")
                .Produces<AuthResponseDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPost("/refresh", RefreshToken)
                .WithName("RefreshToken")
                .WithSummary("Refresh JWT token")
                .Produces<AuthResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> Login(IAuthService service, LoginDto dto)
        {
            try
            {
                var result = await service.LoginAsync(dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> Register(IAuthService service, CreateUserDto dto, Guid residenceId)
        {
            try
            {
                var result = await service.RegisterAsync(dto, residenceId);
                return Results.Created($"/api/users/{result.User.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> RefreshToken(IAuthService service, RefreshTokenDto dto)
        {
            try
            {
                var result = await service.RefreshTokenAsync(dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
