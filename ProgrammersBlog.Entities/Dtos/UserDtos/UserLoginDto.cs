using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserLoginDto
{
    [DisplayName("E-Mail")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(100, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(10, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DisplayName("Password")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; }
}
