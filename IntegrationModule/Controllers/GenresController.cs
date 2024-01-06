using IntegrationModule.Models;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly ProjectDBContext _context;

        public GenresController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET: Genres
        [HttpGet]
        public ActionResult<IEnumerable<GenreResponse>> GetGenreAll()
        {
            try
            {
                var genreResponses = _context.Genre.Select(dbGenre =>
                    new GenreResponse
                    {
                        Id = dbGenre.Id,
                        Name = dbGenre.Name,
                        Description = dbGenre.Description
                    });
                return Ok(genreResponses);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: Genres/Details/5
        [HttpGet("[action]/{id}")]
        public ActionResult<GenreResponse> GetGenreByID(int? id)
        {
            try
            {
                var dbGenre = _context.Genre.Find(id);
                if (dbGenre == null)
                {
                    return NotFound();
                }
                var genre = new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description
                };
                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<GenreResponse> Create([FromBody] GenreResponse genreResponse)
        {
            try
            {
                var dbGenre = new Genre
                {
                    Name = genreResponse.Name,
                    Description = genreResponse.Description,
                };
                _context.Genre.Add(dbGenre);
                _context.SaveChanges();
                var genre = new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description
                };
                return CreatedAtAction(nameof(GetGenreByID), new { id = genre.Id }, genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: Genres/Edit/5
        [HttpPut("[action]/{id}")]
        public ActionResult<GenreResponse> Update(int id, [FromBody] GenreResponse genreResponse)
        {
            try
            {
                var dbGenre = _context.Genre.Find(id);
                if (dbGenre == null)
                {
                    return NotFound();
                }
                dbGenre.Name = genreResponse.Name;
                dbGenre.Description = genreResponse.Description;
                _context.SaveChanges();
                var genre = new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description
                };
                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //DELETE Genres/Delete/5
        [HttpDelete("[action]/{id}")]
        public ActionResult<GenreResponse> Delete(int id)
        {
            try
            {
                var dbGenre = _context.Genre.Find(id);
                if (dbGenre == null)
                {
                    return NotFound();
                }

                var dbVideo = _context.Video.FirstOrDefault(v => v.GenreId == id);
                if (dbVideo != null)
                {
                    return BadRequest("Cannot delete genre because it is referenced by a movie.");
                }

                _context.Genre.Remove(dbGenre);
                _context.SaveChanges();
                var genre = new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name
                };
                return Ok(genre);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
