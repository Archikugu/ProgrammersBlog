using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProgrammersBlog.Entities.Concrete;

public class WebsiteInfo
{
    [DisplayName("Site Title")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string Title { get; set; }
    [DisplayName("Menu Site Title")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string MenuTitle { get; set; }
    [DisplayName("SEO Description")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string SeoDescription { get; set; }
    [DisplayName("SEO Tags")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(100, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string SeoTags { get; set; }
    [DisplayName("SEO Author")]
    [Required(ErrorMessage = "The {0} field is required.")]
    [MaxLength(60, ErrorMessage = "The {0} field must not be greater than {1} characters.")]
    [MinLength(5, ErrorMessage = "The {0} field must not be less than {1} characters.")]
    public string SeoAuthor { get; set; }
}
