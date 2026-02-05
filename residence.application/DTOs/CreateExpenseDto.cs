using residence.domain.Enums;

namespace residence.application.DTOs;



public record CreateExpenseDto(
    string Title,
    ExpenseType Type,
    decimal Amount,
    DateTime ExpenseDate,
    string Description
);

