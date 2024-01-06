using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class VideosController : Controller
    {
        private readonly VideoService videoService;

        public VideosController(VideoService videoService)
        {
            this.videoService = videoService;
        }

        // GET: Videos
        public async Task<IActionResult> Index(string filter, int page = 1)
        {
            try
            {

                var videos = await videoService.GetAllVideoAsync(page, filter);
                ViewData["filter"] = filter;
                return View(videos);
            }
            catch (HttpRequestException ex)
            {
                ex.Message.ToString();
                return RedirectToAction("Error", "Home");
            }
        }


        // GET: Videos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        [HttpPost]
        public async Task<IActionResult> Create(VideoRequest videoRequest)
        {
            try
            {

                await videoService.CreateVideoAsync(videoRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var video = await videoService.GetVideoAsync(id);
                return View(video);

            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        // POST: Videos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoRequest video)
        {
            try
            {
                await videoService.UpdateVideoAsync(id, video);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await videoService.DeleteVideoAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Shared");
            }
        }
    }
}
