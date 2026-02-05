using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

public class ResidenceRepository : IResidenceRepository
{
    private readonly ApplicationDbContext _context;

    public ResidenceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Residence>> GetAllAsync()
    {
        return await _context.Residences.ToListAsync();
    }

    public async Task<Residence?> GetByIdAsync(Guid id)
    {
        return await _context.Residences.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Residence> CreateAsync(Residence residence)
    {
        residence.CreatedAt = DateTime.UtcNow;
        residence.UpdatedAt = DateTime.UtcNow;
        _context.Residences.Add(residence);
        await _context.SaveChangesAsync();
        return residence;
    }

    public async Task<Residence> UpdateAsync(Residence residence)
    {
        residence.UpdatedAt = DateTime.UtcNow;
        _context.Residences.Update(residence);
        await _context.SaveChangesAsync();
        return residence;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var residence = await GetByIdAsync(id);
        if (residence is null)
            return false;

        _context.Residences.Remove(residence);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Residences.AnyAsync(r => r.Id == id);
    }
}

