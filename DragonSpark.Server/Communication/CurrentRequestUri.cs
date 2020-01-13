using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;

namespace DragonSpark.Server.Communication
{
	sealed class CurrentRequestUri : ReferenceValueTable<HttpRequest, Uri>
	{
		public static CurrentRequestUri Default { get; } = new CurrentRequestUri();

		CurrentRequestUri() : base(x => new Uri(x.GetDisplayUrl())) {}
	}
}