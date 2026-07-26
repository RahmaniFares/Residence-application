using System;

namespace residence.application.DTOs
{
    /// <summary>
    /// DTO for creating a User-House relationship
    /// </summary>
    public class CreateUserHouseDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// House ID
        /// </summary>
        public Guid HouseId { get; set; }

        /// <summary>
        /// Notes about the assignment
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for updating a User-House relationship
    /// </summary>
    public class UpdateUserHouseDto
    {
        /// <summary>
        /// Notes about the assignment
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for User-House relationship response
    /// </summary>
    public class UserHouseDto
    {
        /// <summary>
        /// Relationship ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// House ID
        /// </summary>
        public Guid HouseId { get; set; }

        /// <summary>
        /// Assignment date
        /// </summary>
        public DateTime AssignedDate { get; set; }

        /// <summary>
        /// Notes about the assignment
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// User details
        /// </summary>
        public UserHouseSummaryDto? User { get; set; }

        /// <summary>
        /// House details
        /// </summary>
        public HouseUserSummaryDto? House { get; set; }

        /// <summary>
        /// Created at timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Summary of User for UserHouse response
    /// </summary>
    public class UserHouseSummaryDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }

    /// <summary>
    /// Summary of House for UserHouse response
    /// </summary>
    public class HouseUserSummaryDto
    {
        public Guid Id { get; set; }
        public string Block { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for getting user's houses
    /// </summary>
    public class UserHousesResponseDto
    {
        /// <summary>
        /// List of houses for the user
        /// </summary>
        public ICollection<HouseDetailForUserDto> Houses { get; set; } = new List<HouseDetailForUserDto>();

        /// <summary>
        /// Total count of houses
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// House details for user response
    /// </summary>
    public class HouseDetailForUserDto
    {
        public Guid Id { get; set; }
        public string Block { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public HouseStatus Status { get; set; } = HouseStatus.Vacant;
        public DateTime AssignedDate { get; set; }
        public string? Notes { get; set; }
    }
}
