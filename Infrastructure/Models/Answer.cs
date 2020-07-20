using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace Infrastructure.Models
{
    public class Answer
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public  bool IsCorrect { get; set; }
    }
}
