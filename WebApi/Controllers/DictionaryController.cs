using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
           _dictionaryService = dictionaryService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string key)
        {
            var result = await _dictionaryService.Search(key);

            return Ok(result);
        }
    }
}
