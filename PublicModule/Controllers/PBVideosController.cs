using Microsoft.AspNetCore.Mvc;
using PublicModule.Dal;

namespace PublicModule.Controllers
{
    public class PBVideosController : Controller
    {
        private readonly VideosService _videosService;

        public PBVideosController(VideosService videosService)
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
            accessToken = $"Bearer {accessToken}";
            return accessToken;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideosAsync(string name, string genre, string orderBy, int page, int pageSize)
        {
            var videos = await _videosService.GetVideosAsync(name, genre, orderBy, page, pageSize, GetToken());
            return Json(videos);
        }

        [HttpGet]
        public async Task<IActionResult> GetVideoByIdAsync(int id)
        {
            var video = await _videosService.GetVideoByIdAsync(id, GetToken());
            return View(video);
        }
        
    }
}
