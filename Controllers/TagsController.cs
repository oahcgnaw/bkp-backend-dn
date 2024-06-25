using System.Security.Claims;
using bkpDN.Data;
using bkpDN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bkpDN.Controllers

{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TagsController> _logger;

        public TagsController(AppDbContext context, ILogger<TagsController> logger)
        {
            _context = context;
            _logger = logger;
        }
    
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTag([FromBody] TagDto tagDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var tag = new Tag() { Kind = tagDto.Kind, Sign = tagDto.Sign, Name = tagDto.Name, User_id = int.Parse(userId) };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(new { resource = tag });
        }
    }
}

public class TagDto
{
    public Kind Kind { get; set; }
    public string Sign { get; set; }
    public string Name { get; set; }
}