using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Resident endpoints
    /// </summary>
    public static class ResidentEndpoints
    {
        public static void MapResidentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/residents")
                .WithTags("Residents")
                .WithOpenApi();

            group.MapPost("/", CreateResident)
                .WithName("CreateResident")
                .WithSummary("Create a new resident");

            group.MapGet("/{id}", GetResident)
                .WithName("GetResident")
                .WithSummary("Get resident by ID");

            group.MapPut("/{id}", UpdateResident)
                .WithName("UpdateResident")
                .WithSummary("Update resident");

            group.MapDelete("/{id}", DeleteResident)
                .WithName("DeleteResident")
                .WithSummary("Delete resident");

            group.MapGet("/", GetResidentsByResidence)
                .WithName("GetResidentsByResidence")
                .WithSummary("Get all residents in residence");

            group.MapGet("/house/{houseId}", GetResidentsByHouse)
                .WithName("GetResidentsByHouse")
                .WithSummary("Get residents in a specific house");
        }

        private static async Task<IResult> CreateResident(IResidentService service, Guid residenceId, CreateResidentDto dto)
        {
            try
            {
                var result = await service.CreateResidentAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/residents/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetResident(IResidentService service, Guid id)
        {
            try
            {
                var result = await service.GetResidentByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateResident(IResidentService service, Guid id, UpdateResidentDto dto)
        {
            try
            {
                var result = await service.UpdateResidentAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteResident(IResidentService service, Guid id)
        {
            try
            {
                await service.DeleteResidentAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetResidentsByResidence(IResidentService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetResidentsByResidenceAsync(residenceId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetResidentsByHouse(IResidentService service, Guid houseId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetResidentsByHouseAsync(houseId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
