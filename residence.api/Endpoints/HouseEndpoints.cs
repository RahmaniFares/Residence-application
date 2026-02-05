using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// House endpoints
    /// </summary>
    public static class HouseEndpoints
    {
        public static void MapHouseEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/houses")
                .WithTags("Houses")
                .WithOpenApi();

            group.MapPost("/", CreateHouse)
                .WithName("CreateHouse")
                .WithSummary("Create a new house");

            group.MapGet("/{id}", GetHouse)
                .WithName("GetHouse")
                .WithSummary("Get house by ID");

            group.MapGet("/{id}/details", GetHouseDetails)
                .WithName("GetHouseDetails")
                .WithSummary("Get house with residents details");

            group.MapPut("/{id}", UpdateHouse)
                .WithName("UpdateHouse")
                .WithSummary("Update house");

            group.MapDelete("/{id}", DeleteHouse)
                .WithName("DeleteHouse")
                .WithSummary("Delete house");

            group.MapGet("/", GetHousesByResidence)
                .WithName("GetHousesByResidence")
                .WithSummary("Get all houses in residence");
        }

        private static async Task<IResult> CreateHouse(IHouseService service, Guid residenceId, CreateHouseDto dto)
        {
            try
            {
                var result = await service.CreateHouseAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/houses/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetHouse(IHouseService service, Guid id)
        {
            try
            {
                var result = await service.GetHouseByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetHouseDetails(IHouseService service, Guid id)
        {
            try
            {
                var result = await service.GetHouseDetailsAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateHouse(IHouseService service, Guid id, UpdateHouseDto dto)
        {
            try
            {
                var result = await service.UpdateHouseAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteHouse(IHouseService service, Guid id)
        {
            try
            {
                await service.DeleteHouseAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetHousesByResidence(IHouseService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetHousesByResidenceAsync(residenceId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
