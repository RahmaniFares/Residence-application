using residence.domain.Common;
using residence.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Maintenance incident/request
    /// </summary>
    public class Incident : BaseEntity
    {
        /// <summary>
        /// Incident title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Incident category (Plomberie, Électricité, Sécurité, ClimatisationChauffage, Ascenseur, Autre)
        /// </summary>
        public IncidentCategory Category { get; set; } = IncidentCategory.Autre;

        /// <summary>
        /// Detailed description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Current status
        /// </summary>
        public IncidentStatus Status { get; set; } = IncidentStatus.Open;

        /// <summary>
        /// Priority level
        /// </summary>
        public IncidentPriority Priority { get; set; } = IncidentPriority.Medium;

        /// <summary>
        /// Resident ID who reported
        /// </summary>
        public Guid ResidentId { get; set; }

        /// <summary>
        /// Associated house ID
        /// </summary>
        public Guid? HouseId { get; set; }

        // Navigation properties
        public Resident Resident { get; set; } = null!;
        public House? House { get; set; }
        public ICollection<IncidentComment> Comments { get; set; } = new List<IncidentComment>();
    }
}
