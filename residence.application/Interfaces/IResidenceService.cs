using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Residence service interface
/// </summary>
public interface IResidenceService
{
    Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto dto);
    Task<ResidenceDto> GetResidenceByIdAsync(Guid id);
    Task<ResidenceDto> UpdateResidenceAsync(Guid id, UpdateResidenceDto dto);
    Task DeleteResidenceAsync(Guid id);
    Task<ResidenceSettingsDto> GetSettingsAsync(Guid residenceId);
    Task<ResidenceSettingsDto> UpdateSettingsAsync(Guid residenceId, ResidenceSettingsDto dto);
}
