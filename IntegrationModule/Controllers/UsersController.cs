using IntegrationModule.JWTServices;
using IntegrationModule.Models;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserGenRepository _userGenRepository;
        private readonly ProjectDBContext _context;
        private readonly string _baseURL;

        public UsersController(ProjectDBContext context, IUserGenRepository userGenRepository, IConfiguration configuration)
        {
            _userGenRepository = userGenRepository;
            _context = context;
            _baseURL = configuration["BaseApiUrl"];
        }

        [HttpPost("[action]")]
        public ActionResult<UserSignInResponse> Login([FromBody] UserLoginReq loginReq)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var user = _context.Users.FirstOrDefault(x => x.Username == loginReq.Username);

                if (user == null)
                {
                    return Unauthorized("Invalid username.");
                }

                bool isAuthenticated = Authenticate(loginReq.Username, loginReq.Password);

                if (!isAuthenticated)
                    return Unauthorized("Wrong password.");

                var tokens = _userGenRepository.GenerateJwtTokens(new JwtTokensReq
                {
                    Username = loginReq.Username,
                    Password = loginReq.Password
                });

                return Ok(new UserSignInResponse
                {
                    Id = user.Id, 
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = tokens.AccessToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<User> Register([FromBody] UserRegisterReq userReq)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                if (_context.Users.Any(x => x.Username == userReq.Username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                var registeredUser = _userGenRepository.Add(userReq);
                _context.Users.Add(registeredUser);
                _context.SaveChanges();

                var notification = new Notification
                {
                    CreatedAt = DateTime.UtcNow,
                    ReceiverEmail = registeredUser.Email,
                    Subject = "Registration confirmation!",
                    Body = $"<h1>{userReq.FirstName} {userReq.LastName}</h1><p>Please confirm mail:{_baseURL}html/email-validation.html?username={registeredUser.Username}&b64SecToken={registeredUser.SecurityToken} </p>"
                };

                _context.Notification.Add(notification);
                _context.SaveChanges();

                return Ok(new RegisteredUserResponse
                {
                    Id = registeredUser.Id,
                    Token = registeredUser.SecurityToken,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("[action]")]
        public ActionResult<Tokens> GetJwtTokens([FromBody] JwtTokensReq request)
        {
            try
            {
                var authenticated = Authenticate(request.Username, request.Password);

                if (!authenticated)
                    throw new InvalidOperationException("Wrong authentication");

                return Ok(_userGenRepository.GenerateJwtTokens(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[action]")]
        public ActionResult ValidateEmail([FromBody] EmailValidReq request)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Username == request.Username && x.SecurityToken == request.B64Token) ?? throw new InvalidOperationException("Authentication failed");
                user.IsConfirmed = true;

                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ResetPassword([FromBody] ResetPasswordReq resetPasswordReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _context.Users.Find(resetPasswordReq.Id);
            if (user == null)
            {
                return NotFound("User not found!");
            }
            if (!Authenticate(user.Username.ToLower(), resetPasswordReq.CurrentPassword))
            {
                return BadRequest("Current password is not correct");
            }
            if(Authenticate(user.Username.ToLower(), resetPasswordReq.NewPassword))
            {
                return BadRequest("New password cannot be the same as the old one");
            }
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] hash = KeyDerivation.Pbkdf2(
                               password: resetPasswordReq.NewPassword,
                                              salt: salt,
                                                             prf: KeyDerivationPrf.HMACSHA256,
                                                                            iterationCount: 100000,
                                                                                           numBytesRequested: 256 / 8);
            user.PwdSalt = Convert.ToBase64String(salt);
            user.PwdHash = Convert.ToBase64String(hash);
            _context.SaveChanges();
            return Ok();
        }
        private bool Authenticate(string username, string password)
        {
            var target = _context.Users.FirstOrDefault(x => x.Username == username);

            if (!target.IsConfirmed)
                return false;
            byte[] salt = Convert.FromBase64String(target.PwdSalt);
            byte[] hash = Convert.FromBase64String(target.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
        }
    }
}
