using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Repositories
{
    /// <summary>
    /// Repository interface for Employee operations
    /// </summary>
    public interface IEmployeeRepository
    {
        // Employee CRUD
        Task<Employee?> GetByIdAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByResidenceAsync(Guid residenceId);
        Task<Employee> AddAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);

        // Employee Status & Search
        Task<IEnumerable<Employee>> GetActiveEmployeesByResidenceAsync(Guid residenceId);
        Task<IEnumerable<Employee>> GetEmployeesByPositionAsync(Guid residenceId, string position);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetEmployeeCountByResidenceAsync(Guid residenceId);

        // Salary Operations
        Task<EmployeeSalary?> GetCurrentSalaryByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<EmployeeSalary>> GetSalaryHistoryByEmployeeAsync(Guid employeeId);
        Task<EmployeeSalary?> GetSalaryAtDateAsync(Guid employeeId, DateTime date);
        Task<IEnumerable<EmployeeSalary>> GetSalariesInDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate);
        Task<EmployeeSalary> AddSalaryAsync(EmployeeSalary salary);
        Task<EmployeeSalary> UpdateSalaryAsync(EmployeeSalary salary);
        Task DeleteSalaryAsync(Guid salaryId);

        // Salary Statistics
        Task<(IEnumerable<EmployeeSalary>, int)> GetSalaryHistoryPagedAsync(Guid employeeId, int pageNumber, int pageSize);
        Task<decimal?> GetTotalMonthlyPayrollByResidenceAsync(Guid residenceId);
        Task<decimal?> GetAverageSalaryByPositionAsync(Guid residenceId, string position);
    }
}
