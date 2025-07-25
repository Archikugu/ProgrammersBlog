using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Helpers.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OptionsController : Controller
{
    private readonly AboutUsPageInfo _aboutUsPageInfo;
    private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
    private readonly IToastNotification _toastNotification;
    private readonly WebsiteInfo _websiteInfo;
    private readonly IWritableOptions<WebsiteInfo> _websiteInfoWriter;
    private readonly SmtpSettings _smtpSettings;
    private readonly IWritableOptions<SmtpSettings> _smtpSettingsWriter;
    private readonly ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
    private readonly IWritableOptions<ArticleRightSideBarWidgetOptions> _articleRightSideBarWidgetOptionsWriter;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter,
        IToastNotification toastNotification, IOptionsSnapshot<WebsiteInfo> websiteInfo, IWritableOptions<WebsiteInfo> websiteInfoWriter,
        IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtpSettingsWriter, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions, IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter, ICategoryService categoryService, IMapper mapper)
    {
        _aboutUsPageInfo = aboutUsPageInfo.Value;
        _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
        _toastNotification = toastNotification;
        _websiteInfo = websiteInfo.Value;
        _websiteInfoWriter = websiteInfoWriter;
        _smtpSettings = smtpSettings.Value;
        _smtpSettingsWriter = smtpSettingsWriter;
        _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
        _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult About()
    {
        return View(_aboutUsPageInfo);
    }
    [HttpPost]
    public IActionResult About(AboutUsPageInfo aboutUsPageInfo)
    {
        if (ModelState.IsValid)
        {
            _aboutUsPageInfoWriter.Update(x =>
             {
                 x.Header = aboutUsPageInfo.Header;
                 x.Content = aboutUsPageInfo.Content;
                 x.SeoDescription = aboutUsPageInfo.SeoDescription;
                 x.SeoTags = aboutUsPageInfo.SeoTags;
                 x.SeoAuthor = aboutUsPageInfo.SeoAuthor;
             });
            _toastNotification.AddSuccessToastMessage("About Us page information updated successfully.", new ToastrOptions
            {
                Title = "Operation Successful"
            });
            return View(aboutUsPageInfo);
        }
        return View(aboutUsPageInfo);
    }
    [HttpGet]
    public IActionResult GeneralSettings()
    {
        return View(_websiteInfo);
    }
    [HttpPost]
    public IActionResult GeneralSettings(WebsiteInfo websiteInfo)
    {
        if (ModelState.IsValid)
        {
            _websiteInfoWriter.Update(x =>
             {
                 x.Title = websiteInfo.Title;
                 x.MenuTitle = websiteInfo.MenuTitle;
                 x.SeoDescription = websiteInfo.SeoDescription;
                 x.SeoTags = websiteInfo.SeoTags;
                 x.SeoAuthor = websiteInfo.SeoAuthor;
             });
            _toastNotification.AddSuccessToastMessage("General Settings information updated successfully.", new ToastrOptions
            {
                Title = "Operation Successful"
            });
            return View(websiteInfo);
        }
        return View(websiteInfo);
    }
    [HttpGet]
    public IActionResult EmailSettings()
    {
        return View(_smtpSettings);
    }
    [HttpPost]
    public IActionResult EmailSettings(SmtpSettings smtpSettings)
    {
        if (ModelState.IsValid)
        {
            _smtpSettingsWriter.Update(x =>
             {
                 x.Server = smtpSettings.Server;
                 x.Port = smtpSettings.Port;
                 x.SenderName = smtpSettings.SenderName;
                 x.SenderEmail = smtpSettings.SenderEmail;
                 x.Username = smtpSettings.Username;
                 x.Password = smtpSettings.Password;
             });
            _toastNotification.AddSuccessToastMessage("Email Settings information updated successfully.", new ToastrOptions
            {
                Title = "Operation Successful"
            });
            return View(smtpSettings);
        }
        return View(smtpSettings);
    }
    [HttpGet]
    public async Task<IActionResult> ArticleRightSideBarWidgetSettings()
    {
        var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
        var articleRightSideBarWidgetOptionsViewModel =
            _mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleRightSideBarWidgetOptions);
        articleRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
        return View(articleRightSideBarWidgetOptionsViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel articleRightSideBarWidgetOptionsViewModel)
    {
        ModelState.Remove("Categories");
        var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
        articleRightSideBarWidgetOptionsViewModel.Categories = categoriesResult.Data.Categories;
        if (ModelState.IsValid)
        {
            _articleRightSideBarWidgetOptionsWriter.Update(x =>
            {
                x.Header = articleRightSideBarWidgetOptionsViewModel.Header;
                x.TakeSize = articleRightSideBarWidgetOptionsViewModel.TakeSize;
                x.CategoryId = articleRightSideBarWidgetOptionsViewModel.CategoryId;
                x.FilterBy = articleRightSideBarWidgetOptionsViewModel.FilterBy;
                x.OrderBy = articleRightSideBarWidgetOptionsViewModel.OrderBy;
                x.IsAscending = articleRightSideBarWidgetOptionsViewModel.IsAscending;
                x.StartAt = articleRightSideBarWidgetOptionsViewModel.StartAt;
                x.EndAt = articleRightSideBarWidgetOptionsViewModel.EndAt;
                x.MaxViewCount = articleRightSideBarWidgetOptionsViewModel.MaxViewCount;
                x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;
                x.MaxCommentCount = articleRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                x.MinCommentCount = articleRightSideBarWidgetOptionsViewModel.MinCommentCount;
            });
            _toastNotification.AddSuccessToastMessage("Article Right Side Bar Widget Settings information updated successfully.", new ToastrOptions
            {
                Title = "Operation Successful"
            });
            return View(articleRightSideBarWidgetOptionsViewModel);

        }
        return View(articleRightSideBarWidgetOptionsViewModel);
    }
}
