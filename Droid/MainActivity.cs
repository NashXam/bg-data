using Android.App;
using Android.Widget;
using Android.OS;
using BGData.Services;
using BGData.Models;
using Akavache;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System;

namespace BGData.Droid
{
	[Activity(Label = "BG Data", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			#if DEBUG
			BlobCache.LocalMachine.InvalidateAll();
			BlobCache.LocalMachine.Flush();
			#endif

			var apiService = new ApiService();
			var artService = new ArtService(apiService);
			
			button.Click += async delegate
			{
				button.Text = string.Format("{0} clicks!", count++);

				await artService.GetPublicArt(publicArt =>
				RunOnUiThread(() =>
				{
						var titles = publicArt.Select(art => art.Title);
					button.Text = Newtonsoft.Json.JsonConvert.SerializeObject(titles);
				}));
			};
		}
	}
}


