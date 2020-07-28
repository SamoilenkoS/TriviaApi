using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Models;
using MongoDB.Bson;

namespace Infrastructure
{
    public class ModelFactory
    {
        public static IEnumerable<Player> CreatePlayers(int count)
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
                    CharacterColor = colors.GetValue(random.Next(colors.Length)).ToString()
                };

                players.Add(player);
            }

            return players;
        }

        public static IEnumerable<Category> CreateCategories(int count)
        {
            var categories = new List<Category>();
            for (int i = 0; i < count; i++)
            {
                var category = new Category
                {
                    Name = StringExtension.GetRandomString(10)
                };
                categories.Add(category);
            }

            return categories;
        }

        public static (Question question, IEnumerable<Answer> answers) CreateQuestionWithAnswers(int questionNumber, string categoryTitle)
        {
            var random = new Random();
            var answersCount = 4;
            var answerLength = 5;
            var answers = new List<Answer>();
            var correctAnswerIndex = random.Next(0, answersCount);
            for (int j = 0; j < answersCount; j++)
            {
                answers.Add(
                    new Answer
                    {
                        Text = StringExtension.GetRandomString(answerLength),
                        IsCorrect = j == correctAnswerIndex
                    });
            }
            var question = new Question
            {
                Text = $"This is the {questionNumber} question for the category {categoryTitle}",
                Category = categoryTitle
            };

            return (question, answers);
        }

        public static GameplayRoom CreateGameplayRoom(string playerId)
        {
            return new GameplayRoom
            {
                MaxPlayers = 2,
                Players = new List<string>
                {
                    playerId
                }
            };
        }
    }
}
