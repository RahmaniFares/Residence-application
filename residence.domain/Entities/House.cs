using residence.domain.Common;
using residence.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    public class House : BaseEntity
    {
        /// <summary>
        /// Block identifier (e.g., A, B, C)
        /// </summary>
        public string Block { get; set; } = string.Empty;

        /// <summary>
        /// Unit/Apartment number
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Floor number
        /// </summary>
        public string? Floor { get; set; }

        /// <summary>
        /// Current occupancy status
        /// </summary>
        public HouseStatus Status { get; set; } = HouseStatus.Vacant;

        /// <summary>
        /// Current resident ID (if occupied)
        /// </summary>
        public Guid? CurrentResidentId { get; set; }

        // Navigation properties
        public Resident? CurrentResident { get; set; }
        public ICollection<Resident> Residents { get; set; } = new List<Resident>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }

}
