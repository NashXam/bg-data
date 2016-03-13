using System;
using System.Collections.Generic;
using BGData.Models;

namespace BGData
{
	public static class App
	{
		public static int CurrentOffset { get; set; } = 0;

		public static List<PublicArt> HandleNoArt(List<PublicArt> artList)
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
	}
}

