using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CommentDtos;
using ProgrammersBlog.MvcUI.Helpers.Abstract;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.CommentAjaxViewModels;
using ProgrammersBlog.Core.Utilities.Extensions;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CommentController : BaseController
{
    private readonly ICommentService _commentService;
    public CommentController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, ICommentService commentService) : base(userManager, mapper, imageHelper)
    {
        _commentService = commentService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _commentService.GetAllByNonDeletedAsync();
        return View(result.Data);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var result = await _commentService.GetAllByNonDeletedAsync();
        var commentsResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        });
        return Json(commentsResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetDetail(int commentId)
    {
        var result = await _commentService.GetAsync(commentId);
        if (result.ResultStatus == ResultStatus.Success)
        {
            return PartialView("_CommentDetailPartial", result.Data);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int commentId)
    {
        var result = await _commentService.DeleteAsync(commentId, LoggedInUser.UserName);
        var commentResult = JsonSerializer.Serialize(result);
        return Json(commentResult);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int commentId)
    {
        var result = await _commentService.ApproveAsync(commentId, LoggedInUser.UserName);
        var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        });
        return Json(commentResult);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int commentId)
    {
        var result = await _commentService.GetCommentUpdateDtoAsync(commentId);
        if (result.ResultStatus == ResultStatus.Success)
        {
            return PartialView("_CommentUpdatePartial", result.Data);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> Update(CommentUpdateDto commentUpdateDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _commentService.UpdateAsync(commentUpdateDto, LoggedInUser.UserName);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var commentUpdateAjaxModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
                {
                    CommentDto = result.Data,
                    CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
                }, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                return Json(commentUpdateAjaxModel);
            }
        }
        var commentUpdateAjaxErrorModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
        {
            CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
        });
        return Json(commentUpdateAjaxErrorModel);
    }
}