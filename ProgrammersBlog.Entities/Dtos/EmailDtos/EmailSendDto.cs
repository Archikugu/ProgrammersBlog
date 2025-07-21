using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProgrammersBlog.Entities.Dtos.EmailDtos;

public class EmailSendDto
{
    [DisplayName("Your Name")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(60, ErrorMessage = "The {0} field must be at most {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must be at least {1} characters.")]
    public string Name { get; set; }
    [DisplayName("Your Email Address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must be at most {1} characters.")]
    [MinLength(10, ErrorMessage = "The {0} field must be at least {1} characters.")]
    public string Email { get; set; }
    [DisplayName("Subject")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(125, ErrorMessage = "The {0} field must be at most {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must be at least {1} characters.")]
    public string Subject { get; set; }
    [DisplayName("Your Message")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(1500, ErrorMessage = "The {0} field must be at most {1} characters.")]
    [MinLength(20, ErrorMessage = "The {0} field must be at least {1} characters.")]
    public string Message { get; set; }
}
