using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Player
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public BsonInt32 Score { get; set; }
        public BsonDateTime LastGameDate { get; set; }
        public bool IsGameOrganizer { get; set; }
        public ObjectId ConnectionId { get; set; }
        public string CharacterColor { get; set; }
    }
}
