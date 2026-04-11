using residence.application.DTOs;
using residence.application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace residence.api.Endpoints
{

    /// <summary>
    /// Payment endpoints
    /// </summary>
    public static class PaymentEndpoints
    {
        public static void MapPaymentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/payments")
                .WithTags("Payments")
                .WithOpenApi();

            group.MapPost("/", CreatePayment)
                .WithName("CreatePayment")
                .WithSummary("Create a new payment");

            group.MapGet("/{id}", GetPayment)
                .WithName("GetPayment")
                .WithSummary("Get payment by ID");

            group.MapPut("/{id}", UpdatePayment)
                .WithName("UpdatePayment")
                .WithSummary("Update payment");

            group.MapDelete("/{id}", DeletePayment)
                .WithName("DeletePayment")
                .WithSummary("Delete payment");

            group.MapGet("/", GetPaymentsByResidence)
                .WithName("GetPaymentsByResidence")
                .WithSummary("Get all payments in residence");

            group.MapGet("/resident/{residentId}", GetPaymentsByResident)
                .WithName("GetPaymentsByResident")
                .WithSummary("Get resident payments");

            group.MapGet("/house/{houseId}", GetPaymentsByHouse)
                .WithName("GetPaymentsByHouse")
                .WithSummary("Get house payments");

            // KPI endpoints
            group.MapGet("/kpi/dashboard", GetPaymentKpi)
                .WithName("GetPaymentKpi")
                .WithSummary("Get payment KPI and statistics");

            group.MapGet("/kpi/range", GetPaymentKpiByDateRange)
                .WithName("GetPaymentKpiByDateRange")
                .WithSummary("Get payment KPI for date range");

            group.MapGet("/kpi/monthly-summary", GetMonthlySummary)
                .WithName("GetMonthlySummary")
                .WithSummary("Get monthly payment summaries");

            group.MapGet("/kpi/trend", GetPaymentTrend)
                .WithName("GetPaymentTrend")
                .WithSummary("Get payment trend data");
        }

        private static async Task<IResult> CreatePayment(IPaymentService service, Guid residenceId, CreatePaymentDto dto)
        {
            try
            {

                var result = await service.CreatePaymentAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/payments/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPayment(IPaymentService service, Guid id)
        {
            try
            {
                var result = await service.GetPaymentByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdatePayment(IPaymentService service, Guid id, UpdatePaymentDto dto)
        {
            try
            {
                var result = await service.UpdatePaymentAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeletePayment(IPaymentService service, Guid id)
        {
            try
            {
                await service.DeletePaymentAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPaymentsByResidence(IPaymentService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetPaymentsByResidenceAsync(residenceId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPaymentsByResident(IPaymentService service, Guid residentId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetPaymentsByResidentAsync(residentId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPaymentsByHouse(IPaymentService service, Guid houseId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetPaymentsByHouseAsync(houseId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPaymentKpi(IPaymentService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetPaymentKpiAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetPaymentKpiByDateRange(
            IPaymentService service,
            Guid residenceId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate >= endDate)
                    return Results.BadRequest(new { message = "Start date must be before end date" });

                var result = await service.GetPaymentKpiByDateRangeAsync(residenceId, startDate, endDate);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetMonthlySummary(
            IPaymentService service,
            Guid residenceId,
            [FromQuery] int months = 12)
        {
            try
            {
                if (months < 1 || months > 120)
                    return Results.BadRequest(new { message = "Months must be between 1 and 120" });

                var result = await service.GetMonthlyPaymentSummaryAsync(residenceId, months);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }

        private static async Task<IResult> GetPaymentTrend(
            IPaymentService service,
            Guid residenceId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate >= endDate)
                    return Results.BadRequest(new { message = "Start date must be before end date" });

                var result = await service.GetPaymentTrendAsync(residenceId, startDate, endDate);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }
        }
    }
}
