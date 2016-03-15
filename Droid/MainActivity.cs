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
using Android.Content;

namespace BGData.Droid
{
	/// <summary>
	/// Consider this Android setting when using background data fetching:  
	/// 
	/// http://developer.android.com/reference/android/net/ConnectivityManager.html#getBackgroundDataSetting()
	/// </summary>

	[Activity(Label = "BG Data", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
		ListView list;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			list = FindViewById<ListView>(Resource.Id.list);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			var apiService = new ApiService();
			var artService = new ArtService(apiService);
			var artList = await artService.GetPublicArt();

			list.Adapter = BuildAdapter(artList);

			button.Click += async delegate
			{
				button.Text = string.Format("{0} clicks!", count++);
			};

			DroidApp.StartBackgroundService();
		}

		protected override async void OnResume()
		{
			base.OnResume();

			var apiService = new ApiService();
			var artService = new ArtService(apiService);
			var artList = await artService.GetPublicArt();

			list.Adapter = BuildAdapter(artList);
		}

		private ArrayAdapter BuildAdapter(List<PublicArt> artList)
		{
			ArrayAdapter adapter = null;
			if(artList != null)
			{
				adapter = new ArrayAdapter<PublicArt>(this, Android.Resource.Layout.SimpleListItem1, artList);
			}
			else
			{
				adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new List<string> {"Sorry, no data available."});
			}
			return adapter;
		}
	}
}


