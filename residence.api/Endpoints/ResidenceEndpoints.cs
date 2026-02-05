using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Residence endpoints
    /// </summary>
    public static class ResidenceEndpoints
    {
        public static void MapResidenceEndpointsCustom(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences")
                .WithTags("Residences")
                .WithOpenApi();

            group.MapPost("/", CreateResidence)
                .WithName("CreateResidence")
                .WithSummary("Create a new residence")
                .Produces<ResidenceDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("/{id}", GetResidence)
                .WithName("GetResidence")
                .WithSummary("Get residence by ID")
                .Produces<ResidenceDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id}", UpdateResidence)
                .WithName("UpdateResidence")
                .WithSummary("Update residence")
                .Produces<ResidenceDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapDelete("/{id}", DeleteResidence)
                .WithName("DeleteResidence")
                .WithSummary("Delete residence")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapGet("/{residenceId}/settings", GetSettings)
                .WithName("GetSettings")
                .WithSummary("Get residence settings")
                .Produces<ResidenceSettingsDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{residenceId}/settings", UpdateSettings)
                .WithName("UpdateSettings")
                .WithSummary("Update residence settings")
                .Produces<ResidenceSettingsDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> CreateResidence(IResidenceService service, CreateResidenceDto dto)
        {
            try
            {
                var result = await service.CreateResidenceAsync(dto);
                return Results.Created($"/api/residences/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetResidence(IResidenceService service, Guid id)
        {
            try
            {
                var result = await service.GetResidenceByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateResidence(IResidenceService service, Guid id, UpdateResidenceDto dto)
        {
            try
            {
                var result = await service.UpdateResidenceAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteResidence(IResidenceService service, Guid id)
        {
            try
            {
                await service.DeleteResidenceAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetSettings(IResidenceService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetSettingsAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateSettings(IResidenceService service, Guid residenceId, ResidenceSettingsDto dto)
        {
            try
            {
                var result = await service.UpdateSettingsAsync(residenceId, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
