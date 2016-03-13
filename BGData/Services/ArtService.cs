using System;
using BGData.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Akavache;
using System.Reactive.Linq;
using System.Linq;
using System.Reactive;
using ReactiveUI;

namespace BGData.Services
{
	public class ArtService : ReactiveObject
	{
		private readonly IApiService _apiService;
		private readonly IBlobCache _cache;
		private const int _currentLimit = 5;

		public ArtService(IApiService apiService)
		{
			_apiService = apiService;
			_cache = BlobCache.LocalMachine;
		}

		public async Task<List<PublicArt>> GetPublicArt()
		{
			List<PublicArt> cachedPublicArt = null;
			var keys = await _cache.GetAllKeys();

			if(keys.Contains("publicart"))
			{
				cachedPublicArt = await _cache.GetObject<List<PublicArt>>("publicart");
			}

			return cachedPublicArt;
		}

		public async Task GetPublicArt(int pageSize, Action<List<PublicArt>> callback)
		{
			Func<DateTimeOffset, bool> iCanHazMoarData = offset => {
				TimeSpan elapsed = DateTimeOffset.Now - offset;
				return elapsed > new TimeSpan (hours: 0, minutes: 0, seconds: 10);
			};
				
			var publicArt = new List<PublicArt>();

			var dataExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(2));
			var cachedPublicArt = _cache.GetAndFetchLatest("publicart", () => GetRemotePublicArtAsync(pageSize), iCanHazMoarData, dataExpiration);

			cachedPublicArt.Subscribe(art => 
			{
				publicArt = art;
				callback(publicArt);
			});
		}

		private async Task<List<PublicArt>> GetRemotePublicArtAsync(int pageSize)
		{
			List<PublicArt> publicArt = null, localArt = new List<PublicArt>();
			pageSize = pageSize > 0 ? pageSize : _currentLimit;
			publicArt = await _apiService.ArtApi.GetPagedPublicArt(pageSize, App.CurrentOffset);
			App.CurrentOffset += pageSize;

			var keys = await _cache.GetAllKeys();

			if(keys.Contains("publicart"))
			{
				var cachedArt = await _cache.GetObject<List<PublicArt>>("publicart");
				localArt = cachedArt;
			}

			localArt.AddRange(publicArt);
			return localArt;
		}
	}
}

