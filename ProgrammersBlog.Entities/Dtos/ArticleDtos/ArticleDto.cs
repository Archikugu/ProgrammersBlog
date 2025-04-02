using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.ArticleDtos;

public class ArticleDto : DtoGetBase
{
    public Article Article { get; set; }
    //public override ResultStatus ResultStatus { get; set; } = ResultStatus.Success;
}