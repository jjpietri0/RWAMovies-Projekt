using IntegrationModule.Models;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersListController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public UsersListController(ProjectDBContext dbContext)
        {
            _context = dbContext;
        }

        //GET(all)
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<UserResponse>> GetAllUsers(string? usernameFilter = null, string? firstnameFilter = null, string? lastnameFilter = null, string? countryFilter = null)
        {
            try
            {
                var users = _context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(firstnameFilter))
                {
                    users = users.Where(user => user.FirstName.Contains(firstnameFilter));
                }

                if (!string.IsNullOrEmpty(lastnameFilter))
                {
                    users = users.Where(user => user.LastName.Contains(lastnameFilter));
                }

                if (!string.IsNullOrEmpty(usernameFilter))
                {
                    users = users.Where(user => user.Username.Contains(usernameFilter));
                }

                if (!string.IsNullOrEmpty(countryFilter))
                {
                    users = users.Where(user => user.CountryOfResidence.Name.Contains(countryFilter));
                }

                var usersFinal = users.Select(v => new UserResponse
                {
                    Id = v.Id,
                    Username = v.Username,
                    FirstName = v.FirstName,
                    LastName = v.LastName,
                    Email = v.Email,
                    Phone = v.Phone,
                    CountryOfResidenceId = v.CountryOfResidenceId,
                    IsConfirmed = v.IsConfirmed,
                    DeletedAt = v.DeletedAt,
                    CreatedAt = v.CreatedAt
                }).ToList();
                return Ok(usersFinal);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //GET(id)
        [HttpGet("[action]/{id}")]
        public ActionResult<UserResponse> GetId(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }
                var userFinal = new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CountryOfResidenceId = user.CountryOfResidenceId,
                    Phone = user.Phone,
                    IsConfirmed = user.IsConfirmed,
                    DeletedAt = user.DeletedAt,
                    CreatedAt = user.CreatedAt
                };
                return Ok(userFinal);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //PUT
        [HttpPut("[action]/{id}")]
        public ActionResult<UserResponse> Update(int id, [FromBody] UserReq userReq)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.Username = userReq.Username;
                user.FirstName = userReq.FirstName;
                user.LastName = userReq.LastName;
                user.Email = userReq.Email;
                user.CountryOfResidenceId = userReq.CountryOfResidenceId;
                user.Phone = userReq.Phone;
                user.IsConfirmed = userReq.IsConfirmed;

                _context.SaveChanges();


                var userFinal = new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CountryOfResidenceId = user.CountryOfResidenceId,
                    Phone = user.Phone,
                    IsConfirmed = user.IsConfirmed,
                    DeletedAt = user.DeletedAt,
                    CreatedAt = user.CreatedAt
                };

                return Ok(userFinal);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //DELETE
        [HttpDelete("[action]/{id}")]
        public ActionResult<UserResponse> Delete(int id)
        {
            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.DeletedAt = DateTime.UtcNow;
                user.IsConfirmed = false;
                _context.SaveChanges();

                var userFinal = new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    IsConfirmed = user.IsConfirmed,
                    CountryOfResidenceId = user.CountryOfResidenceId,
                    DeletedAt = user.DeletedAt,
                    CreatedAt = user.CreatedAt
                };
                return Ok(userFinal);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
