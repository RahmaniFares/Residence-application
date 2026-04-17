using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Payment entity
/// </summary>
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public override async Task<IEnumerable<Payment>> GetByResidenceAsync(Guid residenceId)
    {
        return await _dbSet
            .Include(p => p.Lines)
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByResidentAsync(Guid residentId)
    {
        return await _dbSet
            .Where(p => p.ResidentId == residentId && !p.IsDeleted)
            .Include(p => p.House)
            .Include(p => p.Resident)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.PeriodStart)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(p => p.HouseId == houseId && !p.IsDeleted)
            .Include(p => p.Resident)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.PeriodStart)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(Guid residenceId, PaymentStatus status)
    {
        return await _dbSet
            .Where(p => p.ResidenceId == residenceId && p.Status == status && !p.IsDeleted)
            .Include(p => p.House)
            .Include(p => p.Resident)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetOverdueAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(p => p.ResidenceId == residenceId && 
                   p.Status != PaymentStatus.Paid && 
                   p.PeriodEnd < DateTime.UtcNow && 
                   !p.IsDeleted)
            .Include(p => p.House)
            .Include(p => p.Resident)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPaidAsync(Guid residentId)
    {
        return await _dbSet
            .Where(p => p.ResidentId == residentId && p.Status == PaymentStatus.Paid && !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }

    public async Task<decimal> GetTotalPendingAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(p => p.ResidenceId == residenceId && p.Status == PaymentStatus.Pending && !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }
}
