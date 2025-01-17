using System.Security.Claims;
using bkpDN.Data;
using bkpDN.Services;
using bkpDN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bkpDN.Controllers
{
    [Route("api/v1/")]
    public class UserController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly ILogger<UserController> _logger;

        public UserController(AppDbContext context, JwtService jwtService, EmailService emailService, ILogger<UserController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("session")]
        public async Task<IActionResult> CreateUser([FromBody] Dictionary<string, string> body)
        {
            // validate the email and code from the request body to the ones in the validationCodes table, then check if user is already in the users table, if not insert user into the users table, and return a jwt token finally
            
            // check and validate the latest validation code 
            var validationCode = await _context.ValidationCodes
                .Where(v => v.User_email == body["email"])
                .OrderByDescending(v => v.Timestamp)
                .FirstOrDefaultAsync();
            // OrderByDescending and FirstOrDefaultAsync are used since LastOrDefaultAsync() fetches all records and then finds the last one in memory, which is less efficient.
            if (validationCode == null || validationCode.Validation_code != body["code"])
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == body["email"]);
            // Check if user already registered
            if (user == null)
            {
                // Add new user
                user = new User { Email = body["email"] };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                // Refresh the user object
                await _context.Entry(user).ReloadAsync();
            }
            
            // Generate jwt token
            var token = _jwtService.GenerateSecurityToken(user);
            return Ok(new { jwt = token });
        }

        [HttpPost("validation_codes")]
        public async Task<IActionResult> SendCode([FromBody] Dictionary<string, string> body)
        {
            _logger.LogInformation("SendCode method called.");
            var validationCode = new Random().Next(100000, 999999).ToString();
            
            var subject = "The code is valid for 5 minutes";
            var message = $"Your validation code is {validationCode}";
            
            await _emailService.SendEmailAsync(body["email"], subject, message);
            
            _context.ValidationCodes.Add(new ValidationCode
            {
                Validation_code = validationCode,
                User_email = body["email"]
            });
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
        
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return Ok(new {resource = user});
        }

    }

}



