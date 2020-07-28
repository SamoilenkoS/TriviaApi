using Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using MongoDB.Bson;

namespace TriviaApi.SignalRHubs
{
    public class TriviaHub : Hub
    {
        public async Task Send(string jsonData)
        { 
            var categories = await Infrastructure.DbConnection.GetAllAsync<Category>(new MongoDB.Bson.BsonDocument());
        }

        public async Task Leave()
        {

        }

        public async Task Join(string characterColor)
        {
            var rooms = await DbConnection.GetAllAsync<GameplayRoom>(new BsonDocument());
            if (!rooms.Any())//no room
            {
                var firstPlayer = await DbConnection.GetAsync<Player>(
                    new BsonDocument("ConnectionId", Context.ConnectionId));
                firstPlayer = await CreateOrUpdatePlayer(firstPlayer, characterColor, true);

                var room = ModelFactory.CreateGameplayRoom(firstPlayer.ConnectionId);
                await DbConnection.SaveAsync(room);//creating room
            }
            else//room exist
            {
                var gameplayRoom = rooms.First();
                var secondPlayer = await DbConnection.GetAsync<Player>(
                    new BsonDocument("ConnectionId", Context.ConnectionId));
                secondPlayer = await CreateOrUpdatePlayer(secondPlayer, characterColor, false);

                var players = new List<string>(gameplayRoom.Players)
                {
                    secondPlayer.ConnectionId
                };
                gameplayRoom.Players = players;
                await DbConnection.UpdateAsync(gameplayRoom);
                var firstPlayer = await DbConnection.GetAsync<Player>(new BsonDocument("ConnectionId", players[0]));
                await Clients.Client(secondPlayer.ConnectionId).SendAsync(
                    "OpponentJoined",
                    firstPlayer.Name,
                    firstPlayer.CharacterColor,
                    firstPlayer.IsGameOrganizer);
                await Clients.Client(firstPlayer.ConnectionId).SendAsync(
                    "OpponentJoined",
                    secondPlayer.Name,
                    secondPlayer.CharacterColor,
                    secondPlayer.IsGameOrganizer);
                await Clients.Client(secondPlayer.ConnectionId).SendAsync("CanPlay");
                await Clients.Client(firstPlayer.ConnectionId).SendAsync("CanPlay");
            }

        }

        private async Task<Player> CreateOrUpdatePlayer(Player player, string characterColor, bool isGameOrganizer)
        {
            if (player == null)
            {
                player = new Player
                {
                    Name = $"Player_{StringExtension.GetRandomString(5)}",
                    ConnectionId = Context.ConnectionId,
                    IsGameOrganizer = isGameOrganizer,
                    CharacterColor = characterColor,
                    LastGameDate = DateTime.Now,
                    Score = 0
                };

                await DbConnection.SaveAsync(player);
            }
            else
            {
                player.IsGameOrganizer = isGameOrganizer;
                player.CharacterColor = characterColor;
                player.LastGameDate = DateTime.Now;
                await DbConnection.UpdateAsync(player);
            }

            return player;
        }

    }
}
