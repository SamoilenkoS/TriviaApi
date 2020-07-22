using MongoDB.Bson;
using System;

namespace Infrastructure.Models
{
    public class Player : IModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime LastGameDate { get; set; }
        public bool IsGameOrganizer { get; set; }
        public ObjectId ConnectionId { get; set; }
        public CharacterColor CharacterColor { get; set; }
    }
}
