using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Incident endpoints
    /// </summary>
    public static class IncidentEndpoints
    {
        public static void MapIncidentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/incidents")
                .WithTags("Incidents")
                .WithOpenApi();

            group.MapPost("/", CreateIncident)
                .WithName("CreateIncident")
                .WithSummary("Report a new incident");

            group.MapGet("/{id}", GetIncident)
                .WithName("GetIncident")
                .WithSummary("Get incident by ID");

            group.MapPut("/{id}", UpdateIncident)
                .WithName("UpdateIncident")
                .WithSummary("Update incident");

            group.MapDelete("/{id}", DeleteIncident)
                .WithName("DeleteIncident")
                .WithSummary("Delete incident");

            group.MapGet("/", GetIncidentsByResidence)
                .WithName("GetIncidentsByResidence")
                .WithSummary("Get all incidents");

            group.MapGet("/resident/{residentId}", GetIncidentsByResident)
                .WithName("GetIncidentsByResident")
                .WithSummary("Get resident's incidents");

            group.MapPost("/{incidentId}/comments", AddCommentToIncident)
                .WithName("AddCommentToIncident")
                .WithSummary("Add comment to incident");

            group.MapGet("/{incidentId}/comments", GetIncidentComments)
                .WithName("GetIncidentComments")
                .WithSummary("Get incident comments");
        }

        private static async Task<IResult> CreateIncident(IIncidentService service, Guid residenceId, CreateIncidentDto dto)
        {
            try
            {
                var result = await service.CreateIncidentAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/incidents/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetIncident(IIncidentService service, Guid id)
        {
            try
            {
                var result = await service.GetIncidentByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateIncident(IIncidentService service, Guid id, UpdateIncidentDto dto)
        {
            try
            {
                var result = await service.UpdateIncidentAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteIncident(IIncidentService service, Guid id)
        {
            try
            {
                await service.DeleteIncidentAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetIncidentsByResidence(IIncidentService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetIncidentsByResidenceAsync(residenceId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetIncidentsByResident(IIncidentService service, Guid residentId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetIncidentsByResidentAsync(residentId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> AddCommentToIncident(IIncidentService service, Guid incidentId, CreateIncidentCommentDto dto)
        {
            try
            {
                var result = await service.AddCommentAsync(incidentId, dto);
                return Results.Created($"/api/incidents/{incidentId}/comments/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetIncidentComments(IIncidentService service, Guid incidentId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetCommentsAsync(incidentId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
