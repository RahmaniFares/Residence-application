using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Comment on a post
    /// </summary>
    public class PostComment : BaseEntity
    {
        /// <summary>
        /// Post ID
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Author/Creator ID
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Comment text
        /// </summary>
        public string Content { get; set; } = string.Empty;

        // Navigation properties
        public Post Post { get; set; } = null!;
        public Resident Author { get; set; } = null!;
    }
}
