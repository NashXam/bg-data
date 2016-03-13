using System;
using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;
using BGData.Models;

namespace BGData.Services
{
	[Headers("Accept: application/json")]
	public interface IArtApi
	{
		[Get ( "/dqkw-tj5j.json?$limit={limit}&$offset={offset}" ) ]
		Task<List<PublicArt>> GetPagedPublicArt(int limit, int offset);
	}
}

