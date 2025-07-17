using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Dtos.CommentDtos;
using ProgrammersBlog.MvcUI.Models;

namespace ProgrammersBlog.MvcUI.Controllers;

public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<JsonResult> Add(CommentAddDto commentAddDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _commentService.AddAsync(commentAddDto);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return Json(new CommentAddAjaxViewModel
                {
                    CommentDto = result.Data,
                    CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto),
                });
            }
            ModelState.AddModelError(string.Empty, result.Message);
        }
        return Json(new CommentAddAjaxViewModel
        {
            CommentAddDto = commentAddDto,
            CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
        });
    }
}
