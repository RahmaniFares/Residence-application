using residence.domain.Enums;

namespace residence.application.DTOs;

public record LoginDto(
    string Email,
    string Password
);

