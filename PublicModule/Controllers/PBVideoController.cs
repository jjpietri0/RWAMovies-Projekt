using Microsoft.AspNetCore.Mvc;
using PublicModule.Dal;

namespace PublicModule.Controllers
{
    public class PBVideoController : Controller
    {
        private readonly VideosService _videosService;

        public PBVideoController(VideosService videosService)
        {
            _videosService = videosService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public string GetToken()
        {
            string accessToken = Request.Headers["Authorization"].ToString().Split(" ")[1];
            return accessToken;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetVideos(string name, string genre, string orderBy, int page, int pageSize)
        {
            string accessToken = GetToken();
            var videos = await _videosService.GetVideosAsync(name, genre, orderBy, page, pageSize, accessToken);
            return Json(videos);
        }

        [HttpGet]
        public async Task<IActionResult> VideoInfo(int id, string accessToken)
        {
            var video = await _videosService.GetVideoIdAsync(id, accessToken);
            if (video != null)
            {
                return View(video);
            }else
            {
                return NotFound();
            }
        }
    }
}
