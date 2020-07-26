using Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriviaApi.SignalRHubs
{
    public class GameHub : Hub
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
            var color = characterColor;
        }

    }
}
