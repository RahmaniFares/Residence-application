using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

using residence.domain.Enums;

namespace residence.application.Services;

/// <summary>
/// Implementation of House service
/// </summary>
public class HouseService : IHouseService
{
    private readonly IHouseRepository _houseRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IRappelRepository _rappelRepository;
    private readonly ITarifHistoryRepository _tarifHistoryRepository;

    public HouseService(
        IHouseRepository houseRepository,
        IPaymentRepository paymentRepository,
        IRappelRepository rappelRepository,
        ITarifHistoryRepository tarifHistoryRepository)
    {
        _houseRepository = houseRepository;
        _paymentRepository = paymentRepository;
        _rappelRepository = rappelRepository;
        _tarifHistoryRepository = tarifHistoryRepository;
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
            .OrderBy(h => h.Block)
            .OrderBy(h => h.Floor)
            .OrderBy(h => h.Unit)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<HouseDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PagedResultDto<HouseDetailDto>> GetHousesByResidenceWithDetailsAsync(Guid residenceId, PaginationDto pagination)
    {
        var houses = await _houseRepository.GetByResidenceWithDetailsAsync(residenceId);

        var total = houses.Count();
        var items = houses
            .OrderBy(h => h.Unit)
            .OrderBy(h => h.Floor)
            .OrderBy(h => h.Block)
            
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(h => new HouseDetailDto(
                h.Id,
                h.Block,
                h.Unit,
                h.Floor,
                (residence.application.DTOs.HouseStatus)h.Status,
                h.CurrentResidentId,
                h.CurrentResident != null ? MapResidentToDto(h.CurrentResident) : null,
                h.Residents.Count,
                h.CreatedAt,
                h.UpdatedAt
            ))
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<HouseDetailDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<HouseFinancialStatementDto> GetHouseFinancialStatementAsync(Guid id)
    {
        var house = await _houseRepository.GetByIdAsync(id);
        if (house == null)
            throw new Exception("House not found");

        var payments = await _paymentRepository.GetByHouseAsync(id);
        var rappels = await _rappelRepository.GetByHouseAsync(id);

        var allTarifs = await _tarifHistoryRepository.GetHistoryByResidenceIdAsync(house.ResidenceId);
        var orderedTarifs = allTarifs.OrderBy(t => t.EffectiveDate).ToList();

        var amountsPaid = BuildPaidAmountsByMonth(payments);


        var statement = new HouseFinancialStatementDto
        {
            HouseId = id,
            TotalRappelPaid = rappels.Sum(r => r.Amount) // As you mentioned, rappel model contains only paid amount
        };



        // Calculate total missing amount by comparing paid amounts to the active tariff for each month
        decimal GetActiveTarifAmount(int year, int month)
        {
            return orderedTarifs
                .Where(t => t.EffectiveDate <= new DateTime(year, month, DateTime.DaysInMonth(year, month)))
                .OrderByDescending(t => t.EffectiveDate)
                .Select(t => t.NewAmount)
                .FirstOrDefault();
        }

        var totalMissingAmount = amountsPaid
            .Select(kv =>
            {
                var (year, month) = kv.Key;
                decimal paid = kv.Value;
                decimal activeTarif = GetActiveTarifAmount(year, month);
                decimal diff = activeTarif - paid;
                return diff > 0m ? diff : 0m;
            })
            .Sum();

        statement.TotalRappelToPay = totalMissingAmount - statement.TotalRappelPaid;
        if (statement.TotalRappelToPay < 0m)
            statement.TotalRappelToPay = 0m;

        return statement;
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

    /// <summary>
    /// Builds a dictionary of paid amounts per month from payment lines.
    /// Optimized to avoid repeated dictionary lookups using AddOrUpdate pattern.
    /// </summary>
    private Dictionary<(int Year, int Month), decimal> BuildPaidAmountsByMonth(IEnumerable<Payment> payments)
    {
        var amountsPaid = new Dictionary<(int Year, int Month), decimal>();

        foreach (var payment in payments.Where(p => p.Status == residence.domain.Enums.PaymentStatus.Paid))
        {
            foreach (var line in payment.Lines)
            {
                AddMonthRangeToDict(amountsPaid, line.FromYear, line.FromMonth, line.ToYear, line.ToMonth, line.Tarif);
            }
        }

        return amountsPaid;
    }

    /// <summary>
    /// Adds a month range to the dictionary with the specified tariff amount.
    /// More efficient than repeated ContainsKey checks.
    /// </summary>
    private void AddMonthRangeToDict(Dictionary<(int Year, int Month), decimal> dict, int fromYear, int fromMonth, int toYear, int toMonth, decimal tarif)
    {
        for (int y = fromYear; y <= toYear; y++)
        {
            int startMonth = (y == fromYear) ? fromMonth : 1;
            int endMonth = (y == toYear) ? toMonth : 12;

            for (int m = startMonth; m <= endMonth; m++)
            {
                var key = (y, m);
                if (dict.TryGetValue(key, out var existingAmount))
                {
                    dict[key] = existingAmount + tarif;
                }
                else
                {
                    dict[key] = tarif;
                }
            }
        }
    }
}
