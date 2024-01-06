using IntegrationModule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public TagsController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Tag>> GetAllTags()
        {
            return _context.Tag.ToList();
        }

        // GET:
        [HttpGet("[action]/{id}")]
        public ActionResult<Tag> GetTagById(int id)
        {
            var tag = _context.Tag.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // POST
        [HttpPost("[action]")]
        public ActionResult<Tag> Create(Tag tag)
        {
            _context.Tag.Add(tag);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTagById), new { id = tag.Id }, tag);
        }

        // PUT:
        [HttpPut("[action]/{id}")]
        public IActionResult Update(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE:
        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteTag(int id)
        {
            var tag = _context.Tag.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tag.Remove(tag);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
