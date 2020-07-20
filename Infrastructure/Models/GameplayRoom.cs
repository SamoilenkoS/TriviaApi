using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class GameplayRoom
    {
        public ObjectId Id { get; set; }
        public BsonInt32 MaxPlayers { get; set; }
        public BsonArray Players { get; set; }
    }
}
