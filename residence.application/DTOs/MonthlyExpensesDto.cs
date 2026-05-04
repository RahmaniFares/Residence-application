namespace residence.application.DTOs;

/// <summary>
/// DTO for monthly expense breakdown
/// </summary>
public record MonthlyExpenseDto(
    int Year,
    int Month,
    string MonthName,
    decimal TotalAmount,
    int ExpenseCount,
    decimal AverageExpense
);

/// <summary>
/// Wrapper for monthly expenses list
/// </summary>
public record MonthlyExpensesDto(
    List<MonthlyExpenseDto> Data,
    decimal TotalAmount,
    int TotalExpenseCount,
    int MonthsWithData
);
