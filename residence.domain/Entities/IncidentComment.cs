using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Comment on an incident
    /// </summary>
    public class IncidentComment : BaseEntity
    {
        /// <summary>
        /// Incident ID
        /// </summary>
        public Guid IncidentId { get; set; }

        /// <summary>
        /// Author/Creator ID
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Comment text
        /// </summary>
        public string Text { get; set; } = string.Empty;

        // Navigation property
        public Incident Incident { get; set; } = null!;
    }
}
