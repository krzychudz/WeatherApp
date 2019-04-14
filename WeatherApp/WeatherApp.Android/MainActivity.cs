using System;

using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Xamarin.Forms;
using Android.Locations;

using System.Net.Http;
using Plugin.Geolocator;
using System.Threading.Tasks;



namespace WeatherApp.Droid
{
    [Activity(Label = "WeatherApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener, ILocationListener
    {

        
        private SensorManager sensorManager;
        float lightSensorValue;

        LinearLayout mainLayout;
        TextView sensorInfo;
        TextView weatherInfo;
        TextView clock;
        TextView day;
        TextView month;
        TextView year;
        ImageView pictogram;
        TextView city;
        TextView temp;

        private string tmp;			  //	TUTAJ BEDZIE TEMPERATURA JAK WSZYSTKO SIE WYKONA
        private string lat = "52";   //
        private string lon = "16";	//
        private readonly HttpClient _client = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
           TabLayoutResource = Resource.Layout.Tabbar;
           ToolbarResource = Resource.Layout.Toolbar;

         

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);

            sensorInfo = FindViewById<TextView>(Resource.Id.sensorInfo);
            mainLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            weatherInfo = FindViewById<TextView>(Resource.Id.weatherInfo);
            clock = FindViewById<TextView>(Resource.Id.clock);
            day = FindViewById<TextView>(Resource.Id.day);
            month = FindViewById<TextView>(Resource.Id.month);
            year = FindViewById<TextView>(Resource.Id.year);
            pictogram = FindViewById<ImageView>(Resource.Id.Pictogram);
            city = FindViewById<TextView>(Resource.Id.city); 
            temp = FindViewById<TextView>(Resource.Id.temp); 


            setDate();
            getTemperature();

            sensorManager = (SensorManager)GetSystemService(SensorService);
            Sensor sen = sensorManager.GetDefaultSensor(SensorType.Light);
            sensorManager.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);

            Device.StartTimer(TimeSpan.FromSeconds(1),  updateClock);
            

            LoadApplication(new App());
           
        }
        
        public async void getTemperature()
        {    
           // await RetreiveLocation();      
            string url = String.Concat("https://api.openweathermap.org/data/2.5/weather?lat=", lat, "&lon=", lon, "&units=metric&appid=bf0be71b211a6aa04facb044788d74ec");      //
            await tempFromUrl(url);
            temp.Text = "Temperatura na zewnątrz: " + tmp;               
        }

        private async Task RetreiveLocation()												
        {																					
            var locator = CrossGeolocator.Current;											
            locator.DesiredAccuracy = 20;														
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1));																					
        }

        private async Task tempFromUrl(string url)									
        {																						
            HttpResponseMessage response = await _client.GetAsync(url);						
            if (response.IsSuccessStatusCode)													
            {																				
                string content = await response.Content.ReadAsStringAsync();		
                string[] splitCont = content.Split(',');								
                for (int i = 0; i < splitCont.Length; i++)
                {								
                    if (splitCont[i].Contains("\"main\":{\"temp\":"))
                    {				
                        string temp = splitCont[i].Substring(15);								
                        this.tmp = temp;													
                    }																	
                }																
            }																				
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode,
                permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions,
                grantResults);
        }

        private void setDate() {

            string[] months = new string[12] {"Stycznia", "Lutego", "Marca", "Kwietnia", "Maja", "Czerwca", "Lipca", "Sierpnia", "Września", "Października", "Listopada", "Grudnia"};

            String dayS = DateTime.Now.ToString("dd");
            String monthS = DateTime.Now.ToString("MM");
            String yearS = DateTime.Now.ToString("yyyy");

            day.Text = dayS;
            month.Text = months[Int32.Parse(monthS) - 1];
            year.Text = yearS;
        }

        private bool updateClock() {

            Device.BeginInvokeOnMainThread(() =>
            {
                String myDate = DateTime.Now.ToString("HH:mm:ss");


                clock.Text = myDate;

            });

      
            return true;
        }

        private void changeBackgorund(float lx)
        {

            if (lx <= 100)
            {
                mainLayout.SetBackgroundResource(Resource.Drawable.sky_storm);
                sensorInfo.SetTextColor(Android.Graphics.Color.White);
                weatherInfo.Text = "Silne zachmurzenie";
                weatherInfo.SetTextColor(Android.Graphics.Color.White);
                clock.SetTextColor(Android.Graphics.Color.White);
                pictogram.SetImageResource(Resource.Drawable.cloud_pictogram);
                day.SetTextColor(Android.Graphics.Color.White);
                month.SetTextColor(Android.Graphics.Color.White);
                year.SetTextColor(Android.Graphics.Color.White);
                city.SetTextColor(Android.Graphics.Color.White);
            }
            else if (lx > 100 && lx <= 200)
            {
                mainLayout.SetBackgroundResource(Resource.Drawable.sky_cloud);
                sensorInfo.SetTextColor(Android.Graphics.Color.Black);
                weatherInfo.Text = "Zachmurzenie";
                weatherInfo.SetTextColor(Android.Graphics.Color.Black);
                clock.SetTextColor(Android.Graphics.Color.Black);
                pictogram.SetImageResource(Resource.Drawable.cloud_sun_pictogram);
                day.SetTextColor(Android.Graphics.Color.Black);
                month.SetTextColor(Android.Graphics.Color.Black);
                year.SetTextColor(Android.Graphics.Color.Black);
                city.SetTextColor(Android.Graphics.Color.Black);
            }
            else
            {
                mainLayout.SetBackgroundResource(Resource.Drawable.sky_sunny);
                sensorInfo.SetTextColor(Android.Graphics.Color.Black);
                weatherInfo.Text = "Słonecznie";
                weatherInfo.SetTextColor(Android.Graphics.Color.Black);
                clock.SetTextColor(Android.Graphics.Color.Black);
                pictogram.SetImageResource(Resource.Drawable.sun_pictogram);
                day.SetTextColor(Android.Graphics.Color.Black);
                month.SetTextColor(Android.Graphics.Color.Black);
                year.SetTextColor(Android.Graphics.Color.Black);
                city.SetTextColor(Android.Graphics.Color.Black);
            }

        }

        public void OnSensorChanged(SensorEvent s)
        {

            s.Sensor = sensorManager.GetDefaultSensor(SensorType.Light);
            lightSensorValue = s.Values[0];
            
          
            sensorInfo.Text = "Natężenie: " + lightSensorValue.ToString("0.00") + "lx";

            changeBackgorund(s.Values[0]);
     

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnLocationChanged(Location location)
        {
           Console.WriteLine(location.Latitude);


            throw new NotImplementedException();
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}