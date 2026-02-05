using residence.application.DTOs;
using residence.application.Interfaces;

namespace residence.api.Endpoints
{
    /// <summary>
    /// Expense endpoints
    /// </summary>
    public static class ExpenseEndpoints
    {
        public static void MapExpenseEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/residences/{residenceId}/expenses")
                .WithTags("Expenses")
                .WithOpenApi();

            group.MapPost("/", CreateExpense)
                .WithName("CreateExpense")
                .WithSummary("Create a new expense");

            group.MapGet("/{id}", GetExpense)
                .WithName("GetExpense")
                .WithSummary("Get expense by ID");

            group.MapPut("/{id}", UpdateExpense)
                .WithName("UpdateExpense")
                .WithSummary("Update expense");

            group.MapDelete("/{id}", DeleteExpense)
                .WithName("DeleteExpense")
                .WithSummary("Delete expense");

            group.MapGet("/", GetExpensesByResidence)
                .WithName("GetExpensesByResidence")
                .WithSummary("Get all expenses");

            group.MapPost("/{expenseId}/images", AddImageToExpense)
                .WithName("AddImageToExpense")
                .WithSummary("Add image to expense");

            group.MapDelete("/images/{imageId}", RemoveImageFromExpense)
                .WithName("RemoveImageFromExpense")
                .WithSummary("Remove image from expense");
        }

        private static async Task<IResult> CreateExpense(IExpenseService service, Guid residenceId, CreateExpenseDto dto)
        {
            try
            {
                var result = await service.CreateExpenseAsync(residenceId, dto);
                return Results.Created($"/api/residences/{residenceId}/expenses/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetExpense(IExpenseService service, Guid id)
        {
            try
            {
                var result = await service.GetExpenseByIdAsync(id);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
        }

        private static async Task<IResult> UpdateExpense(IExpenseService service, Guid id, UpdateExpenseDto dto)
        {
            try
            {
                var result = await service.UpdateExpenseAsync(id, dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> DeleteExpense(IExpenseService service, Guid id)
        {
            try
            {
                await service.DeleteExpenseAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> GetExpensesByResidence(IExpenseService service, Guid residenceId, [AsParameters] PaginationDto pagination)
        {
            try
            {
                var result = await service.GetExpensesByResidenceAsync(residenceId, pagination);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> AddImageToExpense(IExpenseService service, Guid expenseId, CreateExpenseImageDto dto)
        {
            try
            {
                var result = await service.AddImageToExpenseAsync(expenseId, dto);
                return Results.Created($"/api/expenses/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> RemoveImageFromExpense(IExpenseService service, Guid imageId)
        {
            try
            {
                await service.RemoveImageFromExpenseAsync(imageId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }
    }
}
