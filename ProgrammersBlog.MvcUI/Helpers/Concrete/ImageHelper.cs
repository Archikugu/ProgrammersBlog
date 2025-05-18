using ProgrammersBlog.Core.Utilities.Extensions;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Dtos.UserDtos;
using ProgrammersBlog.MvcUI.Helpers.Abstract;

namespace ProgrammersBlog.MvcUI.Helpers.Concrete;

public class ImageHelper : IImageHelper
{
    private readonly IWebHostEnvironment _env;
    private readonly string _wwwroot;
    private const string imgFolder = "images";
    private const string userImagesFolder = "userImages";
    private const string postImagesFolder = "postImages";

    public ImageHelper(IWebHostEnvironment env)
    {
        _env = env;
        // `/img/user.Picture
        _wwwroot = _env.WebRootPath;
    }

    public async Task<IDataResult<ImageUploadedDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null)
    {
        folderName ??= pictureType == PictureType.User ? userImagesFolder : postImagesFolder;

        var folderPath = Path.Combine(_wwwroot, imgFolder, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
        string fileExtension = Path.GetExtension(pictureFile.FileName);
        DateTime dateTime = DateTime.Now;
        string newFileName = $"{name}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
        var path = Path.Combine(_wwwroot, imgFolder, folderName, newFileName);

        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await pictureFile.CopyToAsync(stream);
        }

        string possessiveName = name.EndsWith("s") ? $"{name}'" : $"{name}'s";
        string message = pictureType == PictureType.User
            ? $"{possessiveName} profile picture has been successfully uploaded."
            : $"{possessiveName} post picture has been successfully uploaded.";

        return new DataResult<ImageUploadedDto>(ResultStatus.Success, message, new ImageUploadedDto
        {
            FullName = $"{folderName}/{newFileName}",
            OldName = oldFileName,
            Extension = fileExtension,
            Path = path,
            FolderName = folderName,
            Size = pictureFile.Length
        });
    }

    public IDataResult<ImageDeletedDto> Delete(string pictureName)
    {

        var fileToDelete = Path.Combine(_wwwroot, imgFolder, pictureName);
        if (System.IO.File.Exists(fileToDelete))
        {
            var fileInfo = new FileInfo(fileToDelete);
            var imageDeletedDto = new ImageDeletedDto
            {
                FullName = pictureName,
                Extension = fileInfo.Extension,
                Path = fileInfo.FullName,
                Size = fileInfo.Length
            };

            System.IO.File.Delete(fileToDelete);
            return new DataResult<ImageDeletedDto>(ResultStatus.Success, imageDeletedDto);
        }
        else
        {
            return new DataResult<ImageDeletedDto>(ResultStatus.Error, "File not found", null);
        }
    }

}
