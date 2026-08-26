using Microsoft.AspNetCore.Mvc;
using Public.Analysis.Data;

namespace Public.Analysis.Console.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { Message = "Hello from API" });

        // Example endpoint to access dataset (optional)
        [HttpGet("datasets/{name}")]
        public IActionResult GetDatasetInfo(string name, [FromServices] Dictionary<string, IDataSet> dataSets)
        {
            if (dataSets.TryGetValue(name, out var ds))
                return Ok(ds.MetaData);
            return NotFound();
        }
    }
}
