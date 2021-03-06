﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookConsumerService.Models.DTOs;
using BookConsumerService.Entities;
namespace BookConsumerService.Models.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            //.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            //  .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.Bookgenre.Select(i => i.GenreId)))
            //  .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Bookgenre.Select(i => i.Genre.Name).ToList()));
            CreateMap<BookDto, Book>();
            CreateMap<TaskHistory,TaskDto >();// .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<TaskDto, TaskHistory>();
        }
    }
}
