using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.MvcUI.Helpers.Abstract;

public interface IImageHelper
{
    Task<IDataResult<ImageUploadedDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null);
    IDataResult<ImageDeletedDto> Delete(string pictureName);
}
