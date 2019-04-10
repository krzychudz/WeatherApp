using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GetGeolocation();


            MessagingCenter.Subscribe<MainActivity>(this, "changeLabel", (sender, e) => {
               sensorInfo.Text = e;
               });

        }

        void GetGeolocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                Toast.MakeText(this, location.Latitude, ToastLength.Long).Show();
            }
        }

    }
}
