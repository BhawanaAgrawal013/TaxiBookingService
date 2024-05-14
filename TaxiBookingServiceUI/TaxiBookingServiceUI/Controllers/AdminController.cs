namespace TaxiBookingServiceUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public const string SessionToken = "_admintoken";

        public const string SessionEmail = "_adminEmail";

        public readonly IHttpContextAccessor _httpContext;

        public AdminController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.Remove(SessionEmail);
            HttpContext.Session.Remove(SessionToken);
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            var result = _api.Login(login, ApiUrls.Login);

            if (result == null)
            {
                return View();
            }
            else
            {
                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    HttpContext.Session.SetString(SessionToken, res);
                    HttpContext.Session.SetString(SessionEmail, login.Email);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["errmsg"] = "Wrong Email or Password entered";
                    return View();
                }
            }
        }

        public async Task<ActionResult> Display()
        {
            List<DriverViewModel> drivers = new List<DriverViewModel>();

            HttpResponseMessage res = await _api.GetAsync(ApiUrls.DriverDisplay, 0);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                drivers = JsonConvert.DeserializeObject<List<DriverViewModel>>(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(drivers);

        }

        public async Task<ActionResult> DriverVehicle(string email)
        {
            VehicleDetailViewModel vehicle = new VehicleDetailViewModel();

            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.VehicleDetailSearch, email, null);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                vehicle = JsonConvert.DeserializeObject<VehicleDetailViewModel>(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(vehicle);
        }

        public async Task<ActionResult> ApproveDrivers(string email)
        {
            string token = HttpContext.Session.GetString("_admintoken");

            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.ApproveDriver, email, token);

            if (res.IsSuccessStatusCode)
            {
                TempData["msg"] = "Driver has been approved";
                return RedirectToAction("Display");
            }
            else
            {
                TempData["errmsg"] = "Driver could not be approved";
                return View();
            }
        }

        public ActionResult SearchBooking()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BookingStatus(int id)
        {
            try
            {
                BookingViewModel bookingViewModel = new BookingViewModel();
                HttpResponseMessage res = await _api.GetAsync(ApiUrls.BookingSearch, id);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(bookingViewModel);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
