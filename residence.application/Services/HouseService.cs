using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of House service
/// </summary>
public class HouseService : IHouseService
{
    private readonly IHouseRepository _houseRepository;

    public HouseService(IHouseRepository houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public async Task<HouseDto> CreateHouseAsync(Guid residenceId, CreateHouseDto dto)
    {
        var house = new House
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            CurrentResidentId = dto.ResidentId,
            Block = dto.Block,
            Unit = dto.Unit,
            Floor = dto.Floor,
            Status = 0, // Vacant
            CreatedAt = DateTime.UtcNow
        };

        var created = await _houseRepository.AddAsync(house);
        return MapToDto(created);
    }

    public async Task<HouseDto> GetHouseByIdAsync(Guid id)
    {
        var house = await _houseRepository.GetByIdAsync(id);
        if (house == null)
            throw new Exception("House not found");

        return MapToDto(house);
    }

    public async Task<HouseDetailDto> GetHouseDetailsAsync(Guid id)
    {
        var house = await _houseRepository.GetWithResidentsAsync(id);
        if (house == null)
            throw new Exception("House not found");

        return new HouseDetailDto(
            house.Id,
            house.Block,
            house.Unit,
            house.Floor,
            (residence.application.DTOs.HouseStatus)house.Status,
            house.CurrentResidentId,
            house.CurrentResident != null ? MapResidentToDto(house.CurrentResident) : null,
            house.Residents.Count,
            house.CreatedAt,
            house.UpdatedAt
        );
    }

    public async Task<HouseDto> UpdateHouseAsync(Guid id, UpdateHouseDto dto)
    {
        var house = await _houseRepository.GetByIdAsync(id);
        if (house == null)
            throw new Exception("House not found");

        house.Block = dto.Block;
        house.Unit = dto.Unit;
        house.Floor = dto.Floor;
        house.CurrentResidentId = dto.ResidentId;
        house.Status = (domain.Enums.HouseStatus)dto.Status;
        house.UpdatedAt = DateTime.UtcNow;

        await _houseRepository.UpdateAsync(house);

        return MapToDto(house);
    }

    public async Task DeleteHouseAsync(Guid id)
    {
        var house = await _houseRepository.GetByIdAsync(id);
        if (house == null)
            throw new Exception("House not found");

        await _houseRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<HouseDto>> GetHousesByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var houses = await _houseRepository.GetByResidenceAsync(residenceId);
        
        var total = houses.Count();
        var items = houses
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<HouseDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private HouseDto MapToDto(House house)
    {
        return new HouseDto(
            house.Id,
            house.Block,
            house.Unit,
            house.Floor,
            (residence.application.DTOs.HouseStatus)house.Status,
            house.CurrentResidentId,
            house.CreatedAt,
            house.UpdatedAt
        );
    }

    private ResidentDto MapResidentToDto(Resident resident)
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
            (residence.application.DTOs.ResidentStatus)resident.Status,
            resident.MoveInDate,
            resident.MoveOutDate,
            resident.CreatedAt,
            resident.UpdatedAt
        );
    }
}
