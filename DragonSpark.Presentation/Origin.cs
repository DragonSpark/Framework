using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System;
using System.Net;

namespace DragonSpark.Presentation
{
	public sealed class Origin : IResult<string>
	{
		public static implicit operator string(Origin instance) => instance.Get();

		readonly NavigationManager _manager;

		public Origin(NavigationManager manager) => _manager = manager;

		public string Get() => WebUtility.UrlEncode(new Uri(_manager.Uri).PathAndQuery);
	}
}