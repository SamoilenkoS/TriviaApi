using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TriviaApi.UIModels;

namespace TriviaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        [HttpGet("{categoryId}")]
        public async Task<UIQuestion> GetRandomQuestionByCategory(string categoryId)
        {
            var categoryInfo = await Infrastructure.DbConnection.GetAsync<Category>(ObjectId.Parse(categoryId));
            var dbQuestions =
                await Infrastructure.DbConnection.GetAllAsync<Question>(
                    new BsonDocument(
                        nameof(Question.Category),
                        categoryInfo.Name));
            var random = new Random();
            var questionId = random.Next(0, dbQuestions.Count());
            var answers = new List<Answer>();
            var questionToMap = dbQuestions.ToList()[questionId];
            foreach (var answerId in questionToMap.Answers)
            {
                answers.Add(await Infrastructure.DbConnection.GetAsync<Answer>(answerId));
            }

            var config = new MapperConfiguration(mapperConfiguration =>
            {
                mapperConfiguration.CreateMap<Answer, UIAnswer>()
                    .ForMember(
                        uiAnswer => uiAnswer.Id,
                        memberConfiguration => memberConfiguration.MapFrom(answer => answer.Id.ToString()))
                    .ForMember(
                        uiAnswer => uiAnswer.Text,
                        memberConfiguration => memberConfiguration.MapFrom(answer => answer.Text))
                    .ForMember(
                        uiAnswer => uiAnswer.IsCorrect,
                        memberConfiguration => memberConfiguration.MapFrom(answer => answer.IsCorrect));
                mapperConfiguration.CreateMap<Question, UIQuestion>()
                    .ForMember(
                        uiQuestion => uiQuestion.Id,
                        memberConfiguration => memberConfiguration.MapFrom(question => question.Id.ToString()))
                    .ForMember(
                        uiQuestion => uiQuestion.Text,
                        memberConfiguration => memberConfiguration.MapFrom(question => question.Text))
                    .ForMember(
                        uiQuestion => uiQuestion.Answers,
                        memberConfiguration => memberConfiguration.MapFrom(question => new List<UIAnswer>()));
            });
            var mapper = new Mapper(config);

            var mappedQuestion = mapper.Map<UIQuestion>(questionToMap);
            var mappedAnswers = mapper.Map<IEnumerable<UIAnswer>>(answers);
            mappedQuestion.Answers = mappedAnswers;

            return mappedQuestion;
        }
    }
}
