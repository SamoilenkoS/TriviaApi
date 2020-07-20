using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Question
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public BsonArray Answers { get; set; }
        public string Category { get; set; }
    }
}
