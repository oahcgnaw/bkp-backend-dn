using System.Security.Claims;
using bkpDN.Data;
using bkpDN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bkpDN.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemsController: ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAccount(AccCreationDto accCreationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            DateTime happenedAtUtc = accCreationDto.Happened_at.UtcDateTime;

            var account = new Account()
            {
                User_id = int.Parse(userId),
                Amount = accCreationDto.Amount,
                Happened_at = happenedAtUtc,
                Kind = accCreationDto.Kind,
                Tag_id = accCreationDto.Tag_id
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var tag = await _context.Tags.FindAsync(accCreationDto.Tag_id);

            return Ok(new
            {
                resource = new AccountDto
                {
                    Id = account.Id,
                    User_id = account.User_id,
                    Amount = account.Amount,
                    Note = account.Note,
                    Tag_id = account.Tag_id,
                    Happened_at = account.Happened_at,
                    Created_at = account.Created_at,
                    Updated_at = account.Updated_at,
                    Kind = account.Kind,
                    Deleted_at = account.Deleted_at
                },
                tag
            });

        }
        
    }
}

