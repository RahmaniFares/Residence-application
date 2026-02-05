using residence.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.application.DTOs
{
    public record CreateUserDto(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber,
        UserRole Role = UserRole.Resident
    );
}
