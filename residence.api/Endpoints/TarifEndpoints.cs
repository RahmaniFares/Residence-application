using residence.application.DTOs;
using residence.application.Interfaces;
using System;
using Microsoft.AspNetCore.Mvc;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Tariff management endpoints
    /// </summary>
    public static class TarifEndpoints
    {
        public static void MapTarifEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/tarifs")
                .WithTags("Tariffs")
                .WithOpenApi();

            group.MapPost("/", CreateTarif)
                .WithName("CreateTarif")
                .WithSummary("Create a new tariff");

            group.MapGet("/{tarifId}", GetTarif)
                .WithName("GetTarif")
                .WithSummary("Get tariff by ID");

            group.MapGet("/", GetTarifsByResidence)
                .WithName("GetTarifsByResidence")
                .WithSummary("Get all tariffs for a residence");

            group.MapGet("/current/active", GetCurrentTarif)
                .WithName("GetCurrentTarif")
                .WithSummary("Get current active tariff");

            group.MapPut("/{tarifId}", UpdateTarif)
                .WithName("UpdateTarif")
                .WithSummary("Update tariff");

            group.MapDelete("/{tarifId}", DeleteTarif)
                .WithName("DeleteTarif")
                .WithSummary("Delete tariff");

            group.MapGet("/{tarifId}/history", GetTarifHistory)
                .WithName("GetTarifHistory")
                .WithSummary("Get history of specific tariff changes");

            group.MapGet("/history/all", GetResidenceTarifHistory)
                .WithName("GetResidenceTarifHistory")
                .WithSummary("Get all tariff changes for a residence");

            group.MapGet("/history/range", GetTarifHistoryByDateRange)
                .WithName("GetTarifHistoryByDateRange")
                .WithSummary("Get tariff changes within a date range");

            group.MapPut("/{tarifId}/history/{historyId}", UpdateTarifHistory)
                .WithName("UpdateTarifHistory")
                .WithSummary("Update tariff history record");
        }

        private static async Task<IResult> CreateTarif(
            ITarifService service,
            Guid residenceId,
            CreateTarifDto dto
            )
        {
            try
            {
                var result = await service.CreateTarifAsync(residenceId, dto, string.Empty);
                return Results.Created($"/api/residences/{residenceId}/tarifs/{result.Id}", result);
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

        private static async Task<IResult> GetTarif(ITarifService service, Guid residenceId, Guid tarifId)
        {
            try
            {
                var result = await service.GetTarifByIdAsync(tarifId);
                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetTarifsByResidence(ITarifService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetTarifsByResidenceAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetCurrentTarif(ITarifService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetCurrentTarifAsync(residenceId);
                if (result == null)
                    return Results.NotFound(new { message = "No active tariff found" });

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> UpdateTarif(
            ITarifService service,
            Guid residenceId,
            Guid tarifId,
            UpdateTarifDto dto
            )
        {
            try
            {
                var result = await service.UpdateTarifAsync(residenceId, tarifId, dto,string.Empty);
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

        private static async Task<IResult> DeleteTarif(ITarifService service, Guid residenceId, Guid tarifId)
        {
            try
            {
                var result = await service.DeleteTarifAsync(residenceId, tarifId);
                if (!result)
                    return Results.NotFound();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetTarifHistory(ITarifService service, Guid residenceId, Guid tarifId)
        {
            try
            {
                var result = await service.GetTarifHistoryAsync(tarifId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetResidenceTarifHistory(ITarifService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetResidenceTarifHistoryAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetTarifHistoryByDateRange(
            ITarifService service,
            Guid residenceId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate >= endDate)
                    return Results.BadRequest(new { message = "Start date must be before end date" });

                var result = await service.GetTarifHistoryByDateRangeAsync(residenceId, startDate, endDate);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> UpdateTarifHistory(
            ITarifService service,
            Guid residenceId,
            Guid tarifId,
            Guid historyId,
            UpdateTarifHistoryDto dto)
        {
            try
            {
                var result = await service.UpdateTarifHistoryAsync(residenceId, tarifId, historyId, dto, string.Empty);
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
    }
}
