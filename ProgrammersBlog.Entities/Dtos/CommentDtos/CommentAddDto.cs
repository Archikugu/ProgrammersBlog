using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProgrammersBlog.Entities.Dtos.CommentDtos;

public class CommentAddDto
{

    [DisplayName("Comment")]
    [Required(ErrorMessage = "{0} must not be left empty.")]
    [MaxLength(1000, ErrorMessage = "{0} must not be longer than {1} characters.")]
    [MinLength(2, ErrorMessage = "{0} must not be shorter than {1} characters.")]
    public string Text { get; set; }

    [DisplayName("Your Name")]
    [Required(ErrorMessage = "{0} must not be left empty.")]
    [MaxLength(50, ErrorMessage = "{0} must not be longer than {1} characters.")]
    [MinLength(2, ErrorMessage = "{0} must not be shorter than {1} characters.")]
    public string CreatedByName { get; set; }

    [Required(ErrorMessage = "{0} must not be left empty.")]
    public int ArticleId { get; set; }

}
