using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LottoGeneratorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberSetsController : ControllerBase
    {
        private readonly ILogger<NumberSetsController> _logger;

        public NumberSetsController(ILogger<NumberSetsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetAsync(Models.SetsRequest setsRequest)
        {
            var numberOfSets = setsRequest.sets;
            StringBuilder sb = new();
            foreach (var param in setsRequest.numberParams)
            {
                var strNumberSets = await LottoGeneratorService.NumbersSetGenerator.GenerateSets(param.min, param.max, param.numbers, numberOfSets, param.spreadpercent);
                sb.AppendLine(strNumberSets);
            }
            string result = sb.ToString();
            if (result.Length == 0) return NotFound();
            return Ok(result);
        }



    }
}
