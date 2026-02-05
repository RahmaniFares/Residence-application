using residence.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{

    /// <summary>
    /// Community post
    /// </summary>
    public class Post : BaseEntity
    {
        /// <summary>
        /// Post author ID
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Post content/text
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Image URL (optional)
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// GIF URL (optional)
        /// </summary>
        public string? GifUrl { get; set; }

        // Navigation properties
        public Resident Author { get; set; } = null!;
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    }
}
