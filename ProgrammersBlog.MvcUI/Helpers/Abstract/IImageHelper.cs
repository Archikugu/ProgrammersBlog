using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.MvcUI.Helpers.Abstract;

public interface IImageHelper
{
    Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "userImages");
    IDataResult<ImageDeletedDto> Delete(string pictureName);
}
