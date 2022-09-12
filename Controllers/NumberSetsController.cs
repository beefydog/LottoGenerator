using LottoGeneratorService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LottoGeneratorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class NumberSetsController : ControllerBase
    {
        private readonly ILogger<NumberSetsController> _logger;

        public NumberSetsController(ILogger<NumberSetsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetAsync(Models.SetsRequest setsRequest)
        {
            //example data for reference  
            //int[] Min = { 1, 1, 1 };
            //int[] Max = { 70, 25, 5 }; //first group has number range of 1-70, second group 1-25, third group 1-5
            //int[] NumbersPerGroup = { 5, 2, 1 }; // get 5 numbers for first group, 2 for second, and just 1 for third
            //decimal[] Divergence = { 10, 25, 50 }; // 10% divergence for group 1, 25% for group 2, and 50  for group 3 
            //bool[] SumCheck = { true, true, false }; // divergence included for groups 1 & 2, but group 3 is set to false (ignoring divergence number above)
            //bool[] OECheck = { true, false, false }; // odd/even ratio check is enabled for group 1, disabled for groups 2&3

            int[] Min = new int[0];
            int[] Max = new int[0];
            int[] NumbersPerGroup = new int[0];
            decimal[] Divergence = new decimal[0];
            bool[] SumCheck = new bool[0];
            bool[] OECheck = new bool[0];

            //populate the arrays with the request values
            foreach (var r in setsRequest.numberSet)
            {
                Min = Min.Append(r.min).ToArray();
                Max = Max.Append(r.max).ToArray();
                NumbersPerGroup = NumbersPerGroup.Append(r.numbersPerGroup).ToArray();
                Divergence = Divergence.Append(r.divergence).ToArray();
                SumCheck= SumCheck.Append(r.sumCheck).ToArray();
                OECheck = OECheck.Append(r.oeCheck).ToArray();
            }
            // call the procedure, get List<int[]>  a.k.a a list of integer arrays
            
            var nsets = await LottoGeneratorService.NumbersSetGenerator.GenerateSetsAsListOfIntArray(Min,Max,NumbersPerGroup,Divergence,setsRequest.sets,SumCheck,OECheck,true);

            if (nsets.Count == 0) return NotFound();
            return Ok(nsets); //should return a json object (an array of integer arrays)
        }

        /*
         * 
         *************** Test JSON input ************
        **** the following is for the MEGA-MILLIONs lottery ticket
        {
          "numberSet": [
            {
              "min": 1,
              "max": 70,
              "numbersPerGroup": 5,
              "divergence": 10,
              "sumCheck": true,
              "oeCheck": true
            },
	        {
              "min": 1,
              "max": 25,
              "numbersPerGroup": 1,
              "divergence": 10,
              "sumCheck": false,
              "oeCheck": false
            }
          ],
          "sets": 10
        }
         
         *************** Test Results ***************
         [[1,32,33,34,62,12],[3,7,36,52,60,18],[9,12,41,64,66,13],[7,17,24,54,68,14],[6,28,38,43,57,11],[6,13,22,55,64,25],[6,7,32,54,65,5],[11,22,27,60,62,8],[16,30,33,36,59,14],[2,28,29,52,59,24]]

         */

    }
}
