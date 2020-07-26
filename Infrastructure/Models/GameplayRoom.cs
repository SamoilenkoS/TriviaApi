using System.Collections.Generic;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class GameplayRoom : IModel
    {
        public ObjectId Id { get; set; }
        public int MaxPlayers { get; set; }
        public IEnumerable<ObjectId> Players { get; set; }
    }
}
