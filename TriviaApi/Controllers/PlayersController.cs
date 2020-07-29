using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure;
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
        private readonly IDbConnection _dbConnection;
        public PlayersController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet("leaderboard/{daysPeriod}")]
        public async Task<IEnumerable<UIPlayer>> GetRecentlyPlayedPlayers(int daysPeriod)
        {
            var config = new MapperConfiguration(mapperConfiguration => mapperConfiguration.CreateMap<Player, UIPlayer>()
                .ForMember(
                    uiCategory => uiCategory.Id, 
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Id.ToString()))
                .ForMember(
                    uiCategory => uiCategory.Name,
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Name))
                .ForMember(
                    uiCategory => uiCategory.Score,
                    memberConfiguration => memberConfiguration.MapFrom(category => category.Score))
                .ForMember(
                    uiCategory => uiCategory.LastGameDate,
                    memberConfiguration => memberConfiguration.MapFrom(category => category.LastGameDate)));
            var mapper = new Mapper(config);
            
            var filter = new BsonDocument(nameof(Player.LastGameDate), new BsonDocument("$gt", DateTime.Now.Subtract(TimeSpan.FromDays(daysPeriod))));

            var dbPlayers = await _dbConnection.GetAllAsync<Player>(filter);

            return mapper.Map<IEnumerable<UIPlayer>>(dbPlayers);
        }
    }
}
