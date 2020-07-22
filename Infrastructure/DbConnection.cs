using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using MongoDB.Bson;
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
            Initialize();
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

        public static async Task SaveAsync<T>(IEnumerable<T> items) where T : IModel
        {
            var collection = mongoDatabase.GetCollection<T>(items.ToList()[0].GetType().ToString());
            await collection.InsertManyAsync(items);
        }

        public static async Task UpdateAsync<T>(T item) where T : IModel
        {
            var filter = new BsonDocument(nameof(IModel.Id), item.Id.ToString());
            var items = mongoDatabase.GetCollection<BsonDocument>(typeof(T).ToString());
            await items.ReplaceOneAsync(filter, item.ToBsonDocument());
        }

        private static async Task Initialize()
        {
            var emptyFilter = new BsonDocument();
            var players = await GetAllAsync<Player>(emptyFilter);
            if(players.Count() == 0)
            {
                await CreatePlayers(100);
            }

            var categories = await GetAllAsync<Category>(emptyFilter);
            if(categories.Count() == 0)
            {
                await CreateCategories(10);
            }
        }

        private static async Task CreatePlayers(int count)
        {
            var random = new Random();
            var colors = Enum.GetValues(typeof(CharacterColor));
            var players = new List<Player>();
            for (int i = 0; i < count; i++)
            {
                var player = new Player
                {
                    Score = random.Next(0, 700),
                    Name = $"TestPlayer_{i}",
                    LastGameDate = DateTime.Now.Subtract(TimeSpan.FromDays(random.Next(0,60))),
                    CharacterColor = (CharacterColor)colors.GetValue(random.Next(colors.Length))
                };

                players.Add(player);
            }
            await SaveAsync(players);

        }

        private static async Task CreateCategories(int count)
        {
            var categories = new List<Category>();
            for (int i = 0; i < count; i++)
            {
                var category = new Category
                {
                    Name = GetRandomString(10)
                };
                var questions = await CreateQuestions(50, category.Name);
                category.Questions = questions;
                categories.Add(category);
            }

            await SaveAsync(categories);
        }

        private static async Task<IEnumerable<Question>> CreateQuestions(int count, string categoryTitle)
        {
            var random = new Random();
            var questions = new List<Question>();
            for (int i = 0; i < count; i++)
            {
                var answers = new List<Answer>();
                var correctAnswerIndex = random.Next(0, 4);
                for (int j = 0; j < 4; j++)
                {
                    answers.Add(
                        new Answer
                        {
                            Text = GetRandomString(5),
                            IsCorrect = j == correctAnswerIndex
                        });
                }

                var question = new Question
                {
                    Text = $"This is the {i} question for the category {categoryTitle}",
                    Category = categoryTitle,
                    Answers = answers
                };

                questions.Add(question);
            }

            return questions;
        }

        private static string GetRandomString(int length)
        {
            Random random = new Random();
            var randomString = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var randomChar = 'A';
                randomChar = (char)(randomChar + random.Next(0, 52)); //26 letters * 2 types of register (upper and lower)
                randomString.Append(randomChar);
            }

            return randomString.ToString();
        }
    }
}
