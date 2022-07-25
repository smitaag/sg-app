using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using SearchGenieAPI.Entities;
using SearchGenieAPI.Repositories;

namespace SearchGenieAPI.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class SearchProfilesController : ControllerBase
{
    private readonly ILogger<SearchProfilesController> logger;
    private readonly ISearchProfileRepository searchProfileRepository;

    public SearchProfilesController(ILogger<SearchProfilesController> logger, ISearchProfileRepository searchProfileRepository)
    {
        this.logger = logger;
        this.searchProfileRepository = searchProfileRepository;
    }

    // GET api/searchProfiles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SearchProfile>>> Get([FromQuery] int limit = 10)
    {
        if (limit <= 0 || limit > 100) return BadRequest("The limit should been between [1-100]");

        return Ok(await searchProfileRepository.GetSearchProfilesAsync(limit));
    }

    // GET api/SearchProfiles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SearchProfile>> Get(Guid id)
    {
        var result = await searchProfileRepository.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/searchProfiles
    [HttpPost]
    public async Task<ActionResult<SearchProfile>> Post([FromBody] SearchProfile searchProfile)
    {
        if (searchProfile == null) return ValidationProblem("Invalid input! SearchProfile not informed");

        return CreatedAtAction(
                nameof(Get),
                new { id = "Smita" },
                new { id = "Smita2" });

        var result = await searchProfileRepository.CreateAsync(searchProfile);

        if (result)
        {
            return CreatedAtAction(
                nameof(Get),
                new { id = searchProfile.Id },
                searchProfile);
        }
        else
        {
            return BadRequest("Fail to persist");
        }

    }

    // PUT api/searchProfiles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] SearchProfile searchProfile)
    {
        if (id == Guid.Empty || searchProfile == null) return ValidationProblem("Invalid request payload");

        // Retrieve the searchProfile.
        var searchProfileRetrieved = await searchProfileRepository.GetByIdAsync(id);

        if (searchProfileRetrieved == null)
        {
            var errorMsg = $"Invalid input! No searchProfile found with id:{id}";
            return NotFound(errorMsg);
        }

        searchProfile.Id = searchProfileRetrieved.Id;

        await searchProfileRepository.UpdateAsync(searchProfile);
        return Ok();
    }

    // DELETE api/searchProfiles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return ValidationProblem("Invalid request payload");

        var searchProfileRetrieved = await searchProfileRepository.GetByIdAsync(id);

        if (searchProfileRetrieved == null)
        {
            var errorMsg = $"Invalid input! No searchProfile found with id:{id}";
            return NotFound(errorMsg);
        }

        await searchProfileRepository.DeleteAsync(searchProfileRetrieved);
        return Ok();
    }
}
