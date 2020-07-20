using System;
using MongoDB.Driver;

namespace Infrastructure
{
    public class DbConnection
    {
        public void Connect()
        {
            var connectionString = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("TriviaDB");
            var dbList = mongoClient.ListDatabases().ToList();
            foreach (var db in dbList)
            {
                
            }
        }
    }
}
