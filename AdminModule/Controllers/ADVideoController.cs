using AdminModule.Dal;
using AdminModule.Properties;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminModule.Controllers
{
    public class ADVideoController : Controller
    {
        private readonly VideoService _videoService;
        private readonly string _username;
        private readonly string _password;

        public ADVideoController(VideoService videoService, IOptions<Admin> adminConfig)
        {
            _videoService = videoService;
            _username = adminConfig.Value.Username;
            _password = adminConfig.Value.Password;
        }

        public async Task<IActionResult> Index(string filter, int page = 1)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                var videos = await _videoService.GetAllVideosAsync(page, filter);
                ViewData["CurrentFilter"] = filter;

                return View(videos);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoReq video)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                await _videoService.CreateVideoAsync(video);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                var video = await _videoService.GetVideoByIdAsync(id);
                return View(video);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VideoReq video)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                await _videoService.UpdateVideoAsync(id, video);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                await _videoService.DeleteVideoAsync(id);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadVideos(int page, int pageSize, string nameFilter, string genreFilter)
        {
            try
            {
                await _videoService.GetJwtTokenForAdmin(_username, _password);
                var videos = await _videoService.GetAllVideosWithFilterAsync(page,pageSize,nameFilter,genreFilter);
                return Ok(videos);
            }
            catch (HttpRequestException)
            {
                return PartialView("Partials/_Error");
            }
        }

    }
}
