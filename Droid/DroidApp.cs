using System;
using Android.Content;
using System.Threading.Tasks;

namespace BGData.Droid
{
	public class DroidApp
	{
		public event EventHandler<ServiceConnectedEventArgs> BackgroundServiceConnected = delegate {};
		protected static BackgroundServiceConnection _serviceConnection;
		protected static DroidApp current;
		public static DroidApp Current 
		{
			get { return current; }
		}

		static DroidApp()
		{
			current = new DroidApp();
		}

		protected DroidApp() 
		{
			// create a new service connection so we can get a binder to the service
			_serviceConnection = new BackgroundServiceConnection(null);

			// this event will fire when the Service connectin in the OnServiceConnected call 
			_serviceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => 
			{
				// we will use this event to notify MainActivity when to start updating the UI
				this.BackgroundServiceConnected(this, e);
			};
		}

		public static void StartBackgroundService()
		{
			new Task ( () => 
			{ 

				// Start our main service
				Android.App.Application.Context.StartService(new Intent (Android.App.Application.Context, typeof(BackgroundService)));

				// bind our service (Android goes and finds the running service by type, and puts a reference
				// on the binder to that service)
				// The Intent tells the OS where to find our Service (the Context) and the Type of Service
				// we're looking for (LocationService)
				Intent backgroundServiceIntent = new Intent (Android.App.Application.Context, typeof(BackgroundService));

				// Finally, we can bind to the Service using our Intent and the ServiceConnection we
				// created in a previous step.
				Android.App.Application.Context.BindService (backgroundServiceIntent, _serviceConnection, Bind.AutoCreate);
			} ).Start ();
		}
	}
}

