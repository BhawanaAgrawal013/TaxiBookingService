using TaxiBookingServiceUI.DTOModels;

namespace TaxiBookingServiceUI.Controllers
{
    public class DriverController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();
        private readonly GetStringList getName = new GetStringList();
        private readonly BookingService bookingService = new BookingService();

        public const string SessionDriverToken = "_drivertoken";

        public const string SessionDriverEmail = "_driverEmail";

        public readonly IHttpContextAccessor _httpContext;

        public DriverController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Profile()
        {
            try
            {
                DriverViewModel drivers = new DriverViewModel();

                string driverEmail = HttpContext.Session.GetString("_driverEmail");

                HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.DriverSearch, driverEmail, null);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    drivers = JsonConvert.DeserializeObject<DriverViewModel>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(drivers);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Add()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Add(DriverViewModel driverView)
        {
            try
            {
                if (driverView.PhoneNumber.Length > 10) return View();

                driverView.RoleName = Roles.Driver.ToString();
                if (ModelState.IsValid)
                {
                    var result = _api.PostAsync(driverView, ApiUrls.DriverAdd, null);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Please Login using your email and password";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "Please check your entries again";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public IActionResult Login()
        {
            HttpContext.Session.Remove(SessionDriverEmail);
            HttpContext.Session.Remove(SessionDriverToken);
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                var result = _api.Login(login, ApiUrls.Login);

                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    HttpContext.Session.SetString(SessionDriverToken, res);
                    HttpContext.Session.SetString(SessionDriverEmail, login.Email);
                    return RedirectToAction("DriverLocation", "Location");
                }
                else
                {
                    TempData["errmsg"] = "Wrong Email or Password entered";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> GetRequest()
        {
            try
            {
                if (HttpContext.Session.GetString("_driverLocation") != "true")
                {
                    return RedirectToAction("DriverLocation", "Location");
                }

                BookingRequest bookings = new BookingRequest();

                string email = HttpContext.Session.GetString("_driverEmail");

                HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.BookingRequest, email, null);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    bookings = JsonConvert.DeserializeObject<BookingRequest>(result);
                }
                else if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                return View(bookings);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult AcceptRequest(int id)
        {
            try
            {
                BookingViewModel booking = new BookingViewModel();
                string driverEmail = HttpContext.Session.GetString("_driverEmail");
                string token = HttpContext.Session.GetString("_drivertoken");
                using (HttpClient client = _api.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                    var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/AcceptRequest/{id}/{driverEmail}");
                    var postTask = client.SendAsync(message);
                    postTask.Wait();

                    var res = postTask.Result;

                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        booking = JsonConvert.DeserializeObject<BookingViewModel>(result);

                    }
                    else
                    {
                        TempData["errmsg"] = "You are not approved by the Admin yet, please wait";
                        return RedirectToAction("GetRequest");
                    }
                    TempData["msg"] = "Your Ride has started";
                    return RedirectToAction("CurrentRide", new { id = id });
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public  ActionResult CompleteRide(int id)
        {
            try
            {
                BookingViewModel booking = new BookingViewModel();
                string token = HttpContext.Session.GetString("_drivertoken");
                using (HttpClient client = _api.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                    var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/CompleteRide/{id}");
                    var postTask = client.SendAsync(message);
                    postTask.Wait();

                    var res = postTask.Result;

                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        booking = JsonConvert.DeserializeObject<BookingViewModel>(result);

                        if(String.Equals(booking.PaymentMode, "Wallet"))
                        {
                            bookingService.VerifyWalletPayment(booking.Id, token, true, "Wallet", null); 
                            HttpContext.Session.SetString("_driverLocation", "false");
                            TempData["msg"] = "Please provide a rating for the driver";
                            return RedirectToAction("Rating", new { id = id });
                        }
                        else
                        {
                            TempData["msg"] = "Please verify the payment details, for your convinience";
                            return RedirectToAction("CurrentRide", new { id =booking.Id });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> CurrentRide(int id)
        {
            ViewBag.PaymentModes = getName.GetPaymentModes();

            BookingViewModel bookingViewModel = new BookingViewModel();
            HttpResponseMessage res = await _api.GetAsync(ApiUrls.BookingSearch, id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(result);
            }
            else
            {
                TempData["errmsg"] = "A error has occured in accepting the request. Please try again";
                return RedirectToAction("Index");
            }
            if(bookingViewModel.Status == "Declined")
            {
                TempData["errmsg"] = "User delined the ride, please check your history to see the reason";
                return RedirectToAction("GetRequest");
            }
            return View(bookingViewModel);
        }

        public ActionResult ReachedLocation(int id, string Otp)
        {
            string token = HttpContext.Session.GetString("_drivertoken");

            string otp = TempData["Otp"].ToString();

            if (Otp != otp)
            {
                TempData["errmsg"] = "Wrong OTP entered";
                return RedirectToAction("CurrentRide", new { id = id });
            }

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/ArrivedLocation/{id}");
                var postTask = client.SendAsync(message);
                postTask.Wait();

                var res = postTask.Result;

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    ViewBag.PaymentModes = getName.GetPaymentModes();
                    return RedirectToAction("CurrentRide", new { id = id });
                }
                return RedirectToAction("DriverLocation", "Location");
            }
        }

        public ActionResult PaymentVerfied(int id, string Verify, string PaymentMode, string Remark)
        {
            string token = HttpContext.Session.GetString("_drivertoken");
            bool verified = false;
            if (Verify == "True") verified = true;

            if(bookingService.VerifyWalletPayment(id, token, verified, PaymentMode, Remark))
            {
                HttpContext.Session.SetString("_driverLocation", "false");
                return RedirectToAction("Rating", new { id = id });
            }
            else
            {
                ViewBag.PaymentModes = getName.GetPaymentModes();
                TempData["errmsg"] = "Payment could not be verified, please try again";
                return RedirectToAction("CurrentRide", new { id = id });
            }
        }

        public async Task<ActionResult> Rating(int id)
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

        [HttpPost]
        public ActionResult Rating(BookingViewModel bookingView, string Comment, int Rating)
        {
            try
            {
                RatingModel ratingModel = new()
                {
                    Comment = Comment,
                    Ratings = Rating,
                    Email = bookingView.UserEmail,
                    BookingId = bookingView.Id
                };

                var result = _api.PostAsync(ratingModel, ApiUrls.UserRating, null);

                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    TempData["msg"] = "Please update your location to book another ride";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(bookingView);
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
