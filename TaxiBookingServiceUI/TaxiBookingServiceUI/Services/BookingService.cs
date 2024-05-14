using BingMapsRESTToolkit;

namespace TaxiBookingServiceUI.Services
{
    public class BookingService
    {
        private readonly HelperAPI _api = new HelperAPI();
        public bool UserBooking(BookingViewModel bookingView)
        {
            Coordinate PickupCoordinate = CheckCoordinates(bookingView.PickupLocation).Result;
            Coordinate DropoffCoordinate = CheckCoordinates(bookingView.DropLocation).Result;

            double distance = CalculateDistance(PickupCoordinate.lat, PickupCoordinate.lon, DropoffCoordinate.lat, DropoffCoordinate.lon);

            var speed = 60.0f;

            double TotalTime = 60 * (distance / speed);

            bookingView.Payment = CalculatePayment(distance, bookingView.UserEmail);

            bookingView.DropTime = bookingView.PickupTime.Add(TimeSpan.FromMinutes(TotalTime));
            return true;
        }

        public List<AvailableVehicleModel> GetAvailableVehicles(BookingViewModel bookingView)
        {
            Coordinate PickupCoordinate = CheckCoordinates(bookingView.PickupLocation).Result;

            List<AvailableVehicleModel> availableVehicles = new List<AvailableVehicleModel>();

            using (HttpClient client = _api.Initial())
            {
                var message = new HttpRequestMessage(HttpMethod.Post, $"api/Driver/AvailableDriver/{PickupCoordinate.lat}/{PickupCoordinate.lon}");
                var postTask = client.SendAsync(message);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var res = result.Content.ReadAsStringAsync().Result;
                    availableVehicles = JsonConvert.DeserializeObject<List<AvailableVehicleModel>>(res);
                    return availableVehicles;
                }
                return null;
            }
        }

        public async Task<AvailableVehicleModel> CurrentDriver(string driverEmail)
        {
            AvailableVehicleModel currentDriver = new AvailableVehicleModel();
            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.CurrentDriver, driverEmail, null);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                currentDriver = JsonConvert.DeserializeObject<AvailableVehicleModel>(result);
                return currentDriver;
            }
            return null;
        }

        public decimal CalculatePayment(double distance, string email)
        {
            double payment = 20;

            double cancellationCharge = CancellationTax(email).Result;

            if (distance <= 6) payment = payment + 40 + cancellationCharge;

            else
            {
                payment = payment + (distance * 4) + cancellationCharge;
            }

            return (decimal)payment;
        }

        public double CalculateDistance(double pickupLat, double pickupLon, double dropLat, double dropLon)
        {
            const double earthRadius = 6371; // in km
            var dLat = ToRadians(dropLat - pickupLat);
            var dLon = ToRadians(dropLon - pickupLon);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(pickupLat)) * Math.Cos(ToRadians(dropLat)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadius * c;

            return distance;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public async Task<Coordinate> CheckCoordinates(string address)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://eu1.locationiq.com/v1/");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "key", parameter: "pk.b34504f0ccdf2f561a377ee40b7fadd7");

                HttpResponseMessage res = await client.GetAsync($"search?key=pk.b34504f0ccdf2f561a377ee40b7fadd7&q={address}&countrycodes=in&limit=1&bounded=1&format=json");

                string result = "\0";
                List<Coordinate> coords = new List<Coordinate>();

                if (res.IsSuccessStatusCode)
                {
                    result = res.Content.ReadAsStringAsync().Result;
                    coords = JsonConvert.DeserializeObject<List<Coordinate>>(result);
                }

                return coords[0];
            }
        }

        public async Task<double> CancellationTax(string email)
        {
            double amount = 0;
            List<CancelledBookingView> cancelledBookings = new List<CancelledBookingView>();
            HttpResponseMessage res = await _api.GetByStringAsync(ApiUrls.CancelledBooking, email, null);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                cancelledBookings = JsonConvert.DeserializeObject<List<CancelledBookingView>>(result);
            }
            else
            {
                return 0;
            }

            foreach (var cancelledBooking in cancelledBookings)
            {
                if (cancelledBooking.IsPending && cancelledBooking.IsCharged)
                {
                    amount = (double)cancelledBooking.Amount * 0.2;
                }
            }

            return amount;
        }

        public bool PayCancellationFee(string email)
        {
            using (HttpClient client = _api.Initial())
            {
                var message = new HttpRequestMessage(HttpMethod.Put, $"api/Booking/PayCancellationFee/{email}");
                var postTask = client.SendAsync(message);
                postTask.Wait();

                var res = postTask.Result;

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> GetDriver(BookingViewModel bookingView)
        {
            var availableVehicles = GetAvailableVehicles(bookingView);
            int index = 0;

            while (bookingView.Status != Statuses.Accepted.ToString() && index < availableVehicles.Count)
            {
                bookingView.DriverEmail = availableVehicles[index].Email;

                using (HttpClient client = _api.Initial())
                {
                    var res = await client.PutAsync($"api/Booking/PostRequest/{bookingView.Id}/{bookingView.DriverEmail}", null);

                    if (res.IsSuccessStatusCode)
                    {
                       await Task.Delay(20000);
                        if (BookingStatus(bookingView.Id).Result == Statuses.Accepted.ToString())
                        {
                            return true;
                        }
                        else
                        {
                            index++;
                            continue;
                        }
                    }
                }
            }
            bookingView.DriverEmail = string.Empty;
            return false;
        }

        public async Task<string> BookingStatus(int id)
        {
            BookingViewModel bookingViewModel = new BookingViewModel();
            HttpResponseMessage res = await _api.GetAsync(ApiUrls.BookingSearch, id);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                bookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(result);

                return bookingViewModel.Status;
            }
            else
            {
                return null;
            }
        }

        public bool VerifyWalletPayment(int id, string token, bool verified, string mode, string remark)
        {
            PaymentVerificationModel verificationModel = new()
            {
                IsVerify = verified,
                Remark = remark,
                PaymentMode = mode
            };

            /*using (HttpClient client = _api.Initial())
            {*/
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                //var message = new HttpRequestMessage(HttpMethod.Put, $"api/Payment/PaymentVerify/{id}/{verified}");
                /* var message = new HttpRequestMessage(HttpMethod.Put, )
                 var postTask = client.SendAsync(message);
                 postTask.Wait();

                 var res = postTask.Result;*/

                var res = _api.PutAsync(ApiUrls.PaymentVerification, verificationModel, id, token);

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    return true;
                }
                return false;
            //}
        }
    }
}
