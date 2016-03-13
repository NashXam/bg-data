using System;
using System.Net.Http;
using Refit;
using ModernHttpClient;

namespace BGData.Services
{
	public interface IApiService
	{
		IArtApi ArtApi { get; }
	}
	
}
