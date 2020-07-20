using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Category
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public BsonArray Questions { get; set; }
    }
}
