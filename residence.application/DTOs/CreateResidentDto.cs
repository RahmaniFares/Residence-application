using residence.domain.Enums;

namespace residence.application.DTOs;

public record CreateResidentDto(
    Guid UserId,
    Guid? HouseId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    DateOnly? BirthDate = null
);
