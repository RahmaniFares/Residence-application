using residence.application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Interfaces
{
    /// <summary>
    /// Service interface for Employee management
    /// </summary>
    public interface IEmployeeService
    {
        // Employee CRUD
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<EmployeeDto> GetEmployeeAsync(Guid id);
        Task<EmployeeDetailDto> GetEmployeeDetailAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetEmployeesByResidenceAsync(Guid residenceId);
        Task<EmployeeDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
        Task DeleteEmployeeAsync(Guid id);

        // Employee Search
        Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync(Guid residenceId);
        Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesByPositionAsync(Guid residenceId, string position);
        Task<int> GetEmployeeCountAsync(Guid residenceId);

        // Salary Management
        Task<CurrentEmployeeSalaryDto?> GetCurrentSalaryAsync(Guid employeeId);
        Task<IEnumerable<EmployeeSalaryDto>> GetSalaryHistoryAsync(Guid employeeId);
        Task<EmployeeSalaryDto> ChangeSalaryAsync(CreateEmployeeSalaryDto dto);
        Task<EmployeeSalaryDto?> GetSalaryAtDateAsync(Guid employeeId, DateTime date);
        Task<IEnumerable<EmployeeSalaryDto>> GetSalariesInDateRangeAsync(
            Guid employeeId, DateTime startDate, DateTime endDate);

        // Salary Details
        Task<EmployeeDetailDto> GetEmployeeWithSalaryHistoryAsync(Guid employeeId);
        Task<(IEnumerable<EmployeeSalaryDto>, int)> GetSalaryHistoryPagedAsync(
            Guid employeeId, int pageNumber, int pageSize);

        // Payroll Reports
        Task<decimal?> GetTotalMonthlyPayrollAsync(Guid residenceId);
        Task<decimal?> GetAverageSalaryByPositionAsync(Guid residenceId, string position);
        Task<IEnumerable<EmployeeSummaryDto>> GetPayrollSummaryAsync(Guid residenceId);
    }
}
