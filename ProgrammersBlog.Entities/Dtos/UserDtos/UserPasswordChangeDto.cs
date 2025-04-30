using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserPasswordChangeDto
{
    [DisplayName("Current Password")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [DisplayName("New Password")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DisplayName("Repeat Password")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string RepeatPassword { get; set; }

}
