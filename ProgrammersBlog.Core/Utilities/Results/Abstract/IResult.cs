using ProgrammersBlog.Core.Entities.Concrete;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Core.Utilities.Results.Abstract;

public interface IResult
{
    public ResultStatus ResultStatus { get; }
    public string Message { get;  }
    public Exception? Exception { get;  }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
}
