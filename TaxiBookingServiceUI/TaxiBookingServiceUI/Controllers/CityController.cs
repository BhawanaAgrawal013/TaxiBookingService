using TaxiBookingServiceUI.Models;

namespace TaxiBookingServiceUI.Controllers
{
    public class CityController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();

        GetStringList getName = new GetStringList();
        public async Task<ActionResult> Display()
        {
            try
            {
                List<CityViewModel> cities = new List<CityViewModel>();
                HttpResponseMessage res = await _api.GetAsync(ApiUrls.CityDisplay, 0);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    cities = JsonConvert.DeserializeObject<List<CityViewModel>>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(cities);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Add()
        {
            ViewBag.States = getName.GetStates();
            return View();
            
        }

        [HttpPost]
        public ActionResult Add(CityModel cityModel)
        {
            try
            {
                ViewBag.States = getName.GetStates();
                cityModel.UserEmail = HttpContext.Session.GetString("_adminEmail");
                string token = HttpContext.Session.GetString("_admintoken");
                if (!String.IsNullOrEmpty(cityModel.State))
                {
                    var result = _api.PostAsync(cityModel, ApiUrls.CityAdd, token);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "City Added Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "City could not be added";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ViewBag.States = getName.GetStates();
                CityViewModel state = new CityViewModel();

                HttpResponseMessage res = await _api.GetAsync(ApiUrls.CitySearch, id);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    state = JsonConvert.DeserializeObject<CityViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


                return View(state);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, CityModel cityModel)
        {
            try
            {
                ViewBag.States = getName.GetStates();
                cityModel.UserEmail = HttpContext.Session.GetString("_adminEmail");
                    var result = _api.PutAsync(ApiUrls.CityUpdate, cityModel, id, null);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "City Updated Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        TempData["errmsg"] = "You are currently unauthorized";
                        return RedirectToAction("Display");
                    }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("_admintoken");
                HttpResponseMessage res = await _api.DeleteAsync(ApiUrls.CityDelete, id, token);

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "City Deleted Successfully";
                    return RedirectToAction("Display");
                }
                else
                {
                    TempData["errmsg"] = "You are currently unauthorized";
                    return RedirectToAction("Display");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
