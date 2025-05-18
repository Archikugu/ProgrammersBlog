using AutoMapper;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.ArticleViewModel;

namespace ProgrammersBlog.MvcUI.AutoMapper;

public class ViewModelsProfile : Profile
{
    public ViewModelsProfile()
    {
        CreateMap<ArticleAddViewModel, ArticleAddDto>();
        CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
    }
}
