﻿using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Core.Entities.Abstract;

public abstract class DtoGetBase
{
    public virtual ResultStatus ResultStatus { get; set; }
    public virtual string Message { get; set; }

    public virtual int CurrentPage { get; set; } = 1;
    public virtual int PageSize { get; set; } = 5;
    public virtual int  TotalCount { get; set; }
    public virtual int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
    public virtual bool ShowPrevious => CurrentPage > 1;
    public virtual bool ShowNext => CurrentPage < TotalPages;
    public virtual bool IsAscending { get; set; } = false;
}
