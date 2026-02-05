namespace residence.application.DTOs;

/// <summary>
/// User role enumeration
/// </summary>
public enum UserRole
{
    Admin = 0,
    Resident = 1
}

/// <summary>
/// House status enumeration
/// </summary>
public enum HouseStatus
{
    Occupied = 0,
    Vacant = 1
}

/// <summary>
/// Resident status enumeration
/// </summary>
public enum ResidentStatus
{
    Active = 0,
    MovedOut = 1
}

/// <summary>
/// Payment status enumeration
/// </summary>
public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Overdue = 2
}

/// <summary>
/// Payment method enumeration
/// </summary>
public enum PaymentMethod
{
    Cash = 0,
    Transfer = 1,
    Card = 2
}

/// <summary>
/// Incident status enumeration
/// </summary>
public enum IncidentStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2
}

/// <summary>
/// Incident priority enumeration
/// </summary>
public enum IncidentPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

/// <summary>
/// Incident category enumeration
/// Matches Angular categories: Plomberie, Électricité, Sécurité, Climatisation/Chauffage, Ascenseur, Autre
/// </summary>
public enum IncidentCategory
{
    /// <summary>Plomberie - Plumbing</summary>
    Plomberie = 0,
    /// <summary>Électricité - Electricity</summary>
    Électricité = 1,
    /// <summary>Sécurité - Security</summary>
    Sécurité = 2,
    /// <summary>ClimatisationChauffage - Air Conditioning / Heating</summary>
    ClimatisationChauffage = 3,
    /// <summary>Ascenseur - Elevator</summary>
    Ascenseur = 4,
    /// <summary>Autre - Other</summary>
    Autre = 5
}

/// <summary>
/// Expense type enumeration
/// </summary>
public enum ExpenseType
{
    Maintenance = 0,
    Electricity = 1,
    Water = 2,
    Cleaning = 3,
    Security = 4,
    Gardening = 5,
    Repairs = 6,
    Equipment = 7,
    Insurance = 8,
    Taxes = 9,
    Other = 10
}
