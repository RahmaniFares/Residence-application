using residence.application.DTOs;
using residence.application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Employee management API endpoints
    /// </summary>
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/employees")
                .WithTags("Employees")
                .WithOpenApi();

            // Employee CRUD
            group.MapPost("/", CreateEmployee)
                .WithName("CreateEmployee")
                .WithSummary("Create a new employee");

            // Specific routes BEFORE general {id} routes
            group.MapGet("/residence/{residenceId}", GetEmployeesByResidence)
                .WithName("GetEmployeesByResidence")
                .WithSummary("Get employees by residence");

            group.MapGet("/residence/{residenceId}/active", GetActiveEmployees)
                .WithName("GetActiveEmployees")
                .WithSummary("Get active employees by residence");

            group.MapGet("/residence/{residenceId}/position", GetEmployeesByPosition)
                .WithName("GetEmployeesByPosition")
                .WithSummary("Get employees by position");

            group.MapGet("/residence/{residenceId}/count", GetEmployeeCount)
                .WithName("GetEmployeeCount")
                .WithSummary("Get employee count");

            // Salary Management - specific routes before {id}
            group.MapGet("/{employeeId}/salary/current", GetCurrentSalary)
                .WithName("GetCurrentSalary")
                .WithSummary("Get current salary");

            group.MapGet("/{employeeId}/salary/history", GetSalaryHistory)
                .WithName("GetSalaryHistory")
                .WithSummary("Get salary history");

            group.MapGet("/{employeeId}/salary/history-paged", GetSalaryHistoryPaged)
                .WithName("GetSalaryHistoryPaged")
                .WithSummary("Get paged salary history");

            group.MapPost("/{employeeId}/salary/change", ChangeSalary)
                .WithName("ChangeSalary")
                .WithSummary("Change employee salary (creates new record)");

            group.MapGet("/{employeeId}/salary/at-date", GetSalaryAtDate)
                .WithName("GetSalaryAtDate")
                .WithSummary("Get salary at specific date");

            // Detail route before general {id} route
            group.MapGet("/{id}/detail", GetEmployeeDetail)
                .WithName("GetEmployeeDetail")
                .WithSummary("Get employee with full details");

            // General {id} routes
            group.MapGet("/{id}", GetEmployee)
                .WithName("GetEmployee")
                .WithSummary("Get employee by ID");

            group.MapPut("/{id}", UpdateEmployee)
                .WithName("UpdateEmployee")
                .WithSummary("Update an employee");

            group.MapDelete("/{id}", DeleteEmployee)
                .WithName("DeleteEmployee")
                .WithSummary("Delete an employee");

            // General list route LAST
            group.MapGet("/", GetAllEmployees)
                .WithName("GetAllEmployees")
                .WithSummary("Get all employees");

            // Date range queries
            group.MapPost("/{employeeId}/salary/date-range", GetSalariesInRange)
                .WithName("GetSalariesInRange")
                .WithSummary("Get salaries in date range");

            // Reports - these are specific enough so they should be fine
            group.MapGet("/payroll/{residenceId}/total", GetTotalPayroll)
                .WithName("GetTotalPayroll")
                .WithSummary("Get total monthly payroll");

            group.MapGet("/payroll/{residenceId}/position-average", GetPositionAverage)
                .WithName("GetPositionAverage")
                .WithSummary("Get average salary by position");

            group.MapGet("/payroll/{residenceId}/summary", GetPayrollSummary)
                .WithName("GetPayrollSummary")
                .WithSummary("Get payroll summary");
        }

        #region Employee CRUD

        private static async Task<IResult> CreateEmployee(IEmployeeService service, CreateEmployeeDto dto)
        {
            try
            {
                var result = await service.CreateEmployeeAsync(dto);
                return Results.Created($"/api/employees/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetEmployee(IEmployeeService service, Guid id)
        {
            try
            {
                var result = await service.GetEmployeeAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetEmployeeDetail(IEmployeeService service, Guid id)
        {
            try
            {
                var result = await service.GetEmployeeDetailAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateEmployee(IEmployeeService service, Guid id, UpdateEmployeeDto dto)
        {
            try
            {
                var result = await service.UpdateEmployeeAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteEmployee(IEmployeeService service, Guid id)
        {
            try
            {
                await service.DeleteEmployeeAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetAllEmployees(IEmployeeService service)
        {
            try
            {
                var result = await service.GetAllEmployeesAsync();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Residence-Specific

        private static async Task<IResult> GetEmployeesByResidence(IEmployeeService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetEmployeesByResidenceAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetActiveEmployees(IEmployeeService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetActiveEmployeesAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetEmployeesByPosition(
            IEmployeeService service, Guid residenceId, string position)
        {
            try
            {
                var result = await service.GetEmployeesByPositionAsync(residenceId, position);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetEmployeeCount(IEmployeeService service, Guid residenceId)
        {
            try
            {
                var count = await service.GetEmployeeCountAsync(residenceId);
                return Results.Ok(new { count });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Salary Management

        private static async Task<IResult> GetCurrentSalary(IEmployeeService service, Guid employeeId)
        {
            try
            {
                var result = await service.GetCurrentSalaryAsync(employeeId);
                return result != null ? Results.Ok(result) : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetSalaryHistory(IEmployeeService service, Guid employeeId)
        {
            try
            {
                var result = await service.GetSalaryHistoryAsync(employeeId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetSalaryHistoryPaged(
            IEmployeeService service, Guid employeeId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (salaries, total) = await service.GetSalaryHistoryPagedAsync(employeeId, pageNumber, pageSize);
                return Results.Ok(new
                {
                    items = salaries,
                    total,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> ChangeSalary(IEmployeeService service, CreateEmployeeSalaryDto dto)
        {
            try
            {
                var result = await service.ChangeSalaryAsync(dto);
                return Results.Created($"/api/employees/{dto.EmployeeId}/salary/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetSalaryAtDate(
            IEmployeeService service, Guid employeeId, DateTime date)
        {
            try
            {
                var result = await service.GetSalaryAtDateAsync(employeeId, date);
                return result != null ? Results.Ok(result) : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetSalariesInRange(
            IEmployeeService service, Guid employeeId, EmployeeSalaryDateRangeDto dateRange)
        {
            try
            {
                var result = await service.GetSalariesInDateRangeAsync(employeeId, dateRange.StartDate, dateRange.EndDate);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Reports

        private static async Task<IResult> GetTotalPayroll(IEmployeeService service, Guid residenceId)
        {
            try
            {
                var total = await service.GetTotalMonthlyPayrollAsync(residenceId);
                return Results.Ok(new { monthlyPayroll = total ?? 0 });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPositionAverage(
            IEmployeeService service, Guid residenceId, string position)
        {
            try
            {
                var average = await service.GetAverageSalaryByPositionAsync(residenceId, position);
                return Results.Ok(new { position, averageSalary = average ?? 0 });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetPayrollSummary(IEmployeeService service, Guid residenceId)
        {
            try
            {
                var result = await service.GetPayrollSummaryAsync(residenceId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }

    /// <summary>
    /// Date range request DTO for employee salary queries
    /// </summary>
    public class EmployeeSalaryDateRangeDto
    {
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
