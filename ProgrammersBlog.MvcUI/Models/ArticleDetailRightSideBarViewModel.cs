using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.MvcUI.Models;

public class ArticleDetailRightSideBarViewModel
{
    public string Header { get; set; }
    public ArticleListDto ArticleListDto { get; set; }
    public User User { get; set; }
}
