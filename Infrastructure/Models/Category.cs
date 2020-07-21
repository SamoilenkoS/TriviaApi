using System.Collections.Generic;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Category : IModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
    }
}
