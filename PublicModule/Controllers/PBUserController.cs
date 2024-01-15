using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;
using PublicModule.Dal;

namespace PublicModule.Controllers
{
    public class PBUserController : Controller
    {
        private readonly UserService _userService;

        public PBUserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(int id)
        {
            var user = await _userService.GetIdAsync(id);
            return Json(user);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordReq resetPasswordReq)
        {
            var response = await _userService.ResetPasswordAsync(resetPasswordReq);
            if (response.IsSuccessStatusCode)
            {
                return Ok("Success");
            }
            else
            {
                return RedirectToAction("ResetPassword", "PBUser");
            }
        }
        
    }
}
