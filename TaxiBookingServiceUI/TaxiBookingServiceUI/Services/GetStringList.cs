namespace TaxiBookingServiceUI.Services
{
    public class GetStringList
    {
        private readonly HelperAPI _api = new HelperAPI();
        public List<string> GetStates()
        {
            List<string> stateNames = new List<string>();
            List<StateViewModel> states = new List<StateViewModel>();

            var stateViews = _api.GetName(ApiUrls.StateDisplay, states).Result;

            foreach (var state in stateViews)
            {
                stateNames.Add(state.State);
            }
            return stateNames;
        }

        public List<string> GetVehicleType()
        {
            List<string> typeNames = new List<string>();
            List<VehicleTypeViewModel> types = new List<VehicleTypeViewModel>();

            var res = _api.GetName(ApiUrls.VehicleTypeDisplay, types).Result;

            foreach (var type in res)
            {
                typeNames.Add(type.VehicleType);
            }
            return typeNames;
        }

        public List<string> GetCity(string state)
        {
            List<string> cityNames = new List<string>();
            List<CityViewModel> cities = new List<CityViewModel>();

            var cityViews =  _api.GetName(ApiUrls.CityDisplay, cities).Result;

            foreach (var city in cityViews.Where(e => e.State == state))
            {
                cityNames.Add(city.City);
            }
            return cityNames;
        }

        public List<string> GetArea(string city)
        {
            List<string> areaNames = new List<string>();
            List<AreaViewModel> areas = new List<AreaViewModel>();

            var res = _api.GetName(ApiUrls.AreaDisplay, areas).Result;
            foreach (var area in res.Where(e => e.City == city))
            {
                areaNames.Add(area.Area);
            }
            return areaNames;
        }

        public List<string> GetCities()
        {
            List<string> cityNames = new List<string>();
            List<CityViewModel> cities = new List<CityViewModel>();

            var res = _api.GetName(ApiUrls.CityDisplay, cities).Result;
            foreach (var city in res)
            {
                cityNames.Add(city.City);
            }
            return cityNames;
        }

        public List<string> GetPaymentModes()
        {
            List<string> paymentModeNames = new List<string>();
            List<PaymentModeView> paymentModes = new List<PaymentModeView>();

            var res = _api.GetName(ApiUrls.PaymentModeDisplay, paymentModes).Result;
            foreach (var mode in res)
            {
                paymentModeNames.Add(mode.PaymentMode);
            }
            return paymentModeNames;
        }

        public List<string> GetCancellationReasons()
        {
            List<string> reasons = new List<string>();
            List<CancellationResonViewModel> cancellationResons = new List<CancellationResonViewModel>();

            var res = _api.GetName(ApiUrls.CancellationReasons, cancellationResons).Result;
            foreach (var reason in res)
            {
                reasons.Add(reason.Reason);
            }
            return reasons;
        }
    }
}
