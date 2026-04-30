using residence.application.DTOs;
using residence.application.Interfaces;
using residence.application.Repositories;
using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace residence.application.Services
{
    /// <summary>
    /// Service implementation for Donation business logic
    /// </summary>
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IHouseRepository _houseRepository;

        public DonationService(
            IDonationRepository donationRepository,
            IHouseRepository houseRepository)
        {
            _donationRepository = donationRepository;
            _houseRepository = houseRepository;
        }

        public async Task<DonationDto> CreateDonationAsync(Guid houseId, CreateDonationDto dto)
        {
            // Validate house exists
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new Exception("House not found");

            // Validate amount
            if (dto.Amount <= 0)
                throw new Exception("Donation amount must be greater than zero");

            var donation = new Donation
            {
                Id = Guid.NewGuid(),
                HouseId = dto.HouseId ?? houseId,
                DonorId = dto.DonorId,
                Amount = dto.Amount,
                DonationDate = dto.DonationDate == default ? DateTime.UtcNow : dto.DonationDate,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _donationRepository.AddAsync(donation);
            return MapToDto(created);
        }

        public async Task<DonationDto> GetDonationByIdAsync(Guid id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
                throw new Exception("Donation not found");

            return MapToDto(donation);
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByHouseAsync(Guid houseId)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new Exception("House not found");

            var donations = await _donationRepository.GetByHouseAsync(houseId);
            return donations.Select(MapToDto);
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByDonorAsync(Guid donorId)
        {
            var donations = await _donationRepository.GetByDonorAsync(donorId);
            return donations.Select(MapToDto);
        }

        public async Task<IEnumerable<DonationDto>> GetAllDonationsAsync()
        {
            var donations = await _donationRepository.GetAllAsync();
            return donations.Select(MapToDto);
        }

        public async Task<DonationDto> UpdateDonationAsync(Guid id, UpdateDonationDto dto)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
                throw new Exception("Donation not found");

            // Validate amount if changed
            if (dto.Amount <= 0)
                throw new Exception("Donation amount must be greater than zero");

            donation.HouseId = dto.HouseId ?? donation.HouseId;
            donation.DonorId = dto.DonorId ?? donation.DonorId;
            donation.Amount = dto.Amount;
            donation.DonationDate = dto.DonationDate == default ? donation.DonationDate : dto.DonationDate;
            donation.Description = dto.Description;
            donation.UpdatedAt = DateTime.UtcNow;

            var updated = await _donationRepository.UpdateAsync(donation);
            return MapToDto(updated);
        }

        public async Task DeleteDonationAsync(Guid id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
                throw new Exception("Donation not found");

            await _donationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var donations = await _donationRepository.GetByDateRangeAsync(startDate, endDate);
            return donations.Select(MapToDto);
        }

        public async Task<decimal> GetTotalDonationsByHouseAsync(Guid houseId)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new Exception("House not found");

            return await _donationRepository.GetTotalByHouseAsync(houseId);
        }

        public async Task<DonationDetailDto> GetDonationDetailsAsync(Guid id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
                throw new Exception("Donation not found");

            var detail = new DonationDetailDto
            {
                Id = donation.Id,
                HouseId = donation.HouseId,
                DonorId = donation.DonorId,
                Amount = donation.Amount,
                DonationDate = donation.DonationDate,
                Description = donation.Description,
                CreatedAt = donation.CreatedAt,
                UpdatedAt = donation.UpdatedAt
            };

            // Map house if available
            if (donation.House != null)
            {
                detail.House = new HouseDto(
                    donation.House.Id,
                    donation.House.Block,
                    donation.House.Unit,
                    donation.House.Floor,
                    (residence.application.DTOs.HouseStatus)donation.House.Status,
                    donation.House.CurrentResidentId,
                    donation.House.CreatedAt,
                    donation.House.UpdatedAt
                );
            }

            // Map donor if available
            if (donation.Donor != null)
            {
                detail.Donor = new ResidentDto(
                    donation.Donor.Id,
                    donation.Donor.HouseId,
                    donation.Donor.FirstName,
                    donation.Donor.LastName,
                    donation.Donor.Email,
                    donation.Donor.PhoneNumber,
                    donation.Donor.Address,
                    donation.Donor.BirthDate,
                    (residence.application.DTOs.ResidentStatus)donation.Donor.Status,
                    donation.Donor.MoveInDate,
                    donation.Donor.MoveOutDate,
                    donation.Donor.CreatedAt,
                    donation.Donor.UpdatedAt
                );
            }

            return detail;
        }

        private DonationDto MapToDto(Donation donation)
        {
            return new DonationDto(
                donation.Id,
                donation.HouseId,
                donation.DonorId,
                donation.Amount,
                donation.DonationDate,
                donation.Description,
                donation.CreatedAt,
                donation.UpdatedAt
            );
        }
    }
}
