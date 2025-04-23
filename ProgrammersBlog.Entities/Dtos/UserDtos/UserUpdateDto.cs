using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserUpdateDto
{
    [Required]
    public int Id { get; set; }


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

    [DisplayName("PhoneNumber")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(13, ErrorMessage = "{0} cannot exceed {1} characters")]
    [MinLength(13, ErrorMessage = "{0} must be at least {1} characters")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [DisplayName("Update Picture")]
    [DataType(DataType.Upload)]
    public IFormFile? PictureFile { get; set; }

    [DisplayName("Picture")]
    public string? Picture { get; set; }
}
