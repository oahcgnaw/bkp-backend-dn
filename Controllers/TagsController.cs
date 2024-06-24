using Microsoft.AspNetCore.Mvc;

namespace bkpDN.Controllers;

[Route("api/v1/tags")]
[ApiController]
public class TagsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagDto tagDto)
    {
        // Mocking the actual implementation
        var tag = new Tag { Id = 1, Kind = tagDto.Kind, Sign = tagDto.Sign, Name = tagDto.Name };
        return Ok(new { resource = tag });
    }
}
public class TagDto
{
    public string Kind { get; set; }
    public string Sign { get; set; }
    public string Name { get; set; }
}

public class Tag
{
    public int Id { get; set; }
    public string Kind { get; set; }
    public string Sign { get; set; }
    public string Name { get; set; }
}