using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.domain.Enums;

namespace residence.application.Services;

/// <summary>
/// Implementation of Payment service
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> CreatePaymentAsync(Guid residenceId, CreatePaymentDto dto)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            HouseId = dto.HouseId,
            ResidentId = dto.ResidentId,
            Amount = dto.Amount,
            Method = (domain.Enums.PaymentMethod)(int)dto.Method,
            Status = domain.Enums.PaymentStatus.Paid,
            PeriodStart = dto.PeriodStart,
            PeriodEnd = dto.PeriodEnd,
            PaymentDate = dto.PaymentDate ?? DateTime.UtcNow,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            Lines = dto.Lines?.Select(l => new PaymentLine
            {
                Id = Guid.NewGuid(),
                ResidenceId = residenceId,
                FromMonth = l.FromMonth,
                FromYear = l.FromYear,
                ToMonth = l.ToMonth,
                ToYear = l.ToYear,
                Tarif = l.Tarif,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? new List<PaymentLine>()
        };

        var created = await _paymentRepository.AddAsync(payment);
        return MapToDto(created);
    }

    public async Task<PaymentDto> GetPaymentByIdAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            throw new Exception("Payment not found");

        return MapToDto(payment);
    }

    public async Task<PaymentDto> UpdatePaymentAsync(Guid id, UpdatePaymentDto dto)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            throw new Exception("Payment not found");

        payment.Status = (domain.Enums.PaymentStatus)(int)dto.Status;
        payment.PaymentDate = dto.PaymentDate;
        payment.Notes = dto.Notes;
        payment.UpdatedAt = DateTime.UtcNow;
        payment.Amount = dto.Amount  ?? payment.Amount;
        payment.PeriodStart = dto.PeriodStart ?? payment.PeriodStart;
        payment.PeriodEnd = dto.PeriodEnd ?? payment.PeriodEnd;

        if (dto.Lines != null)
        {
            var newLines = dto.Lines.Select(l => new PaymentLine
            {
                Id = l.Id ?? Guid.NewGuid(),
                PaymentId = payment.Id,
                ResidenceId = payment.ResidenceId,
                FromMonth = l.FromMonth,
                FromYear = l.FromYear,
                ToMonth = l.ToMonth,
                ToYear = l.ToYear,
                Tarif = l.Tarif,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            payment.Lines.Clear();
            foreach (var line in newLines)
            {
                payment.Lines.Add(line);
            }
        }

         await _paymentRepository.UpdateAsync(payment);

        return MapToDto(payment);
    }

    public async Task DeletePaymentAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            throw new Exception("Payment not found");

        await _paymentRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<PaymentDto>> GetPaymentsByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var payments = await _paymentRepository.GetByResidenceAsync(residenceId);
        
        var total = payments.Count();
        var items = payments
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<PaymentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PagedResultDto<PaymentDto>> GetPaymentsByResidentAsync(Guid residentId, PaginationDto pagination)
    {
        var payments = await _paymentRepository.GetByResidentAsync(residentId);
        
        var total = payments.Count();
        var items = payments
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<PaymentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PagedResultDto<PaymentDto>> GetPaymentsByHouseAsync(Guid houseId, PaginationDto pagination)
    {
        var payments = await _paymentRepository.GetByHouseAsync(houseId);

        var total = payments.Count();
        var items = payments
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<PaymentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PaymentKpiDto> GetPaymentKpiAsync(Guid residenceId)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        return await GetPaymentKpiByDateRangeAsync(residenceId, startOfYear, now);
    }

    public async Task<PaymentKpiDto> GetPaymentKpiByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate)
    {
        var allPayments = await _paymentRepository.GetByResidenceAsync(residenceId);

        // Filter payments within the date range
        var filteredPayments = allPayments
            .Where(p => p.PeriodEnd >= startDate && p.PeriodStart <= endDate)
            .ToList();

        var paidPayments = filteredPayments
            .Where(p => p.Status == domain.Enums.PaymentStatus.Paid)
            .ToList();

        var pendingPayments = filteredPayments
            .Where(p => p.Status != domain.Enums.PaymentStatus.Paid)
            .ToList();

        var overduePayments = pendingPayments
            .Where(p => p.PeriodEnd < DateTime.UtcNow)
            .ToList();

        var totalPaidAmount = paidPayments.Sum(p => p.Amount);
        var totalPendingAmount = pendingPayments.Sum(p => p.Amount);
        var totalOverdueAmount = overduePayments.Sum(p => p.Amount);
        var totalExpectedAmount = totalPaidAmount + totalPendingAmount;

        var collectionRate = totalExpectedAmount > 0 
            ? Math.Round((totalPaidAmount / totalExpectedAmount) * 100, 2) 
            : 0;

        var averagePaymentAmount = paidPayments.Any() 
            ? Math.Round(totalPaidAmount / paidPayments.Count, 2) 
            : 0;

        return new PaymentKpiDto
        {
            ResidenceId = residenceId,
            TotalPaidAmount = totalPaidAmount,
            TotalPaidCount = paidPayments.Count,
            TotalPendingAmount = totalPendingAmount,
            TotalPendingCount = pendingPayments.Count,
            TotalOverdueAmount = totalOverdueAmount,
            TotalOverdueCount = overduePayments.Count,
            TotalExpectedAmount = totalExpectedAmount,
            OutstandingBalance = totalPendingAmount + totalOverdueAmount,
            CollectionRate = collectionRate,
            AveragePaymentAmount = averagePaymentAmount,
            PeriodStartDate = filteredPayments.Any() ? filteredPayments.Min(p => p.PeriodStart) : null,
            PeriodEndDate = filteredPayments.Any() ? filteredPayments.Max(p => p.PeriodEnd) : null,
            CalculatedAt = DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<MonthlyPaymentSummaryDto>> GetMonthlyPaymentSummaryAsync(Guid residenceId, int months = 12)
    {
        var allPayments = await _paymentRepository.GetByResidenceAsync(residenceId);
        var summaries = new List<MonthlyPaymentSummaryDto>();

        for (int i = months - 1; i >= 0; i--)
        {
            var date = DateTime.UtcNow.AddMonths(-i);
            var monthStart = new DateTime(date.Year, date.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var monthPayments = allPayments
                .Where(p => p.PeriodEnd >= monthStart && p.PeriodStart <= monthEnd)
                .ToList();

            var paidAmount = monthPayments.Where(p => p.Status == domain.Enums.PaymentStatus.Paid).Sum(p => p.Amount);
            var totalAmount = monthPayments.Sum(p => p.Amount);
            var pendingAmount = totalAmount - paidAmount;

            var collectionPercentage = totalAmount > 0 
                ? Math.Round((paidAmount / totalAmount) * 100, 2) 
                : 0;

            summaries.Add(new MonthlyPaymentSummaryDto
            {
                Year = date.Year,
                Month = date.Month,
                TotalExpected = totalAmount,
                TotalPaid = paidAmount,
                TotalPending = pendingAmount,
                TotalPayments = monthPayments.Count,
                PaidCount = monthPayments.Count(p => p.Status == domain.Enums.PaymentStatus.Paid),
                PendingCount = monthPayments.Count(p => p.Status != domain.Enums.PaymentStatus.Paid),
                CollectionPercentage = collectionPercentage
            });
        }

        return summaries;
    }

    public async Task<IEnumerable<PaymentTrendDto>> GetPaymentTrendAsync(Guid residenceId, DateTime startDate, DateTime endDate)
    {
        var allPayments = await _paymentRepository.GetByResidenceAsync(residenceId);

        var filteredPayments = allPayments
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && p.Status == domain.Enums.PaymentStatus.Paid)
            .OrderBy(p => p.PaymentDate)
            .ToList();

        var trends = new List<PaymentTrendDto>();
        decimal cumulativePaid = 0;

        foreach (var payment in filteredPayments)
        {
            cumulativePaid += payment.Amount;

            var dayPayments = filteredPayments
                .Where(p => p.PaymentDate?.Date == payment.PaymentDate?.Date)
                .ToList();

            var dayTotalPaid = dayPayments.Sum(p => p.Amount);

            var pendingForDay = allPayments
                .Where(p => p.PeriodEnd <= payment.PaymentDate && p.Status != domain.Enums.PaymentStatus.Paid)
                .Sum(p => p.Amount);

            var allAmount = allPayments
                .Where(p => p.PeriodEnd <= payment.PaymentDate)
                .Sum(p => p.Amount);

            var collectionRate = allAmount > 0 
                ? Math.Round((cumulativePaid / allAmount) * 100, 2) 
                : 0;

            trends.Add(new PaymentTrendDto
            {
                Date = payment.PaymentDate?.Date ?? DateTime.UtcNow.Date,
                AmountPaid = dayTotalPaid,
                AmountPending = pendingForDay,
                CumulativePaid = cumulativePaid,
                CollectionRate = collectionRate
            });
        }

        // Remove duplicates and keep only one entry per date
        return trends
            .GroupBy(t => t.Date)
            .Select(g => g.Last())
            .OrderBy(t => t.Date);
    }

    private PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto(
            payment.Id,
            payment.HouseId,
            payment.ResidentId,
            payment.Amount,
            (residence.application.DTOs.PaymentMethod)payment.Method,
            payment.PeriodStart,
            payment.PeriodEnd,
            payment.PaymentDate,
            (residence.application.DTOs.PaymentStatus)payment.Status,
            payment.Notes,
            payment.CreatedAt,
            payment.UpdatedAt,
            payment.Lines?.Select(l => new PaymentLineDto(
                l.Id,
                l.PaymentId,
                l.FromMonth,
                l.FromYear,
                l.ToMonth,
                l.ToYear,
                l.Tarif,
                l.CreatedAt,
                l.UpdatedAt)).ToList()
        );
    }
}