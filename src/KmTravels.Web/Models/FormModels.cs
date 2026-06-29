using System.ComponentModel.DataAnnotations;
using SnlEngineering.Core.Enums;

namespace SnlEngineering.Web.Models;

public class MemberFormModel
{
    public MemberType MemberType { get; set; } = MemberType.Operator;

    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    public string Phone { get; set; } = string.Empty;

    public string? Address { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public string? CompanyName { get; set; }
    public string? LicenseNumber { get; set; }
}

public class ContactFormModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}
