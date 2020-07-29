using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;
using MongoDB.Bson;

namespace Infrastructure
{
    public interface IDbConnection
    {
        Task Initialize();
        Task<IEnumerable<T>> GetAllAsync<T>(BsonDocument filter) where T : IModel;
        Task<T> GetAsync<T>(ObjectId id) where T : IModel;
        Task<T> GetAsync<T>(BsonDocument filter) where T : IModel;
        Task SaveAsync<T>(T item) where T : IModel;
        Task SaveAsync<T>(IEnumerable<T> items) where T : IModel;
        Task UpdateAsync<T>(T item) where T : IModel;
        Task RemoveAsync<T>(BsonDocument filter) where T : IModel;
    }
}
