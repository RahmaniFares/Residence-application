using Microsoft.AspNetCore.Mvc;
using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints;

/// <summary>
/// Rappel (Backpay) endpoints
/// </summary>
public static class RappelEndpoints
{
    public static void MapRappelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/residences/{residenceId}/rappels")
            .WithTags("Rappels")
            .WithOpenApi();

        group.MapPost("/", CreateRappel)
            .WithName("CreateRappel")
            .WithSummary("Create a new rappel (backpay/retroactive payment)");

        group.MapGet("/{id}", GetRappel)
            .WithName("GetRappel")
            .WithSummary("Get a rappel by ID");

        group.MapPut("/{id}", UpdateRappel)
            .WithName("UpdateRappel")
            .WithSummary("Update an existing rappel");

        group.MapDelete("/{id}", DeleteRappel)
            .WithName("DeleteRappel")
            .WithSummary("Delete a rappel");

        group.MapGet("/house/{houseId}", GetRappelsByHouse)
            .WithName("GetRappelsByHouse")
            .WithSummary("Get all rappels for a specific house");

        group.MapGet("/", GetRappelsByResidence)
            .WithName("GetRappelsByResidence")
            .WithSummary("Get all rappels for a residence");
    }

    private static async Task<IResult> CreateRappel(
        [FromRoute] Guid residenceId,
        [FromBody] CreateRappelDto dto,
        [FromServices] IRappelService service)
    {
        var result = await service.CreateRappelAsync(residenceId, dto);
        return Results.Created($"/api/residences/{residenceId}/rappels/{result.Id}", result);
    }

    private static async Task<IResult> GetRappel(
        [FromRoute] Guid id,
        [FromServices] IRappelService service)
    {
        var result = await service.GetRappelByIdAsync(id);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateRappel(
        [FromRoute] Guid id,
        [FromBody] UpdateRappelDto dto,
        [FromServices] IRappelService service)
    {
        var result = await service.UpdateRappelAsync(id, dto);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteRappel(
        [FromRoute] Guid id,
        [FromServices] IRappelService service)
    {
        await service.DeleteRappelAsync(id);
        return Results.NoContent();
    }

    private static async Task<IResult> GetRappelsByHouse(
        [FromRoute] Guid houseId,
        [AsParameters] PaginationDto pagination,
        [FromServices] IRappelService service)
    {
        var result = await service.GetRappelsByHouseAsync(houseId, pagination);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetRappelsByResidence(
        [FromRoute] Guid residenceId,
        [AsParameters] PaginationDto pagination,
        [FromServices] IRappelService service)
    {
        var result = await service.GetRappelsByResidenceAsync(residenceId, pagination);
        return Results.Ok(result);
    }
}
