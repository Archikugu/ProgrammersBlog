using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ProgrammersBlog.Entities.Concrete;


namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.ArticleViewModels;

public class ArticleAddViewModel
{
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
    public IFormFile ThumbnailFile { get; set; }

    [DisplayName("Date")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date { get; set; }

    [DisplayName("Seo Author")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoAuthor { get; set; }

    [DisplayName("Article Description")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(70, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoDescription { get; set; }

    [DisplayName("Article Tags")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    [MaxLength(70, ErrorMessage = "{0} cannot be more than {1} characters")]
    [MinLength(0, ErrorMessage = "{0} cannot be less than {1} characters")]
    public string SeoTags { get; set; }

    [DisplayName("Category")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public int CategoryId { get; set; }

    [DisplayName("Is Active ?")]
    [Required(ErrorMessage = "{0} cannot be empty")]
    public bool IsActive { get; set; }

    [DisplayName("Note")]
    [MaxLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Note { get; set; }

    public IList<Category> Categories { get; set; }
}
