using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Residence settings and configuration
    /// </summary>
    public class ResidenceSettings : BaseEntity
    {
        /// <summary>
        /// Name of the residence
        /// </summary>
        public string ResidenceName { get; set; } = string.Empty;

        /// <summary>
        /// Physical location/place
        /// </summary>
        public string ResidencePlace { get; set; } = string.Empty;

        /// <summary>
        /// Initial budget for the residence
        /// </summary>
        public decimal InitialBudget { get; set; }

        // Navigation property
        public Residence Residence { get; set; } = null!;
    }
}
