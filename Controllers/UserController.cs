using bkpDN.Data;
using bkpDN.Services;
using bkpDN.Models;
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

            var validationCode = await _context.ValidationCodes
                .Where(vc => vc.User_email == body["email"] && vc.Validation_code == body["code"])
                .FirstOrDefaultAsync();
            if (validationCode == null)
            {
                return Unauthorized("Invalid or expired validation code.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == body["email"]);
            // Check if user already registered
            if (user == null)
            {
                // Add new user
                user = new User { Email = body["email"] };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
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
    }

}



