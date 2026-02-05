namespace residence.application.DTOs;

public record CreateExpenseImageDto(
    Guid ExpenseId,
    string ImageUrl
);

