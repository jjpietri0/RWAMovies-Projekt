using IntegrationModule.REQModels;
using Microsoft.AspNetCore.Mvc;
using PublicModule.Dal;

namespace PublicModule.Controllers
{
    public class PBLoginController : Controller
    {
        private readonly LoginService _loginService;

        public PBLoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginReq userLoginReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _loginService.Login(userLoginReq);

                    if (response != null)
                    {
                        var jwtToken = await _loginService.GetTokensAsync(new JwtTokensReq
                        {
                            Password = userLoginReq.Password,
                            Username = userLoginReq.Username
                        });

                        if (jwtToken != null)
                        {

                            var script = $@"
                                <script>
                                    sessionStorage.setItem('AccessToken', '{jwtToken.AccessToken}');
                                    sessionStorage.setItem('Username', '{userLoginReq.Username}');
                                    sessionStorage.setItem('ResponseId', '{response.Id}');
                                    window.location.href = '/';
                                </script>";
                            ViewBag.ScriptToRun = script;
                            return View("AuthSuccess");
                        }
                        else
                        {
                            TempData["Error"] = "Login failed!";
                            return View(userLoginReq);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Login failed!";
                        return View(userLoginReq);
                    }
                }
                else
                {
                    return View(userLoginReq);
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Something went wrong! Login failed!";
                return RedirectToAction("Error");
            }
        }

    }
}
