using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.MvcUI.Controllers;

public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
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
