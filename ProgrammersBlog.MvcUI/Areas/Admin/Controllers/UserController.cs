using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.UserDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;
    public UserController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _userManager = userManager;
        _env = webHostEnvironment;
        _mapper = mapper;
    }

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
    public IActionResult Add()
    {
        return PartialView("_UserAddPartial");
    }

    [HttpPost]
    public async Task<IActionResult> Add(UserAddDto userAddDto)
    {
        if (ModelState.IsValid)
        {
            userAddDto.Picture = await ImageUpload(userAddDto.UserName, userAddDto.PictureFile);
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

    public async Task<IActionResult> Delete(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
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
    public async Task<PartialViewResult> Update(int userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
        return PartialView("_UserUpdatePartial", userUpdateDto);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
    {
        if (ModelState.IsValid)
        {
            bool isNewPictureUploaded = false;
            var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
            var oldUserPicture = oldUser.Picture;
            if (userUpdateDto.PictureFile != null)
            {
                userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                isNewPictureUploaded = true;
            }

            var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
            var result = await _userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                if (isNewPictureUploaded)
                {
                    ImageDelete(oldUserPicture);
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
    public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
    {
        // `/img/user.Picture
        string wwwroot = _env.WebRootPath;
        string fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
        string fileExtension = Path.GetExtension(pictureFile.FileName);
        DateTime dateTime = DateTime.Now;
        string newFileName = $"{userName}_{fileName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
        string path = Path.Combine(wwwroot, "images", newFileName);

        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await pictureFile.CopyToAsync(stream);
        }
        return newFileName;
    }
    public bool ImageDelete(string pictureName)
    {
        string wwwroot = _env.WebRootPath;
        var fileToDelete = Path.Combine(wwwroot, "images", pictureName);
        if (System.IO.File.Exists(fileToDelete))
        {
            System.IO.File.Delete(fileToDelete);
            return true;
        }
        else
        {
            return false;
        }

    }
}
