namespace TaxiBookingServiceUI.Controllers
{
    public class StateController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();
        public async Task<ActionResult> Display()
        {
            try
            {
                List<StateViewModel> states = new List<StateViewModel>();

                HttpResponseMessage res = await _api.GetAsync(ApiUrls.StateDisplay, 0);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    states = JsonConvert.DeserializeObject<List<StateViewModel>>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return View(states);
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
        public ActionResult Add(StateModel stateModel)
        {
            try
            {
                stateModel.UserEmail = HttpContext.Session.GetString("_adminEmail");
                string token = HttpContext.Session.GetString("_admintoken");
                if (!String.IsNullOrEmpty(stateModel.State))
                {
                    HttpResponseMessage result = _api.PostAsync(stateModel, ApiUrls.StateAdd, token);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "State Added Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "State could not be added";
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
                StateViewModel state = new StateViewModel();

                HttpResponseMessage res = await _api.GetAsync(ApiUrls.StateSearch, id);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    state = JsonConvert.DeserializeObject<StateViewModel>(result);

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
        public ActionResult Edit(int id, StateModel stateView)
        {
            try
            {
                stateView.UserEmail = HttpContext.Session.GetString("_adminEmail");
                
                    var result = _api.PutAsync(ApiUrls.StateUpdate, stateView, id, null);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "State Updated Successfully";
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
