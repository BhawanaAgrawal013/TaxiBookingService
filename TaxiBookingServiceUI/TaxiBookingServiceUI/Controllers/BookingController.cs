using TaxiBookingServiceUI.DTOModels;

namespace TaxiBookingServiceUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();
        private readonly GetStringList getName = new GetStringList();

        BookingService _booking = new BookingService();
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult UserBooking()
        {
            try
            {
                ViewBag.PaymentModes = getName.GetPaymentModes();
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UserBooking(BookingViewModel bookingView)
        {
            try
            {
                if (String.IsNullOrEmpty(bookingView.PaymentMode) || bookingView.PhoneNumber.Length < 10 || bookingView.PhoneNumber.Length > 10)
                {
                    ViewBag.PaymentModes = getName.GetPaymentModes();
                    TempData["errmsg"] = "Please select the correct details";
                    return View();
                }

                bookingView.UserEmail = HttpContext.Session.GetString("_userEmail");

                if (_booking.UserBooking(bookingView))
                {
                    TempData["bookingView"] = JsonConvert.SerializeObject(bookingView);
                    TempData.Keep();
                    return RedirectToAction("AvailableRides");
                }

                TempData["errmsg"] = "Your Booking cannot be added, please try again";
                return View();
            }
            catch
            {
                ViewBag.PaymentModes = getName.GetPaymentModes();
                TempData["errmsg"] = "The location is not available";
                return View();
            }
        }

        public ActionResult AvailableRides()
        {
            try
            {
                BookingViewModel bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(TempData["bookingView"].ToString());

                var vehicles = _booking.GetAvailableVehicles(bookingViewModel);

                List<string> vehicleTypes = new List<string>();

                foreach(var vehicleType in vehicles)
                {
                    vehicleTypes.Add(vehicleType.VehicleType.ToString());
                }

                if(vehicleTypes.Count == 0)
                {

                    TempData["errmsg"] = "There are no rides available for your location";
                    return RedirectToAction("UserBooking");
                }

                ViewBag.VehicleTypes = vehicleTypes.Distinct().ToList();

                dynamic mymodel = new ExpandoObject();
                mymodel.Booking = bookingViewModel;
                ViewBag.vehicles = vehicles.Count;
                return View(mymodel);
            }
            catch
            {
                TempData["errmsg"] = "There are no rides available for your location";
                return RedirectToAction("UserBooking");
            }
        }

        [HttpPost]
        public ActionResult AvailableRides(BookingViewModel bookingView, string preference)
        {
            try
            {
                ViewBag.PaymentModes = getName.GetPaymentModes();

                bookingView.VehiclePreference = preference;
                int id;
                bookingView.UserEmail = HttpContext.Session.GetString("_userEmail");

                if (!String.IsNullOrEmpty(preference))
                {
                    var result = _api.PostAsync(bookingView, ApiUrls.BookingAdd, null);

                    if (result.IsSuccessStatusCode)
                    {
                        var res = result.Content.ReadAsStringAsync().Result;
                        id = JsonConvert.DeserializeObject<int>(res);
                        TempData["msg"] = "Booking Requested Successfully";
                        return RedirectToAction("Waiting", new { id = id });
                    }
                    else if(result.StatusCode == System.Net.HttpStatusCode.NotFound && bookingView.PaymentMode == "Wallet")
                    {
                        TempData["errmsg"] = "Please check your wallet";
                    }
                    else
                    {
                        return View("UserBooking");
                    }
                }
                return View("UserBooking");
            }
            catch (Exception ex)
            {
                ViewBag.PaymentModes = getName.GetPaymentModes();
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Waiting(int id)
        {
            try
            {
                BookingViewModel bookingViewModel = new BookingViewModel();
                HttpResponseMessage res = await _api.GetAsync(ApiUrls.BookingSearch, id);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(result);

                    var response = _booking.GetDriver(bookingViewModel).Result;

                    if (response) return RedirectToAction("Booking", new { id = bookingViewModel.Id });
                    else return RedirectToAction("CancelRequest", new { id = bookingViewModel.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (bookingViewModel.Status == "Accepted")
                {
                    return RedirectToAction("Booking", new { id = bookingViewModel.Id });
                }
                return View(bookingViewModel);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult CancelRequest(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("_usertoken");
                using (HttpClient client = _api.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                    var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/CancelRequest/{id}");
                    var postTask = client.SendAsync(message);
                    postTask.Wait();

                    var res = postTask.Result;

                    if (res.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Your request was cancelled successfully";
                        return RedirectToAction("UserBooking");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    return RedirectToAction("Waiting", new {id = id});
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Booking(int id)
        {
            try
            {
                ViewBag.CancellationReasons = getName.GetCancellationReasons();
                ViewBag.PaymentModes = getName.GetPaymentModes();

                string email = HttpContext.Session.GetString("_userEmail");

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

                var vehicle = _booking.CurrentDriver(bookingViewModel.DriverEmail).Result;

                TempData["Otp"] = bookingViewModel.PhoneNumber[5] + bookingViewModel.PhoneNumber[9] + vehicle.RegistrationNumber[3] + vehicle.RegistrationNumber[1];
                TempData.Keep();

                dynamic mymodel = new ExpandoObject();

                mymodel.Booking = bookingViewModel;

                mymodel.Vehicle = vehicle;

                if (bookingViewModel.Status == "Completed")
                {
                    _booking.PayCancellationFee(email);
                    TempData["msg"] = "Ride is complete, please rate our drivers from 1 to 5";
                    return RedirectToAction("Rating", new { id = bookingViewModel.Id });
                }

                return View(mymodel);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Booking(BookingViewModel bookingViewModel, string CancellationReasons, string PaymentMode)
        {
            try
            {
                ViewBag.CancellationReasons = getName.GetCancellationReasons();
                ViewBag.PaymentModes = getName.GetPaymentModes();

                if (!String.IsNullOrEmpty(CancellationReasons))
                {
                    string token = HttpContext.Session.GetString("_usertoken");
                    using (HttpClient client = _api.Initial())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                        var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/DeclineRequest/{bookingViewModel.Id}/{CancellationReasons}");
                        var postTask = client.SendAsync(message);
                        postTask.Wait();

                        var res = postTask.Result;

                        if (res.IsSuccessStatusCode)
                        {
                            var result = res.Content.ReadAsStringAsync().Result;
                            bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(result);
                            return RedirectToAction("UserBooking");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
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
                    Email = bookingView.DriverEmail,
                    BookingId = bookingView.Id
                };

                var result = _api.PostAsync(ratingModel, ApiUrls.RatingAdd, null);

                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    TempData["msg"] = "Booking Completed Successfully";
                    return RedirectToAction("UserBooking");
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
