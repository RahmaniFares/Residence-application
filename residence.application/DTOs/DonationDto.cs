using System;

namespace residence.application.DTOs
{
    /// <summary>
    /// Create Donation DTO
    /// </summary>
    public class CreateDonationDto
    {
        /// <summary>
        /// House ID (optional)
        /// </summary>
        public Guid? HouseId { get; set; }

        /// <summary>
        /// Donor/Contributor ID (optional)
        /// </summary>
        public Guid? DonorId { get; set; }

        /// <summary>
        /// Donation amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of donation
        /// </summary>
        public DateTime DonationDate { get; set; }

        /// <summary>
        /// Description or purpose of donation
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// Update Donation DTO
    /// </summary>
    public class UpdateDonationDto
    {
        /// <summary>
        /// House ID (optional)
        /// </summary>
        public Guid? HouseId { get; set; }

        /// <summary>
        /// Donor/Contributor ID (optional)
        /// </summary>
        public Guid? DonorId { get; set; }

        /// <summary>
        /// Donation amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of donation
        /// </summary>
        public DateTime DonationDate { get; set; }

        /// <summary>
        /// Description or purpose of donation
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// Donation DTO (Response)
    /// </summary>
    public class DonationDto
    {
        public DonationDto() { }

        public DonationDto(
            Guid id,
            Guid? houseId,
            Guid? donorId,
            decimal amount,
            DateTime donationDate,
            string? description,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            Id = id;
            HouseId = houseId;
            DonorId = donorId;
            Amount = amount;
            DonationDate = donationDate;
            Description = description;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Donation ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// House ID
        /// </summary>
        public Guid? HouseId { get; set; }

        /// <summary>
        /// Donor/Contributor ID
        /// </summary>
        public Guid? DonorId { get; set; }

        /// <summary>
        /// Donation amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of donation
        /// </summary>
        public DateTime DonationDate { get; set; }

        /// <summary>
        /// Description or purpose
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Created timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated timestamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Donation Detail DTO (includes related house and donor info)
    /// </summary>
    public class DonationDetailDto : DonationDto
    {
        /// <summary>
        /// Associated house information
        /// </summary>
        public HouseDto? House { get; set; }

        /// <summary>
        /// Donor/Contributor information
        /// </summary>
        public ResidentDto? Donor { get; set; }
    }
}
