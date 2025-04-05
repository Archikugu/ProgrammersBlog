using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Core.Entities.Abstract;

public abstract class DtoGetBase
{
    public virtual ResultStatus ResultStatus { get; set; }
    public virtual string Message { get; set; }

}
