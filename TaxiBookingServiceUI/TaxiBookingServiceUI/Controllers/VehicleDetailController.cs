namespace TaxiBookingServiceUI.Controllers
{
    public class VehicleDetailController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        public async Task<ActionResult> Display()
        {
            try
            {
                VehicleDetailViewModel vehicle = new VehicleDetailViewModel();
                string UserEmail = HttpContext.Session.GetString("_driverEmail");

                HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.VehicleDetailSearch, UserEmail, null);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    vehicle = JsonConvert.DeserializeObject<VehicleDetailViewModel>(result);
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("Add");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(vehicle);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Add()
        {
            try
            {
                GetStringList getName = new GetStringList();
                ViewBag.VehicleTypes = getName.GetVehicleType();
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Add(VehicleDetailModel vehicleDetail)
        {
            try
            {
                GetStringList getName = new GetStringList();
                ViewBag.VehicleTypes = getName.GetVehicleType();

                vehicleDetail.UserEmail = HttpContext.Session.GetString("_driverEmail");
                if (ModelState.IsValid)
                {
                    var result = _api.PostAsync(vehicleDetail, ApiUrls.VehicleDetailAdd, null);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Vehicle Detail Added Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "Vehicle Detail could not be added";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Edit()
        {
            try
            {
                VehicleDetailViewModel user = new VehicleDetailViewModel();
                string email = HttpContext.Session.GetString("_driverEmail");

                HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.VehicleDetailSearch, email, null);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<VehicleDetailViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Edit(string email, VehicleDetailModel vehicleDetail)
        {
            try
            {
                vehicleDetail.UserEmail = HttpContext.Session.GetString("_driverEmail");
                string token = HttpContext.Session.GetString("_drivertoken");
                if (ModelState.IsValid)
                {
                    var result = _api.PutByStringAsync(ApiUrls.VehicleDetailUpdate, vehicleDetail, vehicleDetail.UserEmail, token);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Vehicle Detail Updated Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        TempData["errmsg"] = "You are currently unauthorized";
                        return RedirectToAction("Display");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
