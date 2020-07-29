using System.Collections.Generic;
using Infrastructure.Models;

namespace Infrastructure
{
    public interface IModelFactory
    {
        IEnumerable<Player> CreatePlayers(int count);
        IEnumerable<Category> CreateCategories(int count);
        GameplayRoom CreateGameplayRoom(string playerId);
        (Question question, IEnumerable<Answer> answers) CreateQuestionWithAnswers(int questionNumber,
            string categoryTitle);
    }
}
