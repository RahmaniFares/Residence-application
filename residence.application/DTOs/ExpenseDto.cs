using residence.domain.Enums;

namespace residence.application.DTOs;

public record ExpenseDto(
    Guid Id,
    string Title,
    ExpenseType Type,
    decimal Amount,
    DateTime ExpenseDate,
    string Description,
    List<string> ImageUrls,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

