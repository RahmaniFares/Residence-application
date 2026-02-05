using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdateExpenseDto(
    string Title,
    ExpenseType Type,
    decimal Amount,
    DateTime ExpenseDate,
    string Description
);
