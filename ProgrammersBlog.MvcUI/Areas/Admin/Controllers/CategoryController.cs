﻿using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllByNonDeleted();

        return View(result.Data);
    }
    [HttpGet]
    public IActionResult Add()
    {
        return PartialView("_CategoryAddPartial");
    }
    [HttpPost]
    public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.Add(categoryAddDto, "Gokhan Gok");
            if (result.ResultStatus == ResultStatus.Success)
            {
                var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                {
                    CategoryDto = result.Data,
                    CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto),
                });
                return Json(categoryAddAjaxModel);
            }
        }
        var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
        {
            CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto),
        });
        return Json(categoryAddAjaxErrorModel);
    }

    public async Task<JsonResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllByNonDeleted();
        var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return Json(categories);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int categoryId)
    {
        var result = await _categoryService.Delete(categoryId, "Gokhan Gok");
        var deletedCategory = JsonSerializer.Serialize(result.Data);
        return Json(deletedCategory);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int categoryId)
    {
        var result = await _categoryService.GetCategoryUpdateDto(categoryId);
        if (result.ResultStatus == ResultStatus.Success)
        {
            return PartialView("_CategoryUpdatePartial", result.Data);
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.Update(categoryUpdateDto, "Gokhan Gok");
            if (result.ResultStatus == ResultStatus.Success)
            {
                var categoryUpdateAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                {
                    CategoryDto = result.Data,
                    CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto),
                });
                return Json(categoryUpdateAjaxModel);
            }
        }

        var categoryUpdateAjaxErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
        {
            CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
        });
        return Json(categoryUpdateAjaxErrorModel);
    }
}
