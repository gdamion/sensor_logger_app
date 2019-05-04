using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Hardware;
using Android.Util;
using Java.IO;
using Java.Lang;
using Java.Util.Regex;
using System;
using System.Text;
using Android.OS;

namespace logger3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISensorEventListener
    {
        static readonly object _syncLock = new object();

        SensorManager _SensorManager;
        TextView _accTextView;
        TextView _gyrTextView;
        TextView _magTextView;
        Button _btnStart;
        Button _btnFinish;

        File myFile;
        Java.IO.FileOutputStream fOut;
        Java.IO.OutputStreamWriter myOutWriter;
        BufferedWriter myBufferedWriter;
        PrintWriter myPrintWriter;
        bool status = false;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            _SensorManager = (SensorManager)GetSystemService(SensorService);

            _accTextView = FindViewById<TextView>(Resource.Id.acc_data);
            _gyrTextView = FindViewById<TextView>(Resource.Id.gyr_data);
            _magTextView = FindViewById<TextView>(Resource.Id.mag_data);
            _btnStart = FindViewById<Button>(Resource.Id.btnStart);
            _btnFinish = FindViewById<Button>(Resource.Id.btnFinish);

			_btnStart.Click += (sender, e) =>
			{
				status = true;
				_btnStart.Clickable = false;
				_btnFinish.Clickable = true;

				//string storepath = "/storage/emulated/0/test.txt";
				//string storepath = "/sdcard/android/data/test.txt";
				string storepath = Android.OS.Environment.DirectoryDownloads;

				myFile = new File(storepath + "/acc.csv");
				if (myFile.Exists())
				{
					myFile.Delete();
				}
				myFile.CreateNewFile();
				//FileStream fs = File.Open(filepath, FileMode.Create);
				/*
				//string storepath = <string>Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
				myFile = new File(storepath + "acc.csv");
				if (myFile.Exists())
				{
					myFile.Delete();
				}
				myFile.CreateNewFile();
				*/
				/*
                Toast.MakeText(GetText, "Started Recording Data and Storing at" + storepath, Toast.LENGTH_SHORT).Show();
                try
                {
					
					myFile = new File(storepath + "/myLog/" + "acc.csv");
					if (myFile.Exists())
					{
						myFile.Delete();
					}
					myFile.CreateNewFile();

                    fOut = new FileOutputStream(myFile);
					myOutWriter = new OutputStreamWriter(fOut);
					myBufferedWriter = new BufferedWriter(myOutWriter);
					myPrintWriter = new PrintWriter(myBufferedWriter);
					
                }
                catch (Exception e)
                {
                    Toast.makeText(getBaseContext(), e.getMessage(), Toast.LENGTH_SHORT).show();
                }
				*/
			};

			/*
			_btnFinish.Click += (sender, e) =>
            {
                status = false;
                _btnStart.Clickable = true;
                _btnFinish.Clickable = false;

                Toast.makeText(getBaseContext(), "Stopped Recording", Toast.LENGTH_SHORT).show();
                startFlag = false;
                try
                {
                    fOut.close();
                }
                catch (IOException e)
                {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
            };
			*/
        }

 

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }
        public void OnSensorChanged(SensorEvent e)
        {
            var sensor = e.Sensor;
            lock (_syncLock)
            {
                if (sensor.Type == SensorType.Accelerometer)
                {
                    _accTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                }
                else if (sensor.Type == SensorType.Gyroscope)
                {
                    _gyrTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                }
                else
                {
                    _magTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
                }
				/*
				try
				{
					fOut = new FileOutputStream(myFile);
					myOutWriter = new OutputStreamWriter(fOut);
					myBufferedWriter = new BufferedWriter(myOutWriter);
					myPrintWriter = new PrintWriter(myBufferedWriter);
					myPrintWriter.append(curTime - lastUpdate + ", " + x_float + ", " + y_float + ", " + z_float + "\n");


				}
				catch (IOException e)
				{
					System.out.println("Exception: " + e);
				}
				*/
			}
        }

        protected override void OnResume()
        {
            base.OnResume();
            _SensorManager.RegisterListener(this, _SensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
            _SensorManager.RegisterListener(this, _SensorManager.GetDefaultSensor(SensorType.Gyroscope), SensorDelay.Ui);
            _SensorManager.RegisterListener(this, _SensorManager.GetDefaultSensor(SensorType.MagneticField), SensorDelay.Ui);
        }
        protected override void OnPause()
        {
            base.OnPause();
            _SensorManager.UnregisterListener(this);

        }
       
    }
}