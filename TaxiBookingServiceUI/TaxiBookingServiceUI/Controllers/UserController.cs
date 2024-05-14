namespace TaxiBookingServiceUI.Controllers
{
    public class UserController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public const string SessionUserToken = "_usertoken";

        public const string SessionUserEmail = "_userEmail";

        public readonly IHttpContextAccessor _httpContext;

        public UserController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Profile()
        {
            UserViewModel user = new UserViewModel();
            string _email = HttpContext.Session.GetString("_userEmail");

            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.UserSearch, _email, null);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserViewModel>(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(user);

        }

        public async Task<ActionResult> Edit()
        {
            UserEditModel user = new UserEditModel();
            string email = HttpContext.Session.GetString("_userEmail");

            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.UserSearch, email, null);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<UserEditModel>(result);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }


            return View(user);

        }

        [HttpPost]
        public ActionResult Edit(string email, UserEditModel userEdit)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString("_usertoken");

                var result = _api.PutByStringAsync(ApiUrls.UserUpdate, userEdit, email, token);

                if (result.IsSuccessStatusCode)
                {
                    TempData["msg"] = "User Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errmsg"] = "You are currently unauthorized";
                    return RedirectToAction("Index");
                }
            }
            return View();

        }

        public async Task<ActionResult> Display()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            HttpResponseMessage res = await _api.GetAsync(ApiUrls.UserDisplay, 0);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserViewModel>>(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(users);
        }

        public ActionResult Add()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Add(UserModel userModel)
        {
            using (HttpClient client = _api.Initial())
            {
                userModel.RoleName = Roles.User.ToString();
                if (ModelState.IsValid)
                {
                   var result = _api.PostAsync(userModel, ApiUrls.UserAdd, null);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Please enter your email and password to login";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "User could not be added";
                return View();
            }
        }

        public IActionResult Login()
        {
            HttpContext.Session.Remove(SessionUserToken);
            HttpContext.Session.Remove(SessionUserEmail);
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            var result = _api.Login(login, ApiUrls.Login);

            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString(SessionUserToken, res);
                HttpContext.Session.SetString(SessionUserEmail, login.Email);
                return RedirectToAction("Index", "Booking");
            }
            else
            {
                TempData["errmsg"] = "Wrong Email or Password entered";
                return View();
            }

        }
    }
}
