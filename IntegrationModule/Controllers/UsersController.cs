using IntegrationModule.JWTServices;
using IntegrationModule.Models;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

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
                    Subject = "Confirm register",
                    Body = $"<h1>{userReq.FirstName} {userReq.LastName}</h1><p>Please confirm mail:{_baseURL}/validate-email.html?username={registeredUser.Username}&b64SecToken={registeredUser.SecurityToken} </p>"
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
                var target = _context.Users.FirstOrDefault(x =>
                    x.Username == request.Username && x.SecurityToken == request.B64Token);

                if (target == null)
                {
                    throw new InvalidOperationException("Authentication failed");
                }

                target.IsConfirmed = true;

                _context.Users.Update(target);
                _context.SaveChanges();

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
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
