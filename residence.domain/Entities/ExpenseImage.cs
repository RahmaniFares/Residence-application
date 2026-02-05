using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Expense receipt/supporting image
    /// </summary>
    public class ExpenseImage : BaseEntity
    {
        /// <summary>
        /// Associated expense ID
        /// </summary>
        public Guid ExpenseId { get; set; }

        /// <summary>
        /// Image URL (stored in Supabase Storage)
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation property
        public Expense Expense { get; set; } = null!;
    }
}
