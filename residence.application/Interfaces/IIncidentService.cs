using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Incident service interface
/// </summary>
public interface IIncidentService
{
    Task<IncidentDto> CreateIncidentAsync(Guid residenceId, CreateIncidentDto dto);
    Task<IncidentDto> GetIncidentByIdAsync(Guid id);
    Task<IncidentDto> UpdateIncidentAsync(Guid id, UpdateIncidentDto dto);
    Task DeleteIncidentAsync(Guid id);
    Task<PagedResultDto<IncidentDto>> GetIncidentsByResidenceAsync(Guid residenceId, PaginationDto pagination);
    Task<PagedResultDto<IncidentDto>> GetIncidentsByResidentAsync(Guid residentId, PaginationDto pagination);
    Task<IncidentCommentDto> AddCommentAsync(Guid incidentId, CreateIncidentCommentDto dto);
    Task<PagedResultDto<IncidentCommentDto>> GetCommentsAsync(Guid incidentId, PaginationDto pagination);
}
