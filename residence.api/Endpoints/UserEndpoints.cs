using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// User management endpoints
    /// </summary>
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/users")
                .WithTags("Users")
                .WithOpenApi();

            group.MapPost("/", CreateUser)
                .WithName("CreateUser")
                .WithSummary("Create a new user");

            group.MapGet("/{id}", GetUser)
                .WithName("GetUser")
                .WithSummary("Get user by ID");

            group.MapPut("/{id}", UpdateUser)
                .WithName("UpdateUser")
                .WithSummary("Update user profile information");

            group.MapDelete("/{id}", DeleteUser)
                .WithName("DeleteUser")
                .WithSummary("Delete user account");

            group.MapGet("/residence/{residenceId}", GetUsersByResidence)
                .WithName("GetUsersByResidence")
                .WithSummary("Get all users in residence");
        }

        private static async Task<IResult> CreateUser(IUserService service, Guid residenceId, CreateUserDto dto)
        {
            var result = await service.CreateUserAsync(residenceId, dto);
            return Results.Created($"/api/residences/users/{result.Id}", result);
        }

        private static async Task<IResult> GetUser(IUserService service, Guid id)
        {
            var result = await service.GetUserByIdAsync(id);
            return Results.Ok(result);
        }

        private static async Task<IResult> UpdateUser(IUserService service, Guid id, UpdateUserDto dto)
        {
            var result = await service.UpdateUserAsync(id, dto);
            return Results.Ok(result);
        }

        private static async Task<IResult> DeleteUser(IUserService service, Guid id)
        {
            await service.DeleteUserAsync(id);
            return Results.NoContent();
        }

        private static async Task<IResult> GetUsersByResidence(IUserService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            var result = await service.GetUsersByResidenceAsync(residenceId, pagination);
            return Results.Ok(result);
        }
    }
}
