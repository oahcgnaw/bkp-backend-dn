using System.Security.Claims;
using bkpDN.Data;
using bkpDN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAccounts([FromQuery] int page, DateTimeOffset happened_after,
            DateTimeOffset happened_before)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            DateTime happenedAfterUtc = happened_after.UtcDateTime;
            DateTime happenedBeforeUtc = happened_before.UtcDateTime;
            var itemsPerPage = 10;
            var accounts = await _context.Accounts
                .Where(a => a.User_id == int.Parse(userId) && a.Happened_at >= happenedAfterUtc && a.Happened_at <= happenedBeforeUtc)
                .OrderBy(a => a.Happened_at)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
            var accountsDto = accounts.Select(account => new AccountDto
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
            });
            var response = new
            {
                resources = accountsDto,
                pager = new
                {
                    page,
                    per_page = itemsPerPage,
                    count = _context.Accounts.Count(a => a.Happened_at >= happenedAfterUtc && a.Happened_at <= happenedBeforeUtc)
                }
            };
            return Ok(response);
        }

        [HttpGet("balance")]
        [Authorize]
        public async Task<IActionResult> GetBalance([FromQuery] DateTimeOffset happened_after,
            DateTimeOffset happened_before)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var happenedAfterUtc = happened_after.UtcDateTime;
            var happenedBeforeUtc = happened_before.UtcDateTime;

            var items = await _context.Accounts
                .Where(a => a.User_id == int.Parse(userId) && a.Happened_at >= happenedAfterUtc && a.Happened_at <= happenedBeforeUtc)
                .ToListAsync();
            
            var expenseSum = items.Where(a => a.Kind == Kind.expenses).Sum(a => a.Amount);
            var incomeSum = items.Where(a => a.Kind == Kind.income).Sum(a => a.Amount);

            return Ok(new
            {
                income = incomeSum,
                expenses = expenseSum,
                balance = incomeSum - expenseSum
            });

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            if (account.User_id != int.Parse(userId))
            {
                return Unauthorized();
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Account deleted successfully." });
        }
        
        [HttpGet("summary")]
        [Authorize]
        public async Task<IActionResult> GetSummary([FromQuery] DateTimeOffset happened_after, DateTimeOffset happened_before, Kind kind, GroupByOption group_by)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var happenedAfterUtc = happened_after.UtcDateTime;
            var happenedBeforeUtc = happened_before.UtcDateTime;
            
            var items = await _context.Accounts
                .Where(a => a.User_id == int.Parse(userId) && a.Happened_at >= happenedAfterUtc && a.Happened_at <= happenedBeforeUtc)
                .ToListAsync();
            
            if (group_by == GroupByOption.happened_at)
            {
                // group result by date
                var groupedItems = items.GroupBy(a => a.Happened_at.Date).Select(g => new
                {
                    happened_at = g.Key.ToString("yyyy-MM-dd"),
                    amount = g.Sum(a => a.Amount)
                }).ToList();
                return Ok(new
                {
                    groups = groupedItems,
                    total = groupedItems.Sum(g => g.amount)
                });
            }
            
            if (group_by == GroupByOption.tag_id)
            {
                // group by tag_id
                var groupedItems = items.GroupBy(a => a.Tag_id).Select(g => new
                {
                    tag_id = g.Key,
                    amount = g.Sum(a => a.Amount),
                }).ToList();

                // Fetch tags asynchronously for each group
                var groupsWithTags = new List<object>();
                foreach (var group in groupedItems)
                {
                    var tag = await _context.Tags.FindAsync(group.tag_id);
                    groupsWithTags.Add(new
                    {
                        tag_id = group.tag_id,
                        amount = group.amount,
                        tag
                    });
                }
                
                return Ok(new
                {
                    groups = groupsWithTags,
                    total = groupedItems.Sum(g => g.amount)
                });

            }

            return BadRequest();
        }
        
    }
}

