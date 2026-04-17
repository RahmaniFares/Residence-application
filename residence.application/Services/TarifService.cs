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
    /// Service for tariff management with history tracking
    /// </summary>
    public class TarifService : ITarifService
    {
        private readonly ITarifRepository _tarifRepository;
        private readonly ITarifHistoryRepository _tarifHistoryRepository;
        private readonly IResidenceRepository _residenceRepository;

        public TarifService(
            ITarifRepository tarifRepository,
            ITarifHistoryRepository tarifHistoryRepository,
            IResidenceRepository residenceRepository)
        {
            _tarifRepository = tarifRepository;
            _tarifHistoryRepository = tarifHistoryRepository;
            _residenceRepository = residenceRepository;
        }

        public async Task<TarifDto> CreateTarifAsync(Guid residenceId, CreateTarifDto dto, string userId)
        {
            // Verify residence exists
            var residence = await _residenceRepository.GetByIdAsync(residenceId);
            if (residence == null)
                throw new InvalidOperationException($"Residence with ID {residenceId} not found.");

            // Deactivate previous active tariffs
            var currentTarif = await _tarifRepository.GetCurrentTarifAsync(residenceId);
            if (currentTarif != null)
            {
                currentTarif.IsActive = false;
                currentTarif.EndDate = dto.EffectiveDate.AddDays(-1);
                await _tarifRepository.UpdateAsync(currentTarif);
            }

            // Create new tariff
            var tarif = new Tarif
            {
                ResidenceId = residenceId,
                Description = dto.Description,
                Amount = dto.Amount,
                Currency = dto.Currency,
                EffectiveDate = dto.EffectiveDate,
                IsActive = true,
                Notes = dto.Notes
            };

            var createdTarif = await _tarifRepository.AddAsync(tarif);

            return MapToDto(createdTarif);
        }

        public async Task<TarifDto> UpdateTarifAsync(Guid residenceId, Guid tarifId, UpdateTarifDto dto, string userId)
        {
            var tarif = await _tarifRepository.GetByIdAsync(tarifId);
            if (tarif == null)
                throw new InvalidOperationException($"Tariff with ID {tarifId} not found.");

            if (tarif.ResidenceId != residenceId)
                throw new InvalidOperationException("Tariff does not belong to the specified residence.");

            // Store the old amount for rappel detection
            var oldAmount = tarif.Amount;
            var amountChanged = dto.Amount.HasValue && dto.Amount != tarif.Amount;

            // Record history if amount or description changed
            if (amountChanged || 
                !string.IsNullOrEmpty(dto.Description) && dto.Description != tarif.Description)
            {
                var history = new TarifHistory
                {
                    TarifId = tarifId,
                    ResidenceId = residenceId,
                    PreviousAmount = tarif.Amount,
                    NewAmount = dto.Amount ?? tarif.Amount,
                    PreviousDescription = tarif.Description,
                    NewDescription = dto.Description ?? tarif.Description,
                    EffectiveDate = dto.EffectiveDate ?? DateTime.UtcNow,
                    ChangedBy = userId,
                    ChangeReason = dto.ChangeReason
                };

                await _tarifHistoryRepository.AddAsync(history);
            }

            // Update tariff
            if (!string.IsNullOrEmpty(dto.Description))
                tarif.Description = dto.Description;

            if (dto.Amount.HasValue)
                tarif.Amount = dto.Amount.Value;

            if (!string.IsNullOrEmpty(dto.Currency))
                tarif.Currency = dto.Currency;

            if (dto.EffectiveDate.HasValue)
                tarif.EffectiveDate = dto.EffectiveDate.Value;

            if (!string.IsNullOrEmpty(dto.Notes))
                tarif.Notes = dto.Notes;

            tarif.UpdatedAt = DateTime.UtcNow;

            await _tarifRepository.UpdateAsync(tarif);

            return MapToDto(tarif);
        }

        public async Task<TarifDto?> GetTarifByIdAsync(Guid tarifId)
        {
            var tarif = await _tarifRepository.GetByIdAsync(tarifId);
            return tarif == null ? null : MapToDto(tarif);
        }

        public async Task<IEnumerable<TarifDto>> GetTarifsByResidenceAsync(Guid residenceId)
        {
            var tarifs = await _tarifRepository.GetTarifsByResidenceAsync(residenceId);
            return tarifs.Select(MapToDto).ToList();
        }

        public async Task<TarifDto?> GetCurrentTarifAsync(Guid residenceId)
        {
            var tarif = await _tarifRepository.GetCurrentTarifAsync(residenceId);
            return tarif == null ? null : MapToDto(tarif);
        }

        public async Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryAsync(Guid tarifId)
        {
            var history = await _tarifHistoryRepository.GetHistoryByTarifIdAsync(tarifId);
            return history.Select(MapHistoryToDto).OrderByDescending(h => h.ChangedAt).ToList();
        }

        public async Task<IEnumerable<TarifHistoryDto>> GetResidenceTarifHistoryAsync(Guid residenceId)
        {
            var history = await _tarifHistoryRepository.GetHistoryByResidenceIdAsync(residenceId);
            return history.Select(MapHistoryToDto).OrderByDescending(h => h.ChangedAt).ToList();
        }

        public async Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate)
        {
            var history = await _tarifHistoryRepository.GetHistoryByDateRangeAsync(residenceId, startDate, endDate);
            return history.Select(MapHistoryToDto).OrderByDescending(h => h.ChangedAt).ToList();
        }

        public async Task<TarifHistoryDto> UpdateTarifHistoryAsync(Guid residenceId, Guid tarifId, Guid historyId, UpdateTarifHistoryDto dto, string userId)
        {
            var history = await _tarifHistoryRepository.GetByIdAsync(historyId);
            if (history == null)
                throw new InvalidOperationException($"History record with ID {historyId} not found.");

            if (history.ResidenceId != residenceId)
                throw new InvalidOperationException("History record does not belong to the specified residence.");

            if (history.TarifId != tarifId)
                throw new InvalidOperationException("History record does not belong to the specified tariff.");

            // Update history fields
            if (dto.PreviousAmount.HasValue)
                history.PreviousAmount = dto.PreviousAmount.Value;

            if (dto.NewAmount.HasValue)
                history.NewAmount = dto.NewAmount.Value;

            if (!string.IsNullOrEmpty(dto.PreviousDescription))
                history.PreviousDescription = dto.PreviousDescription;

            if (!string.IsNullOrEmpty(dto.NewDescription))
                history.NewDescription = dto.NewDescription;

            if (dto.EffectiveDate.HasValue)
                history.EffectiveDate = dto.EffectiveDate.Value;

            if (!string.IsNullOrEmpty(dto.ChangeReason))
                history.ChangeReason = dto.ChangeReason;

            await _tarifHistoryRepository.UpdateAsync(history);

            return MapHistoryToDto(history);
        }

        public async Task<bool> DeleteTarifAsync(Guid residenceId, Guid tarifId)
        {
            var tarif = await _tarifRepository.GetByIdAsync(tarifId);
            if (tarif == null)
                return false;

            if (tarif.ResidenceId != residenceId)
                return false;

            await _tarifRepository.DeleteAsync(tarifId);
            await _tarifRepository.SaveChangesAsync();
            return true;
        }

        private TarifDto MapToDto(Tarif tarif) => new TarifDto
        {
            Id = tarif.Id,
            ResidenceId = tarif.ResidenceId,
            Description = tarif.Description,
            Amount = tarif.Amount,
            Currency = tarif.Currency,
            EffectiveDate = tarif.EffectiveDate,
            EndDate = tarif.EndDate,
            IsActive = tarif.IsActive,
            Notes = tarif.Notes,
            CreatedAt = tarif.CreatedAt,
            UpdatedAt = tarif.UpdatedAt
        };

        private TarifHistoryDto MapHistoryToDto(TarifHistory history) => new TarifHistoryDto
        {
            Id = history.Id,
            TarifId = history.TarifId,
            ResidenceId = history.ResidenceId,
            PreviousAmount = history.PreviousAmount,
            NewAmount = history.NewAmount,
            PreviousDescription = history.PreviousDescription,
            NewDescription = history.NewDescription,
            EffectiveDate = history.EffectiveDate,
            ChangedBy = history.ChangedBy,
            ChangeReason = history.ChangeReason,
            ChangedAt = history.ChangedAt
        };
    }
}
