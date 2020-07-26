using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            mongoClient.DropDatabase("TriviaDB", CancellationToken.None);
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
            return items.FirstOrDefault();
        }

        public static async Task SaveAsync<T>(T item) where T : IModel
        {
            var collection = mongoDatabase.GetCollection<T>(item.GetType().ToString());
            await collection.InsertOneAsync(item);
        }

        public static async Task SaveAsync<T>(IEnumerable<T> items) where T : IModel
        {
            var collection = mongoDatabase.GetCollection<T>(items.First().GetType().ToString());
            await collection.InsertManyAsync(items);
        }

        public static async Task UpdateAsync<T>(T item) where T : IModel
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", item.Id);
            var items = mongoDatabase.GetCollection<BsonDocument>(item.ToString());
            await items.ReplaceOneAsync(filter, item.ToBsonDocument());
        }

        private static async Task Initialize()
        {
            var emptyFilter = new BsonDocument();
            var players = await GetAllAsync<Player>(emptyFilter);
            if(!players.Any())
            {
                await CreatePlayers(100);
            }

            var categories = await GetAllAsync<Category>(emptyFilter);
            if(!categories.Any())
            {
                categories = await CreateCategories(10);
            }

            foreach (var category in categories)
            {
                var categoryFilter = new BsonDocument(nameof(Question.Category), category.Name);
                var categoryQuestions = await GetAllAsync<Question>(categoryFilter);
                if (!categoryQuestions.Any())
                {
                    var questions = await CreateQuestionsWithAnswers(50, category.Name);
                    category.Questions = questions.Select(question => question.Id);
                    await UpdateAsync(category);
                }
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

        private static async Task<IEnumerable<Category>> CreateCategories(int count)
        {
            var categories = new List<Category>();
            for (int i = 0; i < count; i++)
            {
                var category = new Category
                {
                    Name = GetRandomString(10)
                };
                categories.Add(category);
            }

            await SaveAsync(categories);

            return categories;
        }

        private static async Task<IEnumerable<Question>> CreateQuestionsWithAnswers(int count, string categoryTitle)
        {
            var random = new Random();
            var questions = new List<Question>();
            var answersCount = 4;
            var answerLength = 5;
            for (int i = 0; i < count; i++)
            {
                var answers = new List<Answer>();
                var correctAnswerIndex = random.Next(0, answersCount);
                for (int j = 0; j < answersCount; j++)
                {
                    answers.Add(
                        new Answer
                        {
                            Text = GetRandomString(answerLength),
                            IsCorrect = j == correctAnswerIndex
                        });
                }

                await SaveAsync(answers);

                var question = new Question
                {
                    Text = $"This is the {i} question for the category {categoryTitle}",
                    Category = categoryTitle,
                    Answers = answers.Select(answer => answer.Id)
                };

                questions.Add(question);
            }

            await SaveAsync(questions);

            return questions;
        }

        private static string GetRandomString(int length)
        {
            var random = new Random();
            var randomString = new StringBuilder();
            var charRange = 'z' - 'A';
            for (int i = 0; i < length; i++)
            {
                var randomChar = (char) ('A' + random.Next(0, charRange));
                randomString.Append(randomChar);
            }

            return randomString.ToString();
        }
    }
}
