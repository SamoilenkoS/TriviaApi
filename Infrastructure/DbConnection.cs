using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Infrastructure
{
    public class DbConnection
    {
        private static readonly IMongoDatabase mongoDatabase;

        static DbConnection()
        {
            var connectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            mongoDatabase = mongoClient.GetDatabase("TriviaDB");
        }

        public static async Task<IEnumerable<T>> GetAllAsync<T>(BsonDocument filter) where T : IModel
        {
            var collection = mongoDatabase.GetCollection<T>(typeof(T).ToString());
            return await collection.Find(filter).ToListAsync();
        }

        public static async Task<T> GetAsync<T>(ObjectId id) where T : IModel
        {
            var filter = new BsonDocument(nameof(IModel.Id), id.ToString());
            var items = await GetAllAsync<T>(filter);
            return items.Count() != 0 ? items.First() : default;
        }

        public static async Task SaveAsync<T>(T item) where T : IModel
        {
            var collection = mongoDatabase.GetCollection<T>(item.GetType().ToString());
            await collection.InsertOneAsync(item);
        }

        public static async Task UpdateAsync<T>(T item) where T : IModel
        {
            var filter = new BsonDocument(nameof(IModel.Id), item.Id.ToString());
            var items = mongoDatabase.GetCollection<BsonDocument>(typeof(T).ToString());
            await items.ReplaceOneAsync(filter, item.ToBsonDocument());
        }
    }
}
