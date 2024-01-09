using AdminModule.Dal;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class ADCountryController : Controller
    {
        private readonly CountryService _countryService;

        public ADCountryController(CountryService countryService)
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
