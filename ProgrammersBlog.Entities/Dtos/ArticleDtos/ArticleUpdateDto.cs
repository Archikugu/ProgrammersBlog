using ProgrammersBlog.Entities.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.ArticleDtos;

public class ArticleUpdateDto
{
    [Required]
    public int Id { get; set; }

    [DisplayName("Title")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string Title { get; set; }

    [DisplayName("Content")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MinLength(20, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string Content { get; set; }

    [DisplayName("Thumbnail")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(250, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(5, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string Thumbnail { get; set; }

    [DisplayName("Date")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    [DisplayName("Seo Author")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoAuthor { get; set; }

    [DisplayName("Seo Description")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(70, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoDescription { get; set; }

    [DisplayName("Seo Tags")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(70, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoTags { get; set; }

    [DisplayName("Category")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    [DisplayName("Is Active ?")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public bool IsActive { get; set; }

    [DisplayName("Is Deleted ?")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public bool IsDeleted { get; set; }
}