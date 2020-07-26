using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using TriviaApi.UIModels;

namespace TriviaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<UICategory>> Get()
        {
            var config = new MapperConfiguration(mapperConfiguration => mapperConfiguration.CreateMap<Category, UICategory>()
                .ForMember(
                    uiCategory => uiCategory.Id, 
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Id.ToString()))
                .ForMember(
                    uiCategory => uiCategory.Name,
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Name)));
            var mapper = new Mapper(config);

            var dbCategories = await DbConnection.GetAllAsync<Category>(new MongoDB.Bson.BsonDocument());

            return mapper.Map<IEnumerable<UICategory>>(dbCategories);
        }

    }
}
