using residence.application.DTOs;
using residence.application.Interfaces;
using residence.application.Repositories;
using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace residence.application.Services
{
    /// <summary>
    /// Service implementation for Employee management
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IResidenceRepository _residenceRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IResidenceRepository residenceRepository)
        {
            _employeeRepository = employeeRepository;
            _residenceRepository = residenceRepository;
        }

        #region Employee CRUD

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            // Validate residence exists
            var residenceExists = await _residenceRepository.GetByIdAsync(dto.ResidenceId);
            if (residenceExists == null)
                throw new Exception("Residence not found");

            // Validate input
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new Exception("First name is required");
            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new Exception("Last name is required");
            if (string.IsNullOrWhiteSpace(dto.Position))
                throw new Exception("Position is required");
            if (dto.HireDate == default)
                throw new Exception("Hire date is required");

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                ResidenceId = dto.ResidenceId,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Position = dto.Position.Trim(),
                Email = dto.Email?.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                HireDate = dto.HireDate,
                EndDate = dto.EndDate,
                Status = EmployeeStatus.Active,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _employeeRepository.AddAsync(employee);
            return MapToDto(created);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");

            var dto = MapToDto(employee);
            var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(id);
            if (currentSalary != null)
            {
                dto.CurrentSalary = currentSalary.Amount;
            }

            return dto;
        }

        public async Task<EmployeeDetailDto> GetEmployeeDetailAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");

            var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(id);
            var salaryHistory = await _employeeRepository.GetSalaryHistoryByEmployeeAsync(id);

            var detail = new EmployeeDetailDto
            {
                Id = employee.Id,
                ResidenceId = employee.ResidenceId,
                FullName = employee.GetFullName(),
                Position = employee.Position,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                Status = (int)employee.Status,
                CurrentSalary = currentSalary != null ? new CurrentEmployeeSalaryDto
                {
                    Id = currentSalary.Id,
                    Amount = currentSalary.Amount,
                    EffectiveDate = currentSalary.EffectiveDate,
                    Reason = currentSalary.Reason
                } : null,
                SalaryHistory = salaryHistory
                    .OrderByDescending(s => s.EffectiveDate)
                    .Select(MapSalaryToDto)
                    .ToList()
            };

            return detail;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var dtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var dto = MapToDto(employee);
                var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employee.Id);
                if (currentSalary != null)
                {
                    dto.CurrentSalary = currentSalary.Amount;
                }
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByResidenceAsync(Guid residenceId)
        {
            var employees = await _employeeRepository.GetByResidenceAsync(residenceId);
            var dtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var dto = MapToDto(employee);
                var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employee.Id);
                if (currentSalary != null)
                {
                    dto.CurrentSalary = currentSalary.Amount;
                }
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");

            // Validate input
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new Exception("First name is required");
            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new Exception("Last name is required");
            if (string.IsNullOrWhiteSpace(dto.Position))
                throw new Exception("Position is required");

            employee.FirstName = dto.FirstName.Trim();
            employee.LastName = dto.LastName.Trim();
            employee.Position = dto.Position.Trim();
            employee.Email = dto.Email?.Trim();
            employee.PhoneNumber = dto.PhoneNumber?.Trim();
            employee.Status = (EmployeeStatus)dto.Status;
            employee.EndDate = dto.EndDate;
            employee.Notes = dto.Notes;
            employee.UpdatedAt = DateTime.UtcNow;

            var updated = await _employeeRepository.UpdateAsync(employee);
            var result = MapToDto(updated);
            var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(id);
            if (currentSalary != null)
            {
                result.CurrentSalary = currentSalary.Amount;
            }

            return result;
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");

            await _employeeRepository.DeleteAsync(id);
        }

        #endregion

        #region Employee Search

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync(Guid residenceId)
        {
            var employees = await _employeeRepository.GetActiveEmployeesByResidenceAsync(residenceId);
            var dtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var dto = MapToDto(employee);
                var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employee.Id);
                if (currentSalary != null)
                {
                    dto.CurrentSalary = currentSalary.Amount;
                }
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesByPositionAsync(Guid residenceId, string position)
        {
            var employees = await _employeeRepository.GetEmployeesByPositionAsync(residenceId, position);
            var summaries = new List<EmployeeSummaryDto>();

            foreach (var employee in employees)
            {
                var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employee.Id);
                var salaryHistory = await _employeeRepository.GetSalaryHistoryByEmployeeAsync(employee.Id);

                summaries.Add(new EmployeeSummaryDto
                {
                    Id = employee.Id,
                    FullName = employee.GetFullName(),
                    Position = employee.Position,
                    CurrentSalary = currentSalary?.Amount,
                    SalaryEffectiveDate = currentSalary?.EffectiveDate,
                    Status = employee.Status.ToString(),
                    SalaryHistoryCount = salaryHistory.Count()
                });
            }

            return summaries;
        }

        public async Task<int> GetEmployeeCountAsync(Guid residenceId)
        {
            return await _employeeRepository.GetEmployeeCountByResidenceAsync(residenceId);
        }

        #endregion

        #region Salary Management

        public async Task<CurrentEmployeeSalaryDto?> GetCurrentSalaryAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employeeId);
            if (currentSalary == null)
                return null;

            return new CurrentEmployeeSalaryDto
            {
                Id = currentSalary.Id,
                Amount = currentSalary.Amount,
                EffectiveDate = currentSalary.EffectiveDate,
                Reason = currentSalary.Reason
            };
        }

        public async Task<IEnumerable<EmployeeSalaryDto>> GetSalaryHistoryAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            var salaries = await _employeeRepository.GetSalaryHistoryByEmployeeAsync(employeeId);
            return salaries
                .OrderByDescending(s => s.EffectiveDate)
                .Select(MapSalaryToDto)
                .ToList();
        }

        public async Task<EmployeeSalaryDto> ChangeSalaryAsync(CreateEmployeeSalaryDto dto)
        {
            // Validate employee exists
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            // Validate amount
            if (dto.Amount <= 0)
                throw new Exception("Salary amount must be greater than zero");

            // Validate effective date
            if (dto.EffectiveDate == default)
                throw new Exception("Effective date is required");

            // Get current salary and deactivate it
            var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(dto.EmployeeId);
            if (currentSalary != null)
            {
                // End the previous salary
                currentSalary.EndDate = dto.EffectiveDate.AddDays(-1);
                currentSalary.IsCurrent = false;
                await _employeeRepository.UpdateSalaryAsync(currentSalary);
            }

            // Create new salary
            var newSalary = new EmployeeSalary
            {
                Id = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                Amount = dto.Amount,
                EffectiveDate = dto.EffectiveDate,
                EndDate = null,
                IsCurrent = true,
                Reason = dto.Reason,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _employeeRepository.AddSalaryAsync(newSalary);
            return MapSalaryToDto(created);
        }

        public async Task<EmployeeSalaryDto?> GetSalaryAtDateAsync(Guid employeeId, DateTime date)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            var salary = await _employeeRepository.GetSalaryAtDateAsync(employeeId, date);
            return salary != null ? MapSalaryToDto(salary) : null;
        }

        public async Task<IEnumerable<EmployeeSalaryDto>> GetSalariesInDateRangeAsync(
            Guid employeeId, DateTime startDate, DateTime endDate)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            if (startDate > endDate)
                throw new Exception("Start date cannot be after end date");

            var salaries = await _employeeRepository.GetSalariesInDateRangeAsync(employeeId, startDate, endDate);
            return salaries
                .OrderByDescending(s => s.EffectiveDate)
                .Select(MapSalaryToDto)
                .ToList();
        }

        #endregion

        #region Salary Details & Reports

        public async Task<EmployeeDetailDto> GetEmployeeWithSalaryHistoryAsync(Guid employeeId)
        {
            return await GetEmployeeDetailAsync(employeeId);
        }

        public async Task<(IEnumerable<EmployeeSalaryDto>, int)> GetSalaryHistoryPagedAsync(
            Guid employeeId, int pageNumber, int pageSize)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            var (salaries, total) = await _employeeRepository.GetSalaryHistoryPagedAsync(employeeId, pageNumber, pageSize);
            var dtos = salaries
                .OrderByDescending(s => s.EffectiveDate)
                .Select(MapSalaryToDto)
                .ToList();

            return (dtos, total);
        }

        public async Task<decimal?> GetTotalMonthlyPayrollAsync(Guid residenceId)
        {
            return await _employeeRepository.GetTotalMonthlyPayrollByResidenceAsync(residenceId);
        }

        public async Task<decimal?> GetAverageSalaryByPositionAsync(Guid residenceId, string position)
        {
            return await _employeeRepository.GetAverageSalaryByPositionAsync(residenceId, position);
        }

        public async Task<IEnumerable<EmployeeSummaryDto>> GetPayrollSummaryAsync(Guid residenceId)
        {
            var employees = await _employeeRepository.GetByResidenceAsync(residenceId);
            var summaries = new List<EmployeeSummaryDto>();

            foreach (var employee in employees)
            {
                var currentSalary = await _employeeRepository.GetCurrentSalaryByEmployeeAsync(employee.Id);
                var salaryHistory = await _employeeRepository.GetSalaryHistoryByEmployeeAsync(employee.Id);

                summaries.Add(new EmployeeSummaryDto
                {
                    Id = employee.Id,
                    FullName = employee.GetFullName(),
                    Position = employee.Position,
                    CurrentSalary = currentSalary?.Amount,
                    SalaryEffectiveDate = currentSalary?.EffectiveDate,
                    Status = employee.Status.ToString(),
                    SalaryHistoryCount = salaryHistory.Count()
                });
            }

            return summaries;
        }

        #endregion

        #region Helper Methods

        private EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                ResidenceId = employee.ResidenceId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = employee.GetFullName(),
                Position = employee.Position,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                EndDate = employee.EndDate,
                Status = (int)employee.Status,
                Notes = employee.Notes,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }

        private EmployeeSalaryDto MapSalaryToDto(EmployeeSalary salary)
        {
            return new EmployeeSalaryDto
            {
                Id = salary.Id,
                EmployeeId = salary.EmployeeId,
                Amount = salary.Amount,
                EffectiveDate = salary.EffectiveDate,
                EndDate = salary.EndDate,
                IsCurrent = salary.IsCurrent,
                PeriodDisplay = salary.GetPeriodDisplay(),
                Reason = salary.Reason,
                Notes = salary.Notes,
                CreatedAt = salary.CreatedAt
            };
        }

        #endregion
    }
}
