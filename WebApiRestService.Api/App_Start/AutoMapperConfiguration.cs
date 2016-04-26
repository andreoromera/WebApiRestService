using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using api = WebApiRestService.Api.Database;
using dto = WebApiRestService.Dto;

namespace WebApiRestService.Api
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<api.Restaurant, dto.Restaurant>();
            Mapper.CreateMap<api.Country, dto.Country>();
        }
    }
}