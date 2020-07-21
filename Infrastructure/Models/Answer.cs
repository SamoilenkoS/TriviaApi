using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Answer : IModel
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public  bool IsCorrect { get; set; }
    }
}
