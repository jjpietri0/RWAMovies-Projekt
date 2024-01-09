using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class ADGenreController : Controller
    {
        private readonly GenreService _genreService;

        public ADGenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: GenreAdmin 
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<JsonResult> GetAllGenres()
        {
            try
            {
                var genres = await _genreService.GetAllGenresAsync();
                return Json(genres);
            }
            catch (HttpRequestException)
            {
                return Json(new { error = "Error fetching genres" });

            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAjax(GenreReq genre)
        {
            try
            {
                await _genreService.CreateGenreAsync(genre);
                return Json(new { success = true });
            }
            catch (HttpRequestException)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditAjax(int id, GenreReq genre)
        {
            try
            {
                await _genreService.UpdateGenreAsync(id, genre);
                return Json(new { success = true });
            }
            catch (HttpRequestException)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAjax(int id)
        {
            try
            {
                await _genreService.DeleteGenreAsync(id);
                return Json(new { success = true });
            }
            catch (HttpRequestException)
            {
                return Json(new { success = false });
            }
        }
    }
}
