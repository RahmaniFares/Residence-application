using residence.application.Repositories;
using residence.domain.Entities;
using residence.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace residence.infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for Employee operations
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Employee CRUD

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Salaries)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Salaries)
                .OrderBy(e => e.Position)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByResidenceAsync(Guid residenceId)
        {
            return await _context.Employees
                .Include(e => e.Salaries)
                .Where(e => e.ResidenceId == residenceId)
                .OrderBy(e => e.Position)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Employee Status & Search

        public async Task<IEnumerable<Employee>> GetActiveEmployeesByResidenceAsync(Guid residenceId)
        {
            return await _context.Employees
                .Include(e => e.Salaries)
                .Where(e => e.ResidenceId == residenceId && e.Status == EmployeeStatus.Active)
                .OrderBy(e => e.Position)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionAsync(Guid residenceId, string position)
        {
            return await _context.Employees
                .Include(e => e.Salaries)
                .Where(e => e.ResidenceId == residenceId && e.Position == position)
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<int> GetEmployeeCountByResidenceAsync(Guid residenceId)
        {
            return await _context.Employees.CountAsync(e => e.ResidenceId == residenceId);
        }

        #endregion

        #region Salary Operations

        public async Task<EmployeeSalary?> GetCurrentSalaryByEmployeeAsync(Guid employeeId)
        {
            return await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == employeeId && es.IsCurrent)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeSalary>> GetSalaryHistoryByEmployeeAsync(Guid employeeId)
        {
            return await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == employeeId)
                .OrderByDescending(es => es.EffectiveDate)
                .ToListAsync();
        }

        public async Task<EmployeeSalary?> GetSalaryAtDateAsync(Guid employeeId, DateTime date)
        {
            return await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == employeeId &&
                       es.EffectiveDate <= date &&
                       (es.EndDate == null || es.EndDate >= date))
                .OrderByDescending(es => es.EffectiveDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeSalary>> GetSalariesInDateRangeAsync(
            Guid employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.EmployeeSalaries
                .Where(es => es.EmployeeId == employeeId &&
                       ((es.EffectiveDate <= endDate && (es.EndDate == null || es.EndDate >= startDate))))
                .OrderByDescending(es => es.EffectiveDate)
                .ToListAsync();
        }

        public async Task<EmployeeSalary> AddSalaryAsync(EmployeeSalary salary)
        {
            _context.EmployeeSalaries.Add(salary);
            await _context.SaveChangesAsync();
            return salary;
        }

        public async Task<EmployeeSalary> UpdateSalaryAsync(EmployeeSalary salary)
        {
            _context.EmployeeSalaries.Update(salary);
            await _context.SaveChangesAsync();
            return salary;
        }

        public async Task DeleteSalaryAsync(Guid salaryId)
        {
            var salary = await _context.EmployeeSalaries.FindAsync(salaryId);
            if (salary != null)
            {
                _context.EmployeeSalaries.Remove(salary);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Salary Statistics

        public async Task<(IEnumerable<EmployeeSalary>, int)> GetSalaryHistoryPagedAsync(
            Guid employeeId, int pageNumber, int pageSize)
        {
            var query = _context.EmployeeSalaries
                .Where(es => es.EmployeeId == employeeId)
                .OrderByDescending(es => es.EffectiveDate);

            var total = await query.CountAsync();
            var salaries = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (salaries, total);
        }

        public async Task<decimal?> GetTotalMonthlyPayrollByResidenceAsync(Guid residenceId)
        {
            var total = await _context.EmployeeSalaries
                .Where(es => es.EmployeeId != Guid.Empty &&
                       es.IsCurrent &&
                       _context.Employees
                           .Where(e => e.ResidenceId == residenceId)
                           .Select(e => e.Id)
                           .Contains(es.EmployeeId))
                .SumAsync(es => (decimal?)es.Amount);

            return total;
        }

        public async Task<decimal?> GetAverageSalaryByPositionAsync(Guid residenceId, string position)
        {
            var average = await _context.EmployeeSalaries
                .Where(es => es.IsCurrent &&
                       _context.Employees
                           .Where(e => e.ResidenceId == residenceId && e.Position == position)
                           .Select(e => e.Id)
                           .Contains(es.EmployeeId))
                .AverageAsync(es => (decimal?)es.Amount);

            return average;
        }

        #endregion
    }
}
