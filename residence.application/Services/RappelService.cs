using residence.application.DTOs;
using residence.application.Interfaces;
using residence.application.Repositories;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.application.Services;

public class RappelService : IRappelService
{
    private readonly IRappelRepository _rappelRepository;

    public RappelService(IRappelRepository rappelRepository)
    {
        _rappelRepository = rappelRepository;
    }

    public async Task<RappelDto> CreateRappelAsync(Guid residenceId, CreateRappelDto dto)
    {
        var rappel = new Rappel
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            HouseId = dto.HouseId,
            Amount = dto.Amount,
            Status = RappelStatus.Unpaid,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _rappelRepository.AddAsync(rappel);
        return MapToDto(created);
    }

    public async Task<RappelDto> GetRappelByIdAsync(Guid id)
    {
        var rappel = await _rappelRepository.GetByIdAsync(id);
        if (rappel == null)
            throw new Exception("Rappel not found");

        return MapToDto(rappel);
    }

    public async Task<RappelDto> UpdateRappelAsync(Guid id, UpdateRappelDto dto)
    {
        var rappel = await _rappelRepository.GetByIdAsync(id);
        if (rappel == null)
            throw new Exception("Rappel not found");

        rappel.Status = dto.Status;
        rappel.PaymentDate = dto.PaymentDate;
        rappel.Notes = dto.Notes;
        rappel.UpdatedAt = DateTime.UtcNow;

        await _rappelRepository.UpdateAsync(rappel);

        return MapToDto(rappel);
    }

    public async Task DeleteRappelAsync(Guid id)
    {
        var rappel = await _rappelRepository.GetByIdAsync(id);
        if (rappel == null)
            throw new Exception("Rappel not found");

        await _rappelRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<RappelDto>> GetRappelsByHouseAsync(Guid houseId, PaginationDto pagination)
    {
        var rappels = await _rappelRepository.GetByHouseAsync(houseId);
        return Paginate(rappels, pagination);
    }

    public async Task<PagedResultDto<RappelDto>> GetRappelsByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var rappels = await _rappelRepository.GetByResidenceAsync(residenceId);
        return Paginate(rappels, pagination);
    }

    private PagedResultDto<RappelDto> Paginate(IEnumerable<Rappel> rappels, PaginationDto pagination)
    {
        var total = rappels.Count();
        var items = rappels
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<RappelDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private RappelDto MapToDto(Rappel rappel)
    {
        return new RappelDto(
            rappel.Id,
            rappel.HouseId,
            rappel.Amount,
            rappel.Status,
            rappel.PaymentDate,
            rappel.Notes,
            rappel.CreatedAt,
            rappel.UpdatedAt
        );
    }
}
