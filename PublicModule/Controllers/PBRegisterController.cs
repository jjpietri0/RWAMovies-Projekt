using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                        var responseString = await response.Content.ReadAsStringAsync();
                        var registeredUserResponse = JsonConvert.DeserializeObject<RegisteredUserResponse>(responseString);
                        var token = registeredUserResponse.Token;

                        TempData["Username"] = userRegisterReq.Username;
                        TempData["Token"] = token.ToString();

                        TempData["Success"] = "User is registered successfully!";
                        return RedirectToAction("Register", "PBRegister");
                    }
                    else
                    {
                        TempData["Error"] = "User failed to register!";
                        return RedirectToAction("Register", "PBRegister");
                        
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
                return RedirectToAction("Register", "PBRegister");
            }
        }

    }
}
