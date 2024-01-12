using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class ADGenreController : Controller
    {
        private readonly GenreService _adGenreService;

        public ADGenreController(GenreService genreService)
        {
            _adGenreService = genreService;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _adGenreService.GetAllGenresAsync());
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> Create(GenreReq genre)
        {
            try
            {
                await _adGenreService.CreateGenreAsync(genre);
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
                var tag = await _adGenreService.GetGenreByIdAsync(id);
                return View(tag);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GenreReq genre)
        {
            try
            {
                await _adGenreService.UpdateGenreAsync(id, genre);
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
                await _adGenreService.DeleteGenreAsync(id);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
