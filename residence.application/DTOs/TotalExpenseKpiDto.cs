namespace residence.application.DTOs;

/// <summary>
/// DTO for total expense KPI
/// </summary>
public record TotalExpenseKpiDto(
    decimal TotalAmount,
    int TotalExpenseCount,
    decimal AverageExpense,
    decimal MaxExpense,
    decimal MinExpense,
    DateTime? EarliestExpenseDate,
    DateTime? LatestExpenseDate
);
