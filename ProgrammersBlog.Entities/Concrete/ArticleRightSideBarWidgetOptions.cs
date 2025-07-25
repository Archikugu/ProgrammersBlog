using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProgrammersBlog.Entities.ComplexTypes;

namespace ProgrammersBlog.Entities.Concrete;

public class ArticleRightSideBarWidgetOptions
{
    [DisplayName("Widget Title")]
    [Required(ErrorMessage = "{0} field is required.")]
    [MaxLength(150, ErrorMessage = "{0} field must be at most {1} characters.")]
    [MinLength(5, ErrorMessage = "{0} field must be at least {1} characters.")]
    public string Header { get; set; }
    [DisplayName("Article Count")]
    [Required(ErrorMessage = "{0} field is required.")]
    [Range(0, 50, ErrorMessage = "{0} field must be at least {1} and at most {2}.")]
    public int TakeSize { get; set; }
    [DisplayName("Category")]
    [Required(ErrorMessage = "{0} field is required.")]
    public int CategoryId { get; set; }
    [DisplayName("Filter Type")]
    [Required(ErrorMessage = "{0} field is required.")]
    public FilterBy FilterBy { get; set; }
    [DisplayName("Order Type")]
    [Required(ErrorMessage = "{0} field is required.")]
    public OrderBy OrderBy { get; set; }
    [DisplayName("Order Criterion")]
    [Required(ErrorMessage = "{0} field is required.")]
    public bool IsAscending { get; set; }
    [DisplayName("Start Date")]
    [Required(ErrorMessage = "{0} field is required.")]
    [DataType(DataType.Date, ErrorMessage = "{0} field must be in date format.")]
    public DateTime StartAt { get; set; }
    [DisplayName("End Date")]
    [Required(ErrorMessage = "{0} field is required.")]
    [DataType(DataType.Date, ErrorMessage = "{0} field must be in date format.")]
    public DateTime EndAt { get; set; }
    [DisplayName("Maximum View Count")]
    [Required(ErrorMessage = "{0} field is required.")]
    public int MaxViewCount { get; set; }
    [DisplayName("Minimum View Count")]
    [Required(ErrorMessage = "{0} field is required.")]
    public int MinViewCount { get; set; }
    [DisplayName("Maximum Comment Count")]
    [Required(ErrorMessage = "{0} field is required.")]
    public int MaxCommentCount { get; set; }
    [DisplayName("Minimum Comment Count")]
    [Required(ErrorMessage = "{0} field is required.")]
    public int MinCommentCount { get; set; }
}
