using System;
		
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using BGData.Models;
using BGData.Services;

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

			artList = HandleNoArt(artList);

			table.Source = new TableSource(artList, this);
			Add (table);

			// here we can use a notification to let us know when the app has entered the foreground
			// from the background, and update the text in the View
			// this causes a flicker, but we will use it for demo purposes
			UIApplication.Notifications.ObserveWillEnterForeground (async (sender, args) => {
				apiService = new ApiService();
				artService = new ArtService(apiService);
				artList = await artService.GetPublicArt();

				artList = HandleNoArt(artList);

				if(artList != null)
				{
					InvokeOnMainThread(() =>
					{
						table.Source = new TableSource(artList, this);
						table.ReloadData();
					});
				}
			});
		}

		static List<PublicArt> HandleNoArt(List<PublicArt> artList)
		{
			if(artList == null)
			{
				var tableItems = new List<PublicArt> {
					new PublicArt {
						Title = "Vegetables"
					},
					new PublicArt {
						Title = "Fruits"
					},
					new PublicArt {
						Title = "Flower Buds"
					},
					new PublicArt {
						Title = "Legumes"
					},
					new PublicArt {
						Title = "Bulbs"
					},
					new PublicArt {
						Title = "Tubers"
					}
				};
				App.CurrentOffset = 0;
				return tableItems;
			}

			return artList;
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
