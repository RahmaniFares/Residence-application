using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdateResidentDto(

    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateOnly? BirthDate = null,
    ResidentStatus Status = ResidentStatus.Active
);