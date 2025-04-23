using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserAddDto
{
    [DisplayName("User Name")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters")]
    public string UserName { get; set; }

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

    [DisplayName("PhoneNumber")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(13, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(13, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [DisplayName("Picture")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [DataType(DataType.Upload)]
    public IFormFile PictureFile { get; set; } 
    public string? Picture { get; set; }
}
