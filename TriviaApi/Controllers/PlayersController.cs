using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TriviaApi.UIModels;

namespace TriviaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        [HttpGet("leaderboard/{daysPeriod}")]
        public async Task<IEnumerable<UIPlayer>> GetRecentlyPlayedPlayers(int daysPeriod)
        {
            var config = new MapperConfiguration(mapperConfiguration => mapperConfiguration.CreateMap<Player, UIPlayer>()
                .ForMember(
                    uiCategory => uiCategory.Id, 
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Id.ToString()))
                .ForMember(
                    uiCategory => uiCategory.Name,
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Name)));
            var mapper = new Mapper(config);
            
            var filter = new BsonDocument(nameof(Player.LastGameDate), new BsonDocument("$gt", DateTime.Now.Subtract(TimeSpan.FromDays(daysPeriod))));

            var dbPlayers = await Infrastructure.DbConnection.GetAllAsync<Player>(filter);

            return mapper.Map<IEnumerable<UIPlayer>>(dbPlayers);
        }
    }
}
