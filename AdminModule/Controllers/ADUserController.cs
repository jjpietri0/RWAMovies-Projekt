using AdminModule.Dal;
using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class ADUserController : Controller
    {
        private readonly UserService _adUserService;

        public ADUserController(UserService userService)
        {
            _adUserService = userService;
        }

        public async Task<IActionResult> Index(string usernameFilter = null, string firstnameFilter = null, string lastnameFilter = null, string countryFilter = null)
        {
            try
            {
                var users = await _adUserService.GetAllUsersAsync(usernameFilter, firstnameFilter, lastnameFilter, countryFilter);
                return View(users);
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
        public async Task<IActionResult> Create(UserRegisterReq user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _adUserService.CreateUserAsync(user);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Success!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Create failed. Try again.";
                        return RedirectToAction("Create");
                    }
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while creating the user.";
                return RedirectToAction("Error", "Home");
            }
        }


        //details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _adUserService.GetUserByIdAsync(id);
                return View(user);
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
                var user = await _adUserService.GetUserByIdAsync(id);
                return View(user);
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserReq user)
        {
            try
            {
                await _adUserService.UpdateUserAsync(id, user);
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
                await _adUserService.DeleteUserAsync(id);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
