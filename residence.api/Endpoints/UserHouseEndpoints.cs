using residence.application.DTOs;
using residence.application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace residence.api.Endpoints
{
    /// <summary>
    /// User-House relationship management endpoints
    /// </summary>
    public static class UserHouseEndpoints
    {
        public static void MapUserHouseEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/user-houses")
                .WithTags("User-Houses")
                .WithOpenApi();

            // Assign user to house
            group.MapPost("/", AssignUserToHouse)
                .WithName("AssignUserToHouse")
                .WithSummary("Assign a user to a house");

            // Get user's houses
            group.MapGet("/users/{userId}", GetUserHouses)
                .WithName("GetUserHouses")
                .WithSummary("Get all houses for a user");

            // Get house's users
            group.MapGet("/houses/{houseId}", GetHouseUsers)
                .WithName("GetHouseUsers")
                .WithSummary("Get all users assigned to a house");

            // Update user-house relationship
            group.MapPut("/users/{userId}/houses/{houseId}", UpdateUserHouse)
                .WithName("UpdateUserHouse")
                .WithSummary("Update user-house relationship");

            // Remove user from house
            group.MapDelete("/users/{userId}/houses/{houseId}", RemoveUserFromHouse)
                .WithName("RemoveUserFromHouse")
                .WithSummary("Remove a user from a house");

            // Check if user is assigned to house
            group.MapGet("/users/{userId}/houses/{houseId}/check", CheckUserAssignment)
                .WithName("CheckUserAssignment")
                .WithSummary("Check if user is assigned to house");

            // Get user-house details
            group.MapGet("/users/{userId}/houses/{houseId}", GetUserHouseDetails)
                .WithName("GetUserHouseDetails")
                .WithSummary("Get user-house relationship details");
        }

        private static async Task<IResult> AssignUserToHouse(
            IUserHouseService service,
            Guid residenceId,
            CreateUserHouseDto dto)
        {
            try
            {
                var result = await service.AssignUserToHouseAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/user-houses/users/{dto.UserId}/houses/{dto.HouseId}", result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetUserHouses(
            IUserHouseService service,
            Guid residenceId,
            Guid userId)
        {
            try
            {
                var result = await service.GetUserHousesAsync(userId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetHouseUsers(
            IUserHouseService service,
            Guid residenceId,
            Guid houseId)
        {
            try
            {
                var result = await service.GetHouseUsersAsync(houseId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> UpdateUserHouse(
            IUserHouseService service,
            Guid residenceId,
            Guid userId,
            Guid houseId,
            UpdateUserHouseDto dto)
        {
            try
            {
                var result = await service.UpdateUserHouseAsync(residenceId, userId, houseId, dto);
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> RemoveUserFromHouse(
            IUserHouseService service,
            Guid residenceId,
            Guid userId,
            Guid houseId)
        {
            try
            {
                var result = await service.RemoveUserFromHouseAsync(residenceId, userId, houseId);
                if (!result)
                    return Results.NotFound();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> CheckUserAssignment(
            IUserHouseService service,
            Guid residenceId,
            Guid userId,
            Guid houseId)
        {
            try
            {
                var result = await service.IsUserAssignedToHouseAsync(userId, houseId);
                return Results.Ok(new { isAssigned = result });
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetUserHouseDetails(
            IUserHouseService service,
            Guid residenceId,
            Guid userId,
            Guid houseId)
        {
            try
            {
                var result = await service.GetUserHouseDetailsAsync(userId, houseId);
                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
