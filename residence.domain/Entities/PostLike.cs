using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Like on a post
    /// </summary>
    public class PostLike : BaseEntity
    {
        /// <summary>
        /// Post ID
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// User ID who liked
        /// </summary>
        public Guid UserId { get; set; }

        // Navigation property
        public Post Post { get; set; } = null!;
    }
}
