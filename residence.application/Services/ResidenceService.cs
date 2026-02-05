using residence.application.DTOs;
using residence.application.Interfaces;
using residence.application.Repositories;
using residence.domain.Entities;

namespace residence.application.Services;

/// <summary>
/// Implementation of Residence service
/// </summary>
public class ResidenceService : IResidenceService
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IResidenceSettingRepository _residenceSettingRepository;


    public ResidenceService(IResidenceRepository residenceRepository,IResidenceSettingRepository residenceSettingRepository)
    {
        _residenceRepository = residenceRepository;
        _residenceSettingRepository = residenceSettingRepository;
    }

    public async Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto dto)
    {
        var residence = new Residence
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _residenceRepository.CreateAsync(residence);
        
        // Create default settings
        var settings = new ResidenceSettings
        {
            Id = Guid.NewGuid(),
            ResidenceId = created.Id,
            ResidenceName = created.Name,
            ResidencePlace = created.Address,
            InitialBudget = 0m,
            CreatedAt = DateTime.UtcNow
        };

        await _residenceSettingRepository.CreateAsync(settings);
        

        return MapToDto(created);
    }

    public async Task<ResidenceDto> GetResidenceByIdAsync(Guid id)
    {
        var residence = await _residenceRepository.GetByIdAsync(id);
        if (residence == null)
            throw new Exception("Residence not found");

        return MapToDto(residence);
    }

    public async Task<ResidenceDto> UpdateResidenceAsync(Guid id, UpdateResidenceDto dto)
    {
        var residence = await _residenceRepository.GetByIdAsync(id);
        if (residence == null)
            throw new Exception("Residence not found");

        residence.Name = dto.Name;
        residence.Address = dto.Address;
        residence.City = dto.City;
        residence.State = dto.State;
        residence.ZipCode = dto.ZipCode;
        residence.Description = dto.Description;
        residence.UpdatedAt = DateTime.UtcNow;

        await _residenceRepository.UpdateAsync(residence);

        return MapToDto(residence);
    }

    public async Task DeleteResidenceAsync(Guid id)
    {
        var residence = await _residenceRepository.GetByIdAsync(id);
        if (residence == null)
            throw new Exception("Residence not found");

        await _residenceRepository.DeleteAsync(id);
    }

    public async Task<ResidenceSettingsDto> GetSettingsAsync(Guid residenceId)
    {
        var settings = await _residenceSettingRepository.GetByResidenceIdAsync(residenceId);
        
        if (settings == null)
            throw new Exception("Settings not found");

        return MapSettingsToDto(settings);
    }

    public async Task<ResidenceSettingsDto> UpdateSettingsAsync(Guid residenceId, ResidenceSettingsDto dto)
    {
        var settings = await _residenceSettingRepository.GetByResidenceIdAsync(residenceId);
        
        if (settings == null)
            throw new Exception("Settings not found");

        settings.ResidenceName = dto.ResidenceName;
        settings.ResidencePlace = dto.ResidencePlace;
        settings.InitialBudget = dto.InitialBudget;
        settings.UpdatedAt = DateTime.UtcNow;

        await _residenceSettingRepository.UpdateAsync(settings);

        return MapSettingsToDto(settings);
    }

    private ResidenceDto MapToDto(Residence residence)
    {
        return new ResidenceDto(
            residence.Id,
            residence.Name,
            residence.Address,
            residence.City,
            residence.State,
            residence.ZipCode,
            residence.Description,
            residence.CreatedAt,
            residence.UpdatedAt
        );
    }

    private ResidenceSettingsDto MapSettingsToDto(ResidenceSettings settings)
    {
        return new ResidenceSettingsDto(
            settings.Id,
            settings.ResidenceName,
            settings.ResidencePlace,
            settings.InitialBudget
        );
    }
}
