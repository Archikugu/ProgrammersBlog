﻿using AutoMapper;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.Business.AutoMapper.Profiles;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<ArticleAddDto, Article>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<ArticleUpdateDto, Article>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));

        CreateMap<Article, ArticleDto>();
        CreateMap<Article, ArticleListDto>();
    }
}
