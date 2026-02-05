using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of Incident service
/// </summary>
public class IncidentService : IIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IIncidentCommentRepository _incidentCommentRepository;

    public IncidentService(IIncidentRepository incidentRepository, IIncidentCommentRepository incidentCommentRepository)
    {
        _incidentRepository = incidentRepository;
        _incidentCommentRepository = incidentCommentRepository;
    }

    public async Task<IncidentDto> CreateIncidentAsync(Guid residenceId, CreateIncidentDto dto)
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            Title = dto.Title,
            Category = (residence.domain.Enums.IncidentCategory)dto.Category,
            Description = dto.Description,
            Status = 0, // Open
            Priority = (residence.domain.Enums.IncidentPriority)dto.Priority,
            ResidentId = dto.ResidentId,
            HouseId = dto.HouseId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _incidentRepository.AddAsync(incident);
        return MapToDto(created);
    }

    public async Task<IncidentDto> GetIncidentByIdAsync(Guid id)
    {
        var incident = await _incidentRepository.GetWithCommentsAsync(id);
        if (incident == null)
            throw new Exception("Incident not found");

        return MapToDto(incident);
    }

    public async Task<IncidentDto> UpdateIncidentAsync(Guid id, UpdateIncidentDto dto)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        if (incident == null)
            throw new Exception("Incident not found");

        incident.Title = dto.Title;
        incident.Category = (residence.domain.Enums.IncidentCategory)dto.Category;
        incident.Description = dto.Description;
        incident.Status = (residence.domain.Enums.IncidentStatus)(int)dto.Status;
        incident.Priority = (residence.domain.Enums.IncidentPriority)dto.Priority;
        incident.UpdatedAt = DateTime.UtcNow;

        await _incidentRepository.UpdateAsync(incident);

        return await GetIncidentByIdAsync(id);
    }

    public async Task DeleteIncidentAsync(Guid id)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);
        if (incident == null)
            throw new Exception("Incident not found");

        await _incidentRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<IncidentDto>> GetIncidentsByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var incidents = await _incidentRepository.GetByResidenceAsync(residenceId);
        
        var total = incidents.Count();
        var items = incidents
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<IncidentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PagedResultDto<IncidentDto>> GetIncidentsByResidentAsync(Guid residentId, PaginationDto pagination)
    {
        var incidents = await _incidentRepository.GetByResidentAsync(residentId);
        
        var total = incidents.Count();
        var items = incidents
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<IncidentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<IncidentCommentDto> AddCommentAsync(Guid incidentId, CreateIncidentCommentDto dto)
    {
        var incident = await _incidentRepository.GetByIdAsync(incidentId);
        if (incident == null)
            throw new Exception("Incident not found");

        var comment = new IncidentComment
        {
            Id = Guid.NewGuid(),
            ResidenceId = incident.ResidenceId,
            IncidentId = incidentId,
            AuthorId =  incident.ResidentId, // Default to incident reporter
            Text = dto.Text,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _incidentCommentRepository.AddAsync(comment);
        return MapCommentToDto(created);
    }

    public async Task<PagedResultDto<IncidentCommentDto>> GetCommentsAsync(Guid incidentId, PaginationDto pagination)
    {
        var comments = await _incidentCommentRepository.GetByIncidentAsync(incidentId);
        
        var total = comments.Count();
        var items = comments
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapCommentToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<IncidentCommentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private IncidentDto MapToDto(Incident incident)
    {
        return new IncidentDto(
            incident.Id,
            incident.Title,
            (residence.application.DTOs.IncidentCategory)incident.Category,
            incident.Description,
            (residence.application.DTOs.IncidentStatus)incident.Status,
            (residence.application.DTOs.IncidentPriority)incident.Priority,
            incident.ResidentId,
            incident.HouseId,
            incident.Comments?.Count ?? 0,
            incident.CreatedAt,
            incident.UpdatedAt
        );
    }

    private IncidentCommentDto MapCommentToDto(IncidentComment comment)
    {
        return new IncidentCommentDto(
            comment.Id,
            comment.IncidentId,
            comment.AuthorId,
            comment.Text,
            comment.CreatedAt
        );
    }
}
