using System;
using Newtonsoft.Json;

namespace BGData.Models
{
	public class PublicArt
	{
		public string Title { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		public string Location { get; set; }

		public override string ToString()
		{
			return Title;
		}
	}
}

