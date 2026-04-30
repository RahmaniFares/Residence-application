using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Donation entity
/// </summary>
public class DonationRepository : Repository<Donation>, IDonationRepository
{
    public DonationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Donation?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
    }

    public override async Task<IEnumerable<Donation>> GetAllAsync()
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Donation>> GetByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => d.HouseId == houseId && !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Donation>> GetByDonorAsync(Guid donorId)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => d.DonorId == donorId && !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Donation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => d.DonationDate >= startDate && d.DonationDate <= endDate && !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(d => d.HouseId == houseId && !d.IsDeleted)
            .SumAsync(d => d.Amount);
    }

    public async Task<Donation?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
    }

    public new async Task<Donation> AddAsync(Donation donation)
    {
        donation.CreatedAt = DateTime.UtcNow;
        donation.IsDeleted = false;
        _dbSet.Add(donation);
        await _context.SaveChangesAsync();

        // Reload to include navigations
        await _context.Entry(donation).ReloadAsync();
        return donation;
    }

    public new async Task<Donation> UpdateAsync(Donation donation)
    {
        donation.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(donation);
        await _context.SaveChangesAsync();

        // Reload to include navigations
        await _context.Entry(donation).ReloadAsync();
        return donation;
    }

    public async Task<decimal> GetTotalByDonorAsync(Guid donorId)
    {
        return await _dbSet
            .Where(d => d.DonorId == donorId && !d.IsDeleted)
            .SumAsync(d => d.Amount);
    }

    public async Task<int> GetCountByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(d => d.HouseId == houseId && !d.IsDeleted)
            .CountAsync();
    }

    public async Task<decimal> GetAverageByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(d => d.HouseId == houseId && !d.IsDeleted)
            .AverageAsync(d => d.Amount);
    }

    public async Task<IEnumerable<Donation>> GetRecentAsync(int count = 10)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Donation>> GetWithHouseDetailsAsync(Guid houseId)
    {
        return await _dbSet
            .Include(d => d.House)
            .Include(d => d.Donor)
            .Where(d => d.HouseId == houseId && !d.IsDeleted)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();
    }
}
