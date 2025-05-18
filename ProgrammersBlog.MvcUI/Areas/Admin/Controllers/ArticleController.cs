using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Business.Utilities;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using ProgrammersBlog.Entities.Dtos.UserDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.ArticleViewModel;
using ProgrammersBlog.MvcUI.Helpers.Abstract;
using ProgrammersBlog.MvcUI.Helpers.Concrete;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Editor")]
public class ArticleController : BaseController
{
    private readonly IArticleService _articleService;
    private readonly ICategoryService _categoryService;
    private readonly IToastNotification _toastNotification;


    public ArticleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, IToastNotification toastNotification) : base(userManager, mapper, imageHelper)
    {
        _articleService = articleService;
        _categoryService = categoryService;
        _toastNotification = toastNotification;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _articleService.GetAllByNonDeletedAsync();
        if (result.ResultStatus == ResultStatus.Success)
            return View(result.Data);
        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var result = await _categoryService.GetAllByNonDeletedAndActiveAsync();

        if (result.ResultStatus == ResultStatus.Success)
        {
            return View(new ArticleAddViewModel
            {
                Categories = result.Data.Categories
            });
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
    {

        ModelState.Remove("Categories");
        if (ModelState.IsValid)
        {
            var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
            var imageResult = await ImageHelper.Upload(articleAddViewModel.Title, articleAddViewModel.ThumbnailFile, PictureType.Post);

            articleAddDto.Thumbnail = imageResult.Data.FullName;

            var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                _toastNotification.AddSuccessToastMessage(result.Message,new ToastrOptions
                {
                    Title = "Article Add Succesfuly"
                });
                return RedirectToAction("Index", "Article");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
            }
        }
        var categories = await _categoryService.GetAllByNonDeletedAsync();
        articleAddViewModel.Categories = categories.Data.Categories;
        return View(articleAddViewModel);
    }
    [HttpGet]
    public async Task<IActionResult> Update(int articleId)
    {

        var articleResult = await _articleService.GetArticleUpdateAsync(articleId);
        var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
        if (articleResult.ResultStatus == ResultStatus.Success && categoriesResult.ResultStatus == ResultStatus.Success)
        {
            var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
            articleUpdateViewModel.Categories = categoriesResult.Data.Categories;
            return View(articleUpdateViewModel);
        }
        return NotFound();
    }


    [HttpPost]
    public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateViewModel)
    {
        ModelState.Remove("Categories");
        if (ModelState.IsValid)
        {
            bool isNewThumbnailUploaded = false;
            var oldThumbnail = articleUpdateViewModel.Thumbnail;
            if (articleUpdateViewModel.ThumbnailFile != null)
            {
                var uploadeddImageResult = await ImageHelper.Upload(articleUpdateViewModel.Title,
                    articleUpdateViewModel.ThumbnailFile, PictureType.Post);

                articleUpdateViewModel.Thumbnail = uploadeddImageResult.ResultStatus == ResultStatus.Success
                    ? uploadeddImageResult.Data.FullName
                    : "postImages/defaultThumbnail.jpg";

                if (oldThumbnail != "postImages/defaultThumbnail.jpg")
                {
                    isNewThumbnailUploaded = true;
                }
            }
            var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
            var result = await _articleService.UpdateAsync(articleUpdateDto, LoggedInUser.UserName);
            if (result.ResultStatus == ResultStatus.Success)
            {
                if (isNewThumbnailUploaded)
                {
                    ImageHelper.Delete(oldThumbnail);
                }
                _toastNotification.AddWarningToastMessage(result.Message, new ToastrOptions
                {
                    Title = "Article Update Succesfuly"
                });
                return RedirectToAction("Index", "Article");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
            }
        }
        var categories = await _categoryService.GetAllByNonDeletedAsync();
        articleUpdateViewModel.Categories = categories.Data.Categories;
        return View(articleUpdateViewModel);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int articleId)
    {
        var articleResult = await _articleService.GetAsync(articleId);
        if (articleResult.ResultStatus == ResultStatus.Success)
        {
            var article = articleResult.Data;
            var result = await _articleService.DeleteAsync(articleId, LoggedInUser.UserName);
            if (result.ResultStatus == ResultStatus.Success)
            {
                if (!string.IsNullOrEmpty(article.Article.Thumbnail) && article.Article.Thumbnail != "postImages/defaultThumbnail.jpg")
                {
                    ImageHelper.Delete(article.Article.Thumbnail);
                }
            }
            var jsonResult = JsonSerializer.Serialize(result);
            return Json(jsonResult);
        }
        return Json(JsonSerializer.Serialize(new { ResultStatus = ResultStatus.Error, Message = Messages.Article.NotFound(false) }));
    }

    [HttpGet]
    public async Task<JsonResult> GetAllArticles()
    {
        var articles = await _articleService.GetAllByNonDeletedAndActiveAsync();
        var articleResuult = JsonSerializer.Serialize(articles, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        return Json(articleResuult);
    }
}
