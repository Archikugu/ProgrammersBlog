using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProgrammersBlog.Entities.Concrete;

public class AboutUsPageInfo
{
    [DisplayName("Header")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(150, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string Header { get; set; }

    [DisplayName("Content")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(1500, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string Content { get; set; }

    [DisplayName("SEO Description")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoDescription { get; set; }

    [DisplayName("SEO Tags")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoTags { get; set; }

    [DisplayName("SEO Author")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoAuthor { get; set; }
}
