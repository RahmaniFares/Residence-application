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
    /// Service for managing User-House relationships
    /// </summary>
    public class UserHouseService : IUserHouseService
    {
        private readonly IUserHouseRepository _userHouseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHouseRepository _houseRepository;

        public UserHouseService(
            IUserHouseRepository userHouseRepository,
            IUserRepository userRepository,
            IHouseRepository houseRepository)
        {
            _userHouseRepository = userHouseRepository;
            _userRepository = userRepository;
            _houseRepository = houseRepository;
        }

        public async Task<UserHouseDto> AssignUserToHouseAsync(Guid residenceId, CreateUserHouseDto dto)
        {
            // Verify user exists and belongs to residence
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null || user.ResidenceId != residenceId)
                throw new InvalidOperationException($"User with ID {dto.UserId} not found in residence {residenceId}");

            // Verify house exists and belongs to residence
            var house = await _houseRepository.GetByIdAsync(dto.HouseId);
            if (house == null || house.ResidenceId != residenceId)
                throw new InvalidOperationException($"House with ID {dto.HouseId} not found in residence {residenceId}");

            // Check if already assigned
            var existing = await _userHouseRepository.GetUserHouseAsync(dto.UserId, dto.HouseId);
            if (existing != null)
                throw new InvalidOperationException("User is already assigned to this house");

            // Create new assignment
            var userHouse = new UserHouse
            {
                UserId = dto.UserId,
                HouseId = dto.HouseId,
                AssignedDate = DateTime.UtcNow,
                Notes = dto.Notes
            };

            var created = await _userHouseRepository.AddAsync(userHouse);
            return await MapToDtoAsync(created);
        }

        public async Task<bool> RemoveUserFromHouseAsync(Guid residenceId, Guid userId, Guid houseId)
        {
            // Verify user and house belong to residence
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.ResidenceId != residenceId)
                return false;

            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null || house.ResidenceId != residenceId)
                return false;

            // Remove the assignment
            var userHouse = await _userHouseRepository.GetUserHouseAsync(userId, houseId);
            if (userHouse == null)
                return false;

            await _userHouseRepository.RemoveUserFromHouseAsync(userId, houseId);
            return true;
        }

        public async Task<UserHousesResponseDto> GetUserHousesAsync(Guid userId)
        {
            var userHouses = await _userHouseRepository.GetUserHousesAsync(userId);
            var list = userHouses.ToList();

            var houses = new List<HouseDetailForUserDto>();
            foreach (var uh in list)
            {
                var house = await _houseRepository.GetByIdAsync(uh.HouseId);
                if (house != null)
                {
                    houses.Add(new HouseDetailForUserDto
                    {
                        Id = house.Id,
                        Block = house.Block,
                        Unit = house.Unit,
                        Floor = house.Floor,
                        Status = (HouseStatus)house.Status,
                        AssignedDate = uh.AssignedDate,
                        Notes = uh.Notes
                    });
                }
            }

            return new UserHousesResponseDto
            {
                Houses = houses,
                TotalCount = houses.Count
            };
        }

        public async Task<IEnumerable<UserHouseDto>> GetHouseUsersAsync(Guid houseId)
        {
            var userHouses = await _userHouseRepository.GetHouseUsersAsync(houseId);
            var result = new List<UserHouseDto>();

            foreach (var uh in userHouses)
            {
                result.Add(await MapToDtoAsync(uh));
            }

            return result;
        }

        public async Task<UserHouseDto> UpdateUserHouseAsync(Guid residenceId, Guid userId, Guid houseId, UpdateUserHouseDto dto)
        {
            // Verify user and house belong to residence
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.ResidenceId != residenceId)
                throw new InvalidOperationException($"User not found in residence {residenceId}");

            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null || house.ResidenceId != residenceId)
                throw new InvalidOperationException($"House not found in residence {residenceId}");

            // Get the relationship
            var userHouse = await _userHouseRepository.GetUserHouseAsync(userId, houseId);
            if (userHouse == null)
                throw new InvalidOperationException("User-House relationship not found");

            // Update notes
            if (!string.IsNullOrEmpty(dto.Notes))
                userHouse.Notes = dto.Notes;

            userHouse.UpdatedAt = DateTime.UtcNow;

            await _userHouseRepository.UpdateAsync(userHouse);
            return await MapToDtoAsync(userHouse);
        }

        public async Task<bool> IsUserAssignedToHouseAsync(Guid userId, Guid houseId)
        {
            return await _userHouseRepository.IsUserAssignedToHouseAsync(userId, houseId);
        }

        public async Task<UserHouseDto?> GetUserHouseDetailsAsync(Guid userId, Guid houseId)
        {
            var userHouse = await _userHouseRepository.GetUserHouseAsync(userId, houseId);
            if (userHouse == null)
                return null;

            return await MapToDtoAsync(userHouse);
        }

        private async Task<UserHouseDto> MapToDtoAsync(UserHouse userHouse)
        {
            var user = await _userRepository.GetByIdAsync(userHouse.UserId);
            var house = await _houseRepository.GetByIdAsync(userHouse.HouseId);

            return new UserHouseDto
            {
                Id = userHouse.Id,
                UserId = userHouse.UserId,
                HouseId = userHouse.HouseId,
                AssignedDate = userHouse.AssignedDate,
                Notes = userHouse.Notes,
                CreatedAt = userHouse.CreatedAt,
                User = user != null ? new UserHouseSummaryDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl
                } : null,
                House = house != null ? new HouseUserSummaryDto
                {
                    Id = house.Id,
                    Block = house.Block,
                    Unit = house.Unit,
                    Floor = house.Floor,
                    Status = house.Status.ToString()
                } : null
            };
        }
    }
}
