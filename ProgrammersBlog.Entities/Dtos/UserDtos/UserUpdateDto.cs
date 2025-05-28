using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserUpdateDto
{
    [Required]
    public int Id { get; set; }

    [DisplayName("Username")]
    [Required(ErrorMessage = "{0} should not be left empty.")]
    [MaxLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string UserName { get; set; }

    [DisplayName("Email Address")]
    [Required(ErrorMessage = "{0} should not be left empty.")]
    [MaxLength(100, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(10, ErrorMessage = "{0} must be at least {1} characters long.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [DisplayName("Phone Number")]
    [Required(ErrorMessage = "{0} should not be left empty.")]
    [MaxLength(13, ErrorMessage = "{0} must not exceed {1} characters.")] // +905555555555
    [MinLength(13, ErrorMessage = "{0} must be at least {1} characters long.")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [DisplayName("Update Picture")]
    [DataType(DataType.Upload)]
    public IFormFile? PictureFile { get; set; }

    [DisplayName("Picture")]
    public string? Picture { get; set; }

    [DisplayName("First Name")]
    [MaxLength(30, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(2, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string FirstName { get; set; }

    [DisplayName("Last Name")]
    [MaxLength(30, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(2, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string LastName { get; set; }

    [DisplayName("About")]
    [MaxLength(1000, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string About { get; set; }

    [DisplayName("Twitter")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? TwitterLink { get; set; }

    [DisplayName("Facebook")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? FacebookLink { get; set; }

    [DisplayName("Instagram")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? InstagramLink { get; set; }

    [DisplayName("LinkedIn")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? LinkedInLink { get; set; }

    [DisplayName("YouTube")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? YoutubeLink { get; set; }

    [DisplayName("GitHub")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? GitHubLink { get; set; }

    [DisplayName("Website")]
    [MaxLength(250, ErrorMessage = "{0} must not exceed {1} characters.")]
    [MinLength(20, ErrorMessage = "{0} must be at least {1} characters long.")]
    public string? WebsiteLink { get; set; }

}
