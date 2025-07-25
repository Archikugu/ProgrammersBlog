using System.ComponentModel.DataAnnotations;

namespace ProgrammersBlog.Entities.ComplexTypes;

public enum OrderBy
{
    [Display(Name = "Date")]
    Date = 0,
    [Display(Name = "View Count")]
    ViewCount = 1,
    [Display(Name = "Comment Count")]
    CommentCount = 2,
}
