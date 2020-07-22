using System.Collections.Generic;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Question : IModel
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}
