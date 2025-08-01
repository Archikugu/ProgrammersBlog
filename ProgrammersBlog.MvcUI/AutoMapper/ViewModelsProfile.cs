﻿using AutoMapper;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;
using ProgrammersBlog.MvcUI.Areas.Admin.Models.ArticleViewModels;

namespace ProgrammersBlog.MvcUI.AutoMapper;

public class ViewModelsProfile : Profile
{
    public ViewModelsProfile()
    {
        CreateMap<ArticleAddViewModel, ArticleAddDto>();
        CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
        CreateMap<ArticleRightSideBarWidgetOptions, ArticleRightSideBarWidgetOptionsViewModel>().ReverseMap();
    }
}
