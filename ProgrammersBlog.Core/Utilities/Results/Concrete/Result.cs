
using ProgrammersBlog.Core.Entities.Concrete;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Core.Utilities.Results.Concrete;

public class Result : IResult
{
    public Result(ResultStatus resultStatus)
    {
        ResultStatus = resultStatus;
    }
    public Result(ResultStatus resultStatus, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        ValidationErrors = validationErrors;
    }
    public Result(ResultStatus resultStatus, string message)
    {
        ResultStatus = resultStatus;
        Message = message;
    }
    public Result(ResultStatus resultStatus, string message, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        Message = message;
        ValidationErrors = validationErrors;
    }
    public Result(ResultStatus resultStatus, string message, Exception exception)
    {
        ResultStatus = resultStatus;
        Message = message;
        Exception = exception;
    }
    public Result(ResultStatus resultStatus, string message, Exception exception, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        Message = message;
        Exception = exception;
        ValidationErrors = validationErrors;
    }
    public ResultStatus ResultStatus { get; }
    public string Message { get; }
    public Exception? Exception { get; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
}
