using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.UserDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;
using ProgrammersBlog.MvcUI.Helpers.Abstract;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IImageHelper _imageHelper;
    private readonly IToastNotification _toastNotification;
    public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper, IToastNotification toastNotification)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _imageHelper = imageHelper;
        _toastNotification = toastNotification;
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(new UserListDto
        {
            Users = users,
            ResultStatus = ResultStatus.Success,
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<JsonResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var userlistDto = JsonSerializer.Serialize(new UserListDto
        {
            Users = users,
            ResultStatus = ResultStatus.Success,
        },
        new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        });
        return Json(userlistDto);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Add()
    {
        return PartialView("_UserAddPartial");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(UserAddDto userAddDto)
    {
        if (ModelState.IsValid)
        {
            var uploadedImageDtoResult = await _imageHelper.Upload(userAddDto.UserName, userAddDto.PictureFile, PictureType.User);
            userAddDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success
                ? uploadedImageDtoResult.Data.FullName
                : "userImages/defaultUser.png";

            var user = _mapper.Map<User>(userAddDto);
            var result = _userManager.CreateAsync(user, userAddDto.Password).Result;
            if (result.Succeeded)
            {
                var userAddAjaxVM = JsonSerializer.Serialize(new UserAddAjaxViewModel
                {
                    UserDto = new UserDto
                    {
                        ResultStatus = ResultStatus.Success,
                        Message = $"{userAddDto.UserName} has been successfully added.",
                        User = user
                    },
                    UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto),
                });
                return Json(userAddAjaxVM);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                {
                    UserAddDto = userAddDto,
                    UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto),
                });
                return Json(userAddAjaxErrorModel);
            }
        }
        var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
        {
            UserAddDto = userAddDto,
            UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto),
        });
        return Json(userAddAjaxModelStateErrorModel);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _imageHelper.Delete(user.Picture);
            var deleteUser = JsonSerializer.Serialize(new UserDto
            {
                ResultStatus = ResultStatus.Success,
                Message = $"{user.UserName} has been successfully deleted.",
                User = user
            });
            return Json(deleteUser);
        }
        else
        {
            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error.Description}\n";
            }
            var deleteUserErrorModel = JsonSerializer.Serialize(new UserDto
            {
                ResultStatus = ResultStatus.Error,
                Message = $"An error occurred while deleting {user.UserName}.\n{errorMessages}",
                User = user
            });
            return Json(deleteUserErrorModel);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<PartialViewResult> Update(int userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
        return PartialView("_UserUpdatePartial", userUpdateDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
    {
        if (ModelState.IsValid)
        {
            bool isNewPictureUploaded = false;
            var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
            var oldUserPicture = oldUser.Picture;
            if (userUpdateDto.PictureFile != null)
            {
                var uploadedImageDtoResult = await _imageHelper.Upload(userUpdateDto.UserName, userUpdateDto.PictureFile, PictureType.User);
                userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success
                    ? uploadedImageDtoResult.Data.FullName
                    : "userImages/defaultUser.png";

                if (oldUserPicture != "userImages/defaultUser.png")
                {
                    isNewPictureUploaded = true;
                }
            }

            var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
            var result = await _userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                if (isNewPictureUploaded)
                {
                    _imageHelper.Delete(oldUserPicture);
                }
                var userUpdateVM = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserDto = new UserDto
                    {
                        ResultStatus = ResultStatus.Success,
                        Message = $"{updatedUser.UserName} has been successfully updated.",
                        User = updatedUser
                    },
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
                });
                return Json(userUpdateVM);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
                });
                return Json(userUpdateErrorViewModel);
            }
        }
        else
        {
            var userUpdateModelStateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
            {
                UserUpdateDto = userUpdateDto,
                UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto),
            });
            return Json(userUpdateModelStateErrorViewModel);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View("UserLogin");
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View("UserLogin");
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
                return View("UserLogin");
            }
        }
        else
        {
            return View("UserLogin");
        }
    }

    [HttpGet]
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { Area = "" });
    }

    [Authorize]
    [HttpGet]
    public async Task<ViewResult> ChangeDetails()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
        return View(userUpdateDto);
    }

    [Authorize]
    [HttpPost]
    public async Task<ViewResult> ChangeDetails(UserUpdateDto userUpdateDto)
    {
        if (ModelState.IsValid)
        {
            bool isNewPictureUploaded = false;
            var oldUser = await _userManager.GetUserAsync(HttpContext.User);
            var oldUserPicture = oldUser.Picture;
            if (userUpdateDto.PictureFile != null)
            {
                var uploadedImageDtoResult = await _imageHelper.Upload(userUpdateDto.UserName, userUpdateDto.PictureFile, PictureType.User);
                userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success
                    ? uploadedImageDtoResult.Data.FullName
                    : "userImages/defaultUser.png";

                if (oldUserPicture != "userImages/defaultUser.png")
                {
                    isNewPictureUploaded = true;
                }
            }

            var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
            var result = await _userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                if (isNewPictureUploaded)
                {
                    _imageHelper.Delete(oldUserPicture);
                }

                _toastNotification.AddWarningToastMessage($"{updatedUser.UserName} has been successfully updated.", new ToastrOptions
                {
                    Title = "User Change Details Succesfuly"
                });

                return View(userUpdateDto);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userUpdateDto);
            }
        }
        else
        {
            return View(userUpdateDto);
        }
    }

    [Authorize]
    [HttpGet]
    public ViewResult PasswordChange()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
            if (isVerified)
            {
                var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                    _toastNotification.AddSuccessToastMessage($"Your password has been successfully changed.", new ToastrOptions
                    {
                        Title = "User Password Change Succesfuly"
                    });
                    return View();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                ModelState.AddModelError("", "Current password is incorrect.");
                return View(userPasswordChangeDto);
            }
        }
        else
        {
            return View(userPasswordChangeDto);
        }
    }
}
