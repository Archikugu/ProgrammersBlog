﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Helpers.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.EmailDtos;

namespace ProgrammersBlog.MvcUI.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly IArticleService _articleService;
    private readonly AboutUsPageInfo _aboutUsPageInfo;
    private readonly IMailService _mailService;
    private readonly IToastNotification _toastNotification;
    private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;

    public HomeController(IArticleService articleService, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IMailService mailService, IToastNotification toastNotification, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter)
    {
        _articleService = articleService;
        _aboutUsPageInfo = aboutUsPageInfo.Value;
        _mailService = mailService;
        _toastNotification = toastNotification;
        _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
    }
    [Route("index")]
    [Route("homepage")]
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
    {
        var articlesResult = await (categoryId == null
            ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
            : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize, isAscending));
        return View(articlesResult.Data);
    }
    [Route("about")]
    [HttpGet]
    public async Task<IActionResult> About()
    {
        //throw new Exception("An error occurred while loading the About page."); // Simulating an error for demonstration purposes
        return View(_aboutUsPageInfo);
    }

    [Route("contact")]
    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Contact(EmailSendDto emailSendDto)
    {
        if (ModelState.IsValid)
        {
            var result = _mailService.SendContactEmail(emailSendDto);
            _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
            {
                Title = "Operation Successful",
            });
            return View();
        }
        return View(emailSendDto);
    }
}
