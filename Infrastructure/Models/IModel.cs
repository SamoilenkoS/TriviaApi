using MongoDB.Bson;

namespace Infrastructure.Models
{
    public interface IModel
    {
        ObjectId Id { get; set; }
    }
}
