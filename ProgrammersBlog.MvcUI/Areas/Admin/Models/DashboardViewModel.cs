﻿using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models;

public class DashboardViewModel
{
    public int CategoriesCount { get; set; }
    public int ArticlesCount { get; set; }
    public int CommentsCount { get; set; }
    public int UsersCount { get; set; }
    public ArticleListDto Articles { get; set; }
}
