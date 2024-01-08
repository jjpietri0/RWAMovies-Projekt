using AdminModule.Dal;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class CountryAdminController : Controller
    {
        private readonly CountryService _countryService;

        public CountryAdminController(CountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var countries = await _countryService.GetAllCountriesAsync(page);
                return View(countries);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
