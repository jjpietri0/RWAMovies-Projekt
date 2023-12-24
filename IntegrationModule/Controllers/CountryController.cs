using IntegrationModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public CountryController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET: api/Country
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var countries = _context.Country
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                var response = countries.Select(c => new Country
                {
                    Code = c.Code,
                    Name = c.Name,
                });
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Country>> GetCountryById(int id)
        {
            try
            {
                var country = _context.Country.Find(id);
                if (country == null)
                {
                    return NotFound();
                }
                var response = new Country
                {
                    Code = country.Code,
                    Name = country.Name,
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
