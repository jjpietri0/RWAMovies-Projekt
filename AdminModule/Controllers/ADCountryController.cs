using AdminModule.Dal;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class ADCountryController : Controller
    {
        private readonly CountryService _adCountryService;

        public ADCountryController(CountryService countryService)
        {
            _adCountryService = countryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var countries = await _adCountryService.GetAllCountriesAsync(page);
                return View(countries);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
