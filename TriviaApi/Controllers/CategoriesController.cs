using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TriviaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController()
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await DbConnection.GetAllAsync<Category>(new MongoDB.Bson.BsonDocument());
        }
    }
}
