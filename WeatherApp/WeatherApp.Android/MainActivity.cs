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
        TextView date;

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
            date = FindViewById<TextView>(Resource.Id.date);

            String myDate = DateTime.Now.ToString("dd/MM/yyyy");
            date.Text = myDate;

            sensorManager = (SensorManager)GetSystemService(SensorService);
            Sensor sen = sensorManager.GetDefaultSensor(SensorType.Light);
            sensorManager.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);

            Device.StartTimer(TimeSpan.FromSeconds(1),  updateClock);
            

            LoadApplication(new App());
           
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
            }
            else if (lx > 100 && lx <= 200)
            {
                mainLayout.SetBackgroundResource(Resource.Drawable.sky_cloud);
                sensorInfo.SetTextColor(Android.Graphics.Color.Black);
                weatherInfo.Text = "Zachmurzenie";
                weatherInfo.SetTextColor(Android.Graphics.Color.Black);
            }
            else
            {
                mainLayout.SetBackgroundResource(Resource.Drawable.sky_sunny);
                sensorInfo.SetTextColor(Android.Graphics.Color.Black);
                weatherInfo.Text = "Słonecznie";
                weatherInfo.SetTextColor(Android.Graphics.Color.Black);
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