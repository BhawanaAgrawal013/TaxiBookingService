namespace TaxiBookingServiceUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public const string SessionLocation = "_driverLocation";

        GetStringList getName = new GetStringList();
        public IActionResult DriverLocation()
        {
            try
            {
                ViewBag.States = getName.GetStates();
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult DriverLocation(LocationViewModel locationView)
        {
            try
            {
                ViewBag.States = getName.GetStates();

                locationView.UserEmail = HttpContext.Session.GetString("_driverEmail");
                string token = HttpContext.Session.GetString("_drivertoken");
                if (!String.IsNullOrEmpty(locationView.State))
                {
                    var result = _api.PostAsync(locationView, ApiUrls.LocationAdd, token);

                    if (result.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString(SessionLocation, "true");
                        TempData["msg"] = "Location is updated, you can accept rides";
                        return RedirectToAction("GetRequest", "Driver");
                    }
                    else
                    {
                         return View();
                    }
                }
                TempData["errmsg"] = "Location could not be iupdated";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult GetCities(string state)
        {
            return Json(getName.GetCity(state));
        }

        [HttpPost]
        public ActionResult GetArea(string city)
        {
            return Json(getName.GetArea(city));
        }

        
    }
}
