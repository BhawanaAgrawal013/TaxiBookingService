using TaxiBookingServiceUI.DTOModels;

namespace TaxiBookingServiceUI.Controllers
{
    public class CancellationReasonController : Controller
    {
        private readonly HelperAPI _api = new HelperAPI();
        public async Task<ActionResult> Display()
        {
            try
            {
                List<CancellationResonViewModel> states = new List<CancellationResonViewModel>();

                HttpResponseMessage res = await _api.GetAsync(ApiUrls.CancelReasonDisplay, 0);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    states = JsonConvert.DeserializeObject<List<CancellationResonViewModel>>(result);
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
        public ActionResult Add(CancellationReasonModel cancellationReason)
        {
            try
            {
                cancellationReason.UserEmail = HttpContext.Session.GetString("_adminEmail");
                string token = HttpContext.Session.GetString("_admintoken");

                if (!String.IsNullOrEmpty(cancellationReason.Reason))
                {
                    HttpResponseMessage result = _api.PostAsync(cancellationReason, ApiUrls.CancelReasonAdd, token);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Reason Added Successfully";
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        return View();
                    }
                }
                TempData["errmsg"] = "Reason could not be added";
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
