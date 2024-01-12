using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;
using PublicModule.Dal;

namespace PublicModule.Controllers
{
    public class PBRegisterController : Controller
    {
        private readonly RegisterService _registerService;

        public PBRegisterController(RegisterService registerService)
        {
            _registerService = registerService;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterReq userRegisterReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _registerService.Register(userRegisterReq);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "User is registered successfully!";
                        return RedirectToAction("Login", "PBLogin");
                    }
                    else
                    {
                        TempData["Error"] = "User failed to register!";
                        return View(userRegisterReq);
                    }
                }
                else
                {
                    return View(userRegisterReq);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong! Registration failed!";
                return RedirectToAction("Error");
            }
        }

    }
}
