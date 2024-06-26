using System.Security.Claims;
using bkpDN.Data;
using bkpDN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bkpDN.Controllers

{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TagsController(AppDbContext context)
        {
            _context = context;
        }
    
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTag([FromBody] TagCreationDto tagCreationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var tag = new Tag() { Kind = tagCreationDto.Kind, Sign = tagCreationDto.Sign, Name = tagCreationDto.Name, User_id = int.Parse(userId) };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(new { resource = new TagDto
                {
                    Id = tag.Id,
                    User_id = tag.User_id, 
                    Name = tag.Name,
                    Sign = tag.Sign,
                    Deleted_at = tag.Deleted_at,
                    Created_at = tag.Created_at,
                    Updated_at = tag.Updated_at,
                    Kind = tag.Kind
                }
            });
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTags([FromQuery] int page = 1, Kind kind = Kind.expenses)
        {
            var itemsPerPage = 10;
            var tags = await _context.Tags
                .Where(t => t.Kind == kind)
                .OrderBy(t => t.Created_at)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
            var tagsDto = tags.Select(tag => new TagDto
            {
                Id = tag.Id,
                User_id = tag.User_id, 
                Name = tag.Name,
                Sign = tag.Sign,
                Deleted_at = tag.Deleted_at,
                Created_at = tag.Created_at,
                Updated_at = tag.Updated_at,
                Kind = tag.Kind
            });
            var response = new
            {
                resources = tagsDto,
                pager = new
                {
                    page,
                    per_page = itemsPerPage,
                    total = _context.Tags.Count(t => t.Kind == kind)
                }
            };
            return Ok(response);
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            var tagDto = new TagDto
            {
                Id = tag.Id,
                User_id = tag.User_id, 
                Name = tag.Name,
                Sign = tag.Sign,
                Deleted_at = tag.Deleted_at,
                Created_at = tag.Created_at,
                Updated_at = tag.Updated_at,
                Kind = tag.Kind
            };
            return Ok(tagDto);
        }
        
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PatchTag([FromBody] TagCreationDto tagCreationDto, [FromRoute] int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            tag.Name = tagCreationDto.Name;
            tag.Sign = tagCreationDto.Sign;
            tag.Updated_at = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var tagDto = new TagDto
            {
                Id = tag.Id,
                User_id = tag.User_id, 
                Name = tag.Name,
                Sign = tag.Sign,
                Deleted_at = tag.Deleted_at,
                Created_at = tag.Created_at,
                Updated_at = tag.Updated_at,
                Kind = tag.Kind
            };
            return Ok(tagDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Tag deleted successfully."});
        }
    }
}