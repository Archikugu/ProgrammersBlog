﻿using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using ProgrammersBlog.MvcUI.Models;

namespace ProgrammersBlog.MvcUI.Controllers;

public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    [HttpGet]
    public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
    {
        var searchResult = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
        if (searchResult.ResultStatus == ResultStatus.Success)
            return View(new ArticleSearchViewModel
            {
                ArticleListDto = searchResult.Data,
                Keyword = keyword
            });
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> Detail(int articleId)
    {
        var articleResult = await _articleService.GetAsync(articleId);
        if (articleResult.ResultStatus == ResultStatus.Success)
        {
            return View(articleResult.Data);
        }

        return NotFound();
    }
}
