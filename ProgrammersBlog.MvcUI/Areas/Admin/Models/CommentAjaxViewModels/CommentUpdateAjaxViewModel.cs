using ProgrammersBlog.Entities.Dtos.CommentDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.CommentAjaxViewModels
{
    public class CommentUpdateAjaxViewModel
    {
        public CommentUpdateDto CommentUpdateDto { get; set; }
        public string CommentUpdatePartial { get; set; }
        public CommentDto CommentDto { get; set; }
    }
}
