using ProgrammersBlog.Core.Entities.Concrete;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Core.Utilities.Results.Concrete;

public class DataResult<T> : IDataResult<T>
{
    public DataResult(ResultStatus resultStatus, T data)
    {
        ResultStatus = resultStatus;
        Data = data;
    }
    public DataResult(ResultStatus resultStatus, T data, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        Data = data;
        ValidationErrors = validationErrors;
    }
    public DataResult(ResultStatus resultStatus, string message, T data)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
    }
    public DataResult(ResultStatus resultStatus, string message, T data, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
        ValidationErrors = validationErrors;
    }
    public DataResult(ResultStatus resultStatus, string message, T data, Exception exception)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
        Exception = exception;
    }
    public DataResult(ResultStatus resultStatus, string message, T data, Exception exception, IEnumerable<ValidationError> validationErrors)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
        Exception = exception;
        ValidationErrors = validationErrors;
    }
    public ResultStatus ResultStatus { get; }
    public string Message { get; }
    public Exception? Exception { get; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
    public T Data { get; }
}
