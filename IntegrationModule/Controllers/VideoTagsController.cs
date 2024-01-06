using IntegrationModule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoTagsController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public VideoTagsController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<VideoTag>> GetAllVideoTags()
        {
            return _context.VideoTag.ToList();
        }

        // GET:
        [HttpGet("[action]/{id}")]
        public ActionResult<VideoTag> GetVideoTagById(int id)
        {
            var tag = _context.VideoTag.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // POST
        [HttpPost("[action]")]
        public ActionResult<VideoTag> Create(VideoTag videoTag)
        {
            _context.VideoTag.Add(videoTag);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetVideoTagById), new { id = videoTag.Id }, videoTag);
        }

        // PUT:
        [HttpPut("[action]/{id}")]
        public IActionResult PutVideoTag(int id, VideoTag tag)
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
            var tag = _context.VideoTag.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.VideoTag.Remove(tag);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
