using System;
using Android.App;
using Android.OS;
using Android.Content;
using BGData.Services;

namespace BGData.Droid
{
	public class ServiceConnectedEventArgs : EventArgs
	{
		public IBinder Binder { get; set; }
	}

	public class BackgroundServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate {};
		public BackgroundServiceBinder Binder { get; set; }

		public BackgroundServiceConnection (BackgroundServiceBinder binder)
		{
			if (binder != null) 
			{
				Binder = binder;
			}
		}

		public void OnServiceConnected(ComponentName name, IBinder service)
		{
			var serviceBinder = service as BackgroundServiceBinder;
			if(serviceBinder != null)
			{
				Binder = serviceBinder;
				Binder.IsBound = true;

				this.ServiceConnected(this, new ServiceConnectedEventArgs () { Binder = service } );
				serviceBinder.Service.StartUpdates();
			}
		}

		public void OnServiceDisconnected(ComponentName name)
		{
			this.Binder.IsBound = false;
		}
	}

	public class BackgroundServiceBinder : Binder
	{
		public BackgroundService Service { get; protected set; }
		public bool IsBound { get; set; }
		public BackgroundServiceBinder(BackgroundService service)
		{
			Service = service;
		}
	}

	[Service]
	public class BackgroundService : Service
	{
		IBinder _binder;

		public override void OnCreate()
		{
			base.OnCreate();
		}

		public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		public override IBinder OnBind(Intent intent)
		{
			_binder = new BackgroundServiceBinder(this);
			return _binder;
		}

		public async void StartUpdates()
		{
			var apiService = new ApiService();
			var artService = new ArtService(apiService);

			// AlarmManager scheduling would be good
			await artService.GetPublicArt(20, artList =>
			{
			});
		}
	}
}

