using System;

using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;





namespace WeatherApp.Droid
{
    [Activity(Label = "WeatherApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener
    {

        private SensorManager sensorManager;
        float lightSensorValue;

   
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            sensorManager = (SensorManager)GetSystemService(SensorService);
            Sensor sen = sensorManager.GetDefaultSensor(SensorType.Light);
            sensorManager.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);

       
           
            LoadApplication(new App());
           
        }

        public void OnSensorChanged(SensorEvent s)
        {
            s.Sensor = sensorManager.GetDefaultSensor(SensorType.Light);
            lightSensorValue = s.Values[0];
            Toast.MakeText(this, lightSensorValue.ToString("0.00"), ToastLength.Long).Show();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }
    }
}