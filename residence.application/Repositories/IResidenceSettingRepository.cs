using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.application.Repositories
{
    public interface IResidenceSettingRepository
    {
        Task<ResidenceSettings?> GetByIdAsync(Guid id);
        Task<ResidenceSettings?> GetByResidenceIdAsync(Guid id);

        Task<ResidenceSettings> CreateAsync(ResidenceSettings settings);
        Task<ResidenceSettings> UpdateAsync(ResidenceSettings settings);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
