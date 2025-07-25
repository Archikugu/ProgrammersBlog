using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProgrammersBlog.Entities.Concrete;

public class SmtpSettings
{

    [DisplayName("Server")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string Server { get; set; }
    [DisplayName("Port")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [Range(0, 9999, ErrorMessage = "The {0} field must be at least {1} and at most {2}.")]
    public int Port { get; set; }
    [DisplayName("Sender Name")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(2, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string SenderName { get; set; }
    [DisplayName("Sender Email")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "The {0} field must be in email format.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(10, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string SenderEmail { get; set; }
    [DisplayName("Username/Email")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(1, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string Username { get; set; }
    [DisplayName("Password")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [DataType(DataType.Password, ErrorMessage = "The {0} field must be in password format.")]
    [MaxLength(50, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string Password { get; set; }
}
