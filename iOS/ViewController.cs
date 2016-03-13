using System;
		
using System.Collections.Generic;
using BGData.Models;
using BGData.Services;
using CoreGraphics;
using UIKit;

namespace BGData.iOS
{
	public partial class ViewController : UIViewController
	{
		int count = 1;
		UITableView table;

		public ViewController(IntPtr handle) : base(handle)
		{		
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			Button.AccessibilityIdentifier = "myButton";
			Button.TouchUpInside += delegate
			{
				var title = string.Format("{0} clicks!", count++);
				Button.SetTitle(title, UIControlState.Normal);
			};

			var apiService = new ApiService();
			var artService = new ArtService(apiService);
			var artList = await artService.GetPublicArt();

			table = new UITableView(new CGRect(0,20,View.Bounds.Width, View.Bounds.Height - 20)); // defaults to Plain style
			table.AutoresizingMask = UIViewAutoresizing.All;

			table.Source = BuildTableSource(artList);
			Add (table);

			// here we can use a notification to let us know when the app has entered the foreground
			// from the background, and update the text in the View
			// this causes a flicker, but we will use it for demo purposes
			UIApplication.Notifications.ObserveWillEnterForeground (async (sender, args) => {
				apiService = new ApiService();
				artService = new ArtService(apiService);
				artList = await artService.GetPublicArt();

				InvokeOnMainThread(() =>
				{
					table.Source = BuildTableSource(artList);
					table.ReloadData();
				});
			});
		}

		UITableViewSource BuildTableSource(List<PublicArt> artList)
		{
			UITableViewSource tableSource = null;
			if(artList != null)
			{
				tableSource = new TableSource<PublicArt>(artList, this);
			}
			else
			{
				tableSource = new TableSource<string>(new List<string> {
												"Sorry, no data available."
											}, this);
			}
			return tableSource;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void DidReceiveMemoryWarning()
		{		
			base.DidReceiveMemoryWarning();		
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
