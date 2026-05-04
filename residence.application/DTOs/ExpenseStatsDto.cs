namespace residence.application.DTOs;

/// <summary>
/// DTO for expense statistics by type
/// </summary>
public record ExpenseTypeStatsDto(
    ExpenseType Type,
    string TypeName,
    int Count,
    decimal TotalAmount,
    decimal AverageAmount,
    decimal PercentageOfTotal
);

/// <summary>
/// Wrapper for expense type statistics
/// </summary>
public record ExpenseStatsDto(
    List<ExpenseTypeStatsDto> Data,
    decimal TotalAmount,
    int TotalExpenseCount,
    ExpenseTypeStatsDto HighestCategory,
    ExpenseTypeStatsDto LowestCategory
);
