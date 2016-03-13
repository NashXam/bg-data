using System;
using System.Net.Http;
using Refit;
using ModernHttpClient;

namespace BGData.Services
{
	public class ApiService : IApiService
	{
		public const string ApiBaseAddress = "https://data.nashville.gov/resource";
		readonly Lazy<IArtApi> _artApi;

		public ApiService (string apiBaseAddress = null)
		{
			Func<HttpMessageHandler, IArtApi> createClient = messageHandler =>
			{
				var client = new HttpClient(messageHandler)
				{
					BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
				};

				return RestService.For<IArtApi>(client);
			};

			_artApi = new Lazy<IArtApi> (() => createClient(new NativeMessageHandler()));
		}

		public IArtApi ArtApi
		{
			get { return _artApi.Value; }
		}
	}
}

