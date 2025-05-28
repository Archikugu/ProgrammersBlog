﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ICommentService _commentService;
    private readonly IArticleService _articleService;
    private readonly UserManager<User> _userManager;
    public HomeController(ICategoryService categoryService, ICommentService commentService, IArticleService articleService, UserManager<User> userManager)
    {
        _categoryService = categoryService;
        _commentService = commentService;
        _articleService = articleService;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,AdminArea.Home.Read")]
    public async Task<IActionResult> Index()
    {
        var categoriesCountResult = await _categoryService.CountByNonDeletedAsync();
        var articlesCountResult = await _articleService.CountByNonDeletedAsync();
        var commentsCountResult = await _commentService.CountByNonDeletedAsync();
        var userCount = await _userManager.Users.CountAsync();
        var articlesResult = await _articleService.GetAllAsync();

        if (categoriesCountResult.ResultStatus == ResultStatus.Success && articlesCountResult.ResultStatus == ResultStatus.Success && commentsCountResult.ResultStatus == ResultStatus.Success
            && userCount > -1 && articlesResult.ResultStatus == ResultStatus.Success)
        {
            return View(new DashboardViewModel
            {
                CategoriesCount = categoriesCountResult.Data,
                ArticlesCount = articlesCountResult.Data,
                CommentsCount = commentsCountResult.Data,
                UsersCount = userCount,
                Articles = articlesResult.Data
            });
        }
        return NotFound();
    }
}
