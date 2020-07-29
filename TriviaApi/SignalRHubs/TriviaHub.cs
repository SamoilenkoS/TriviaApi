using Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using MongoDB.Bson;
using Serilog;

namespace TriviaApi.SignalRHubs
{
    public class TriviaHub : Hub
    {
        private readonly IDbConnection _dbConnection;
        private readonly IModelFactory _modelFactory;
        public TriviaHub(IDbConnection dbConnection, IModelFactory modelFactory)
        {
            _dbConnection = dbConnection;
            _modelFactory = modelFactory;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Leave();
        }

        public async Task Send(string jsonData)
        {
            Log.Logger.Information($"Send: {jsonData}");
            var gameplayRoom = await GetPlayerRoom();
            var anotherPlayerIds = gameplayRoom.Players.Except(new List<string> {Context.ConnectionId});
            foreach (var playerId in anotherPlayerIds)
            {
                await Clients.Client(playerId).SendAsync("Send", jsonData);
            }
        }

        public async Task Join(string characterColor)
        {
            Log.Logger.Information($"Join: {Context.ConnectionId} {characterColor}");
            var rooms = await _dbConnection.GetAllAsync<GameplayRoom>(new BsonDocument());
            if (!rooms.Any())//no room
            {
                var firstPlayer = await _dbConnection.GetAsync<Player>(
                    new BsonDocument("ConnectionId", Context.ConnectionId));
                firstPlayer = await CreateOrUpdatePlayer(firstPlayer, characterColor, true);

                var room = _modelFactory.CreateGameplayRoom(firstPlayer.ConnectionId);
                await _dbConnection.SaveAsync(room);//creating room
                Log.Logger.Information($"User {Context.ConnectionId} added to room {room.Id}");
            }
            else//room exist
            {
                var gameplayRoom = rooms.First(room => room.Players.Count() < room.MaxPlayers);
                var secondPlayer = await _dbConnection.GetAsync<Player>(
                    new BsonDocument("ConnectionId", Context.ConnectionId));
                secondPlayer = await CreateOrUpdatePlayer(secondPlayer, characterColor, false);

                var players = new List<string>(gameplayRoom.Players)
                {
                    secondPlayer.ConnectionId
                };
                gameplayRoom.Players = players;
                await _dbConnection.UpdateAsync(gameplayRoom);
                Log.Logger.Information($"User {Context.ConnectionId} added to room {gameplayRoom.Id}");
                var firstPlayer = await _dbConnection.GetAsync<Player>(new BsonDocument("ConnectionId", players[0]));
                await NotifyOpponentJoined(firstPlayer, secondPlayer);
                if (gameplayRoom.Players.Count() == gameplayRoom.MaxPlayers)
                {
                    foreach (var playerId in gameplayRoom.Players)
                    {
                        await Clients.Client(playerId).SendAsync("CanPlay");
                    }
                }
            }

        }

        public async Task Leave()
        {
            Log.Logger.Information($"Leave: {Context.ConnectionId}");
            var gameplayRoom = await GetPlayerRoom();
            if (gameplayRoom == null)
            {
                return;
            }
            var anotherPlayerIds = gameplayRoom.Players.Except(new List<string> {Context.ConnectionId});
            foreach (var playerId in anotherPlayerIds)
            {
                await Clients.Client(playerId).SendAsync("OpponentLeave");
            }

            await _dbConnection.RemoveAsync<GameplayRoom>(new BsonDocument("_id", gameplayRoom.Id));
        }

        private async Task NotifyOpponentJoined(Player firstPlayer, Player secondPlayer)
        {
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

                await _dbConnection.SaveAsync(player);
            }
            else
            {
                player.IsGameOrganizer = isGameOrganizer;
                player.CharacterColor = characterColor;
                player.LastGameDate = DateTime.Now;
                await _dbConnection.UpdateAsync(player);
            }

            return player;
        }

        private async Task<GameplayRoom> GetPlayerRoom()
        {
            var roomFilter = new BsonDocument("Players", Context.ConnectionId);
            return await _dbConnection.GetAsync<GameplayRoom>(roomFilter);
        }

    }
}
