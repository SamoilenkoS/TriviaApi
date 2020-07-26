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
                    nameof(UICategory.Id), 
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Id.ToString()))
                .ForMember(
                    nameof(UICategory.Name),
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Name))
                .ForMember(
                    nameof(UICategory.Questions),
                    memberConfiguration => memberConfiguration.MapFrom(
                        category => category.Questions.Select(objectId => objectId.ToString()))));
            var mapper = new Mapper(config);
            var dbCategories = await DbConnection.GetAllAsync<Category>(new MongoDB.Bson.BsonDocument());
            return mapper.Map<IEnumerable<UICategory>>(dbCategories);
        }

    }
}
