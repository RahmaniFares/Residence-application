using residence.application.DTOs;
using residence.application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Donation/Contribution management API endpoints
    /// </summary>
    public static class DonationEndpoints
    {
        public static void MapDonationEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/donations")
                .WithTags("Donations")
                .WithOpenApi();

            // Donation CRUD
            group.MapPost("/", CreateDonation)
                .WithName("CreateDonation")
                .WithSummary("Create a new donation");

            //// Specific routes BEFORE general {id} routes
            group.MapGet("/house/{houseId}", GetDonationsByHouse)
                .WithName("GetDonationsByHouse")
                .WithSummary("Get donations for a specific house");

            group.MapGet("/donor/{donorId}", GetDonationsByDonor)
                .WithName("GetDonationsByDonor")
                .WithSummary("Get donations from a specific donor");

            group.MapGet("/by-date-range", GetDonationsByDateRange)
                .WithName("GetDonationsByDateRange")
                .WithSummary("Get donations within a date range");

            group.MapGet("/statistics/total-by-donor", GetTotalDonationsByDonor)
                .WithName("GetTotalDonationsByDonor")
                .WithSummary("Get total donations from a donor");

            //// Specific detail route before general {id} route
            group.MapGet("/{id}/details", GetDonationDetails)
                .WithName("GetDonationDetails")
                .WithSummary("Get detailed donation information");

            group.MapGet("/house/{houseId}/total", GetTotalDonationsByHouse)
                .WithName("GetTotalDonationsByHouse")
                .WithSummary("Get total donation amount for a house");

            //// General {id} routes
            group.MapGet("/{id}", GetDonationById)
                .WithName("GetDonationById")
                .WithSummary("Get donation by ID");

            group.MapPut("/{id}", UpdateDonation)
                .WithName("UpdateDonation")
                .WithSummary("Update a donation");

            group.MapDelete("/{id}", DeleteDonation)
                .WithName("DeleteDonation")
                .WithSummary("Delete a donation");

            //// General list route LAST
            group.MapGet("/", GetAllDonations)
                .WithName("GetAllDonations")
                .WithSummary("Get all donations");
        }

        private static async Task<IResult> CreateDonation(IDonationService service, [FromBody] CreateDonationDto dto)
        {
            try
            {
                if (!dto.HouseId.HasValue)
                    return Results.BadRequest(new { message = "HouseId is required" });

                var result = await service.CreateDonationAsync(dto.HouseId.Value, dto);
                return Results.Created($"/api/donations/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetDonationById(IDonationService service, Guid id)
        {
            try
            {
                var result = await service.GetDonationByIdAsync(id);
                if (result == null)
                    return Results.NotFound(new { message = "Donation not found" });

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateDonation(IDonationService service, Guid id, [FromBody] UpdateDonationDto dto)
        {
            try
            {
                var result = await service.UpdateDonationAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteDonation(IDonationService service, Guid id)
        {
            try
            {
                await service.DeleteDonationAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetAllDonations(IDonationService service)
        {
            try
            {
                var result = await service.GetAllDonationsAsync();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetDonationsByHouse(IDonationService service, Guid houseId)
        {
            try
            {
                var result = await service.GetDonationsByHouseAsync(houseId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetDonationsByDonor(IDonationService service, Guid donorId)
        {
            try
            {
                var result = await service.GetDonationsByDonorAsync(donorId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetDonationsByDateRange(IDonationService service, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return Results.BadRequest(new { message = "Start date must be before end date" });

                var result = await service.GetDonationsByDateRangeAsync(startDate, endDate);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetTotalDonationsByHouse(IDonationService service, Guid houseId)
        {
            try
            {
                var result = await service.GetTotalDonationsByHouseAsync(houseId);
                return Results.Ok(new { houseId, total = result });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetDonationDetails(IDonationService service, Guid id)
        {
            try
            {
                var result = await service.GetDonationDetailsAsync(id);
                if (result == null)
                    return Results.NotFound(new { message = "Donation not found" });

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetTotalDonationsByDonor(IDonationService service, Guid donorId)
        {
            try
            {
                var donations = await service.GetDonationsByDonorAsync(donorId);
                var total = donations.Sum(d => d.Amount);
                return Results.Ok(new { donorId, total });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
