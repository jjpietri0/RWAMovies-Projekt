using IntegrationModule.Models;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public VideosController(ProjectDBContext dbContext)
        {
            _context = dbContext;
        }

        [Authorize]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<VideoResponse>> GetAll([FromQuery] string? name, [FromQuery] string? genre, [FromQuery] string? orderBy, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            try
            {
                var videos = _context.Video.AsQueryable();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    videos = videos.Where(v => v.Name.ToLower().Contains(name.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(genre))
                {
                    videos = videos.Where(v => v.Genre.Name.ToLower().Contains(genre.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    videos = orderBy.ToLower() switch
                    {
                        "id" => videos.OrderBy(v => v.Id),
                        "name" => videos.OrderBy(v => v.Name),
                        "totaltime" => videos.OrderBy(v => v.TotalSeconds),
                        _ => videos.OrderBy(v => v.Id),
                    };
                }
                else
                {
                    videos = videos.OrderBy(v => v.Id);
                }
                if (page.HasValue && pageSize.HasValue)
                {
                    videos = videos.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var videoResponses = videos.Select(v => new VideoResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    Description = v.Description,
                    GenreId = v.GenreId,
                    TotalSeconds = v.TotalSeconds,
                    StreamingUrl = v.StreamingUrl,
                    ImageId = v.ImageId,
                    Tags = v.VideoTags.Select(vt => new TagResponse
                    {
                        Id = vt.Tag.Id,
                        Name = vt.Tag.Name
                    }).ToList()
                });
                return Ok(videoResponses);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        public ActionResult<VideoResponse> GetId(int id)
        {
            try
            {
                var video = _context.Video.Find(id);
                if (video == null)
                {
                    return NotFound();
                }
                var videoResponse = new VideoResponse
                {
                    Id = video.Id,
                    Name = video.Name,
                    Description = video.Description,
                    GenreId = video.GenreId,
                    TotalSeconds = video.TotalSeconds,
                    StreamingUrl = video.StreamingUrl,
                    ImageId = video.ImageId,
                    Tags = video.VideoTags.Select(vt => new TagResponse
                    {
                        Id = vt.Tag.Id,
                        Name = vt.Tag.Name
                    }).ToList()
                };
                return Ok(videoResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //POST
        [Authorize]
        [HttpPost("[action]")]
        public ActionResult<VideoResponse> Create([FromBody] VideoCreate videoCreate)
        {
            try
            {
                var genre = _context.Genre.Find(videoCreate.GenreId);
                if (genre == null)
                {
                    return NotFound();
                }

                Image image;
                if (!string.IsNullOrEmpty(videoCreate.ImageUrl))
                {
                    image = _context.Image.FirstOrDefault(i => i.Content == videoCreate.ImageUrl);
                    if (image == null)
                    {
                        image = new Image { Content = videoCreate.ImageUrl };
                        _context.Image.Add(image);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    image = _context.Image.Find(videoCreate.ImageId);
                    if (image == null)
                    {
                        return NotFound();
                    }
                }

                var video = new Video
                {
                    Name = videoCreate.Name,
                    Description = videoCreate.Description,
                    StreamingUrl = videoCreate.StreamingUrl,
                    TotalSeconds = videoCreate.TotalSeconds,
                    Image = image,
                    Genre = genre
                };
                _context.Video.Add(video);
                _context.SaveChanges();

                var videoResponse = new VideoResponse
                {
                    Id = video.Id,
                    Name = video.Name,
                    GenreId = video.GenreId,
                    Description = video.Description,
                    TotalSeconds = video.TotalSeconds,
                    ImageId = video.ImageId,
                    StreamingUrl = video.StreamingUrl
                };
                return Ok(videoResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //PUT
        [Authorize]
        [HttpPut("[action]/{id}")]
        public ActionResult<VideoResponse> Update(int id, [FromBody] VideoCreate videoRequest)
        {
            try
            {
                var video = _context.Video.Find(id);
                if (video == null)
                {
                    return NotFound();
                }
                video.Name = videoRequest.Name;
                video.Description = videoRequest.Description;
                video.TotalSeconds = videoRequest.TotalSeconds;
                video.StreamingUrl = videoRequest.StreamingUrl;
                video.ImageId = videoRequest.ImageId;
                video.GenreId = videoRequest.GenreId;
                _context.SaveChanges();

                var videoResponse = new VideoResponse
                {
                    Id = video.Id,
                    Name = video.Name,
                    Description = video.Description,
                    GenreId = video.GenreId,
                    TotalSeconds = video.TotalSeconds,
                    StreamingUrl = video.StreamingUrl,
                    ImageId = video.ImageId
                };
                return Ok(videoResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //DELETE
        [Authorize]
        [HttpDelete("[action]/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var video = _context.Video.Find(id);
                if (video == null)
                {
                    return NotFound();
                }

                var image = _context.Image.Find(video.ImageId);
                if (image != null && _context.Video.Count(v => v.ImageId == image.Id) == 1)
                {
                    _context.Image.Remove(image);
                }

                var videoTags = _context.VideoTag.Where(vt => vt.VideoId == video.Id);
                if (videoTags.Any())
                {
                    _context.VideoTag.RemoveRange(videoTags);
                }

                _context.Video.Remove(video);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
