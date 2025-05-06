using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;

namespace ProgrammersBlog.Business.Concrete;

public class CommentManager : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IDataResult<int>> Count()
    {
        var commentsCount = await _unitOfWork.Comments.CountAsync();
        if (commentsCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, commentsCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }

    public async Task<IDataResult<int>> CountByIsDeleted()
    {
        var commentsCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);
        if (commentsCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, commentsCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }
}
