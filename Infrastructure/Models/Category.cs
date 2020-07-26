using System.Collections.Generic;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Category : IModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ObjectId> Questions { get; set; }
    }
}
