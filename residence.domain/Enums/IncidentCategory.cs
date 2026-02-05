namespace residence.domain.Enums;

/// <summary>
/// Incident category enumeration
/// Matches Angular categories: 'Plomberie', 'Électricité', 'Sécurité', 'Climatisation / Chauffage', 'Ascenseur', 'Autre'
/// </summary>
public enum IncidentCategory
{
    /// <summary>
    /// Plumbing - Water pipes, fixtures, drainage
    /// </summary>
    Plomberie = 0,

    /// <summary>
    /// Electricity - Electrical system, power issues
    /// </summary>
    Électricité = 1,

    /// <summary>
    /// Security - Locks, alarms, access control
    /// </summary>
    Sécurité = 2,

    /// <summary>
    /// Air Conditioning / Heating - HVAC systems
    /// </summary>
    ClimatisationChauffage = 3,

    /// <summary>
    /// Elevator - Lift maintenance and repairs
    /// </summary>
    Ascenseur = 4,

    /// <summary>
    /// Other - Miscellaneous issues
    /// </summary>
    Autre = 5
}
