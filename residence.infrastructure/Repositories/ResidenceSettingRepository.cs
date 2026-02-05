using Microsoft.EntityFrameworkCore;
using residence.application.Repositories;
using residence.domain.Entities;
using residence.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.infrastructure.Repositories
{
    public class ResidenceSettingRepository : IResidenceSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public ResidenceSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ResidenceSettings?> GetByIdAsync(Guid id)
        {
            return await _context.ResidenceSettings.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<ResidenceSettings?> GetByResidenceIdAsync(Guid id)
        {
            return await _context.ResidenceSettings.FirstOrDefaultAsync(r => r.ResidenceId == id);
        }

        public async Task<ResidenceSettings> CreateAsync(ResidenceSettings settings)
        {
            settings.CreatedAt = DateTime.UtcNow;
            settings.UpdatedAt = DateTime.UtcNow;
            _context.ResidenceSettings.Add(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<ResidenceSettings> UpdateAsync(ResidenceSettings settings)
        {
            settings.UpdatedAt = DateTime.UtcNow;
            _context.ResidenceSettings.Update(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var settings = await GetByIdAsync(id);
            if (settings is null)
                return false;

            _context.ResidenceSettings.Remove(settings);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Residences.AnyAsync(r => r.Id == id);
        }
    }
}
