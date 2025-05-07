using ProgrammersBlog.Core.Utilities.Results.Abstract;

namespace ProgrammersBlog.Business.Abstract;

public interface ICommentService
{
    Task<IDataResult<int>> CountAsync();
    Task<IDataResult<int>> CountByNonDeletedAsync();


}
