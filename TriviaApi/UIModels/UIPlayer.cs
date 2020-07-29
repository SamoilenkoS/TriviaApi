using System;

namespace TriviaApi.UIModels
{
    public class UIPlayer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime LastGameDate { get; set; }
    }
}
