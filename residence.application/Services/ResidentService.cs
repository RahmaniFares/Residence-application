using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of Resident service
/// </summary>
public class ResidentService : IResidentService
{
    private readonly IResidentRepository _residentRepository;

    public ResidentService(IResidentRepository residentRepository)
    {
        _residentRepository = residentRepository;
    }

    public async Task<ResidentDto> CreateResidentAsync(Guid residenceId, CreateResidentDto dto)
    {
        var resident = new Resident
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            UserId = dto.UserId,
            HouseId = dto.HouseId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            Status = domain.Enums.ResidentStatus.Active, // Active
            MoveInDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _residentRepository.AddAsync(resident);
        return MapToDto(created);
    }

    public async Task<ResidentDto> GetResidentByIdAsync(Guid id)
    {
        var resident = await _residentRepository.GetByIdAsync(id);
        if (resident == null)
            throw new Exception("Resident not found");

        return MapToDto(resident);
    }

    public async Task<ResidentDto> UpdateResidentAsync(Guid id, UpdateResidentDto dto)
    {
        var resident = await _residentRepository.GetByIdAsync(id);
        if (resident == null)
            throw new Exception("Resident not found");

        resident.HouseId = dto.HouseId;
        resident.UserId = dto.UserId;
        resident.FirstName = dto.FirstName;
        resident.LastName = dto.LastName;
        resident.PhoneNumber = dto.PhoneNumber;
        resident.Address = dto.Address;
        resident.BirthDate = dto.BirthDate;
        resident.Status = (domain.Enums.ResidentStatus)(int)dto.Status;
        resident.UpdatedAt = DateTime.UtcNow;

        await _residentRepository.UpdateAsync(resident);

        return MapToDto(resident);
    }

    public async Task DeleteResidentAsync(Guid id)
    {
        var resident = await _residentRepository.GetByIdAsync(id);
        if (resident == null)
            throw new Exception("Resident not found");

        await _residentRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<ResidentDto>> GetResidentsByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var residents = await _residentRepository.GetByResidenceAsync(residenceId);
        
        var total = residents.Count();
        var items = residents
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<ResidentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PagedResultDto<ResidentDto>> GetResidentsByHouseAsync(Guid houseId, PaginationDto pagination)
    {
        var residents = await _residentRepository.GetByHouseAsync(houseId);
        
        var total = residents.Count();
        var items = residents
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<ResidentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private ResidentDto MapToDto(Resident resident)
    {
        return new ResidentDto(
            resident.Id,
            resident.UserId,
            resident.HouseId,
            resident.FirstName,
            resident.LastName,
            resident.Email,
            resident.PhoneNumber,
            resident.Address,
            resident.BirthDate,
            (ResidentStatus)resident.Status,
            resident.MoveInDate,
            resident.MoveOutDate,
            resident.CreatedAt,
            resident.UpdatedAt
        );
    }
}
