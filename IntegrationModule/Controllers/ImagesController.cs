using IntegrationModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly ProjectDBContext _context;

        public ImagesController(ProjectDBContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Image>> GetAll()
        {
            try
            {
                var images = _context.Image.ToList();
                return Ok(images);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("[action]/{id}")]
        public ActionResult<string> GetImageUrl(int id)
        {
            var image = _context.Image.Find(id);

            if (image == null)
            {
                return NotFound();
            }
            return Ok(image.Content);
        }
    }
}
