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
    /// Expense record
    /// </summary>
    public class Expense : BaseEntity
    {
        /// <summary>
        /// Expense title/label
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Expense type/category
        /// </summary>
        public ExpenseType Type { get; set; }

        /// <summary>
        /// Expense amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Expense date
        /// </summary>
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Detailed description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Associated block (if null, expense is shared across all blocks)
        /// </summary>
        public Guid? BlockId { get; set; }

        // Navigation properties
        public ICollection<ExpenseImage> Images { get; set; } = new List<ExpenseImage>();

        /// <summary>
        /// Associated block for this expense
        /// </summary>
        public Block? Block { get; set; }
    }
}
