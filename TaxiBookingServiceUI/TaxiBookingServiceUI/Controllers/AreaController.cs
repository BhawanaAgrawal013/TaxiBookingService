namespace TaxiBookingServiceUI.Controllers
{
    public class AreaController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();
        private readonly GetStringList getName = new GetStringList();
        public async Task<ActionResult> Display()
        {
            try
            {
                List<AreaViewModel> users = new List<AreaViewModel>();
                HttpResponseMessage res = await _api.GetAsync(ApiUrls.AreaDisplay, 0);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<AreaViewModel>>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(users);
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
        public ActionResult Add(AreaModel areaModel)
        {
            try
            {
                ViewBag.States = getName.GetStates();
                areaModel.UserEmail = HttpContext.Session.GetString("_adminEmail");
                string token = HttpContext.Session.GetString("_admintoken");
                HttpResponseMessage res = _api.PostAsync(areaModel, ApiUrls.AreaAdd, token);

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Area Added Successfully";
                    return RedirectToAction("Display");
                }
                else
                {
                    return View();
                }
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
                AreaViewModel area = new AreaViewModel();
                HttpResponseMessage res = await _api.GetAsync(ApiUrls.AreaSearch, id);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    area = JsonConvert.DeserializeObject<AreaViewModel>(result);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(area);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, AreaModel areaModel)
        {
            try
            {
                ViewBag.States = getName.GetStates();
                areaModel.UserEmail = HttpContext.Session.GetString("_adminEmail");
                string token = HttpContext.Session.GetString("_admintoken");
                HttpResponseMessage res = _api.PutAsync(ApiUrls.AreaUpdate, areaModel, id, token);

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Area Updated Successfully";
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
                HttpResponseMessage res = await _api.DeleteAsync(ApiUrls.AreaDelete, id, token);

                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Area Deleted Successfully";
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
