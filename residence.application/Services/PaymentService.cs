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
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
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
            payment.UpdatedAt
        );
    }
}
