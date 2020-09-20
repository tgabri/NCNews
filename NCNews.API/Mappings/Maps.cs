using AutoMapper;
using NCNews.API.DTOs;
using NCNews.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();

            CreateMap<Topic, TopicDTO>().ReverseMap();
            CreateMap<Topic, TopicCreateDTO>().ReverseMap();
            CreateMap<Topic, TopicUpdateDTO>().ReverseMap();

            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<Article, ArticleCreateDTO>().ReverseMap();
            CreateMap<Article, ArticleUpdateDTO>().ReverseMap();
        }
    }
}
