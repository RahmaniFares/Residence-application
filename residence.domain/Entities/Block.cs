using residence.domain.Common;
using System;
using System.Collections.Generic;

namespace residence.domain.Entities
{
    /// <summary>
    /// Residential block/unit representation
    /// </summary>
    public class Block : BaseEntity
    {
        /// <summary>
        /// Block name/identifier (A, B, C, D, E)
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Cost-sharing coefficient for this block
        /// Used to allocate shared expenses proportionally
        /// </summary>
        public decimal Coefficient { get; set; }

        /// <summary>
        /// Associated residence
        /// </summary>
        public Guid ResidenceId { get; set; }

        // Navigation properties
        /// <summary>
        /// Expenses allocated to this block
        /// </summary>
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
