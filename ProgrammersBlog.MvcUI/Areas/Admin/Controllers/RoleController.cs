﻿using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.RoleDtos;
using ProgrammersBlog.Entities.Dtos.UserDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;
using ProgrammersBlog.MvcUI.Helpers.Abstract;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class RoleController : BaseController
{
    private readonly RoleManager<Role> _roleManager;

    public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
    {
        _roleManager = roleManager;
    }

    [Authorize(Roles = "SuperAdmin,Role.Read")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(new RoleListDto
        {
            Roles = roles
        });
    }

    [Authorize(Roles = "SuperAdmin,Role.Read")]
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var roleListDto = JsonSerializer.Serialize(new RoleListDto
        {
            Roles = roles
        });
        return Json(roleListDto);
    }

    [Authorize(Roles = "SuperAdmin,User.Update")]
    [HttpGet]
    public async Task<IActionResult> Assign(int userId)
    {
        var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        var roles = await _roleManager.Roles.ToListAsync();
        var userRoles = await UserManager.GetRolesAsync(user);

        UserRoleAssignDto userRoleAssignDto = new UserRoleAssignDto
        {
            UserId = user.Id,
            UserName = user.UserName
        };

        foreach (var role in roles)
        {
            RoleAssignDto roleAssignDto = new RoleAssignDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                HasRole = userRoles.Contains(role.Name)
            };
            userRoleAssignDto.RoleAssignDtos.Add(roleAssignDto);
        }
        return PartialView("_RoleAssignPartial", userRoleAssignDto);
    }

    [Authorize(Roles = "SuperAdmin,User.Update")]
    [HttpPost]
    public async Task<IActionResult> Assign(UserRoleAssignDto userRoleAssignDto)
    {
        if (ModelState.IsValid)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userRoleAssignDto.UserId);
            foreach (var roleAssignDto in userRoleAssignDto.RoleAssignDtos)
            {
                if (roleAssignDto.HasRole)
                    await UserManager.AddToRoleAsync(user, roleAssignDto.RoleName);
                else
                {
                    await UserManager.RemoveFromRoleAsync(user, roleAssignDto.RoleName);
                }
            }

            await UserManager.UpdateSecurityStampAsync(user);

            var userRoleAssignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
            {
                UserDto = new UserDto
                {
                    User = user,
                    Message = $"Role assignment process has been successfully completed for user {user.UserName}.",
                    ResultStatus = ResultStatus.Success
                },
                RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto)
            });
            return Json(userRoleAssignAjaxViewModel);
        }
        else
        {
            var userRoleAssignAjaxErrorModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
            {
                RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto),
                UserRoleAssignDto = userRoleAssignDto
            });
            return Json(userRoleAssignAjaxErrorModel);
        }
    }
}
