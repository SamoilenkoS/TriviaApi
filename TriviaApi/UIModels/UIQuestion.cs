using System.Collections.Generic;

namespace TriviaApi.UIModels
{
    public class UIQuestion
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public IEnumerable<UIAnswer> Answers { get; set; }
    }
}
