using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.CommentDtos;

public class CommentListDto
{
    public IList<Comment> Comments { get; set; }
}
