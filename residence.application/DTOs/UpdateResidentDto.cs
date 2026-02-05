using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdateResidentDto(
    Guid? UserId,
    Guid? HouseId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Address,
    DateOnly? BirthDate = null,
    ResidentStatus Status = ResidentStatus.Active
);