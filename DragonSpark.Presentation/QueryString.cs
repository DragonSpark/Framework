using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace DragonSpark.Presentation
{
	sealed class QueryString : ReferenceValueStore<NavigationManager, Dictionary<string, StringValues>>
	{
		public static QueryString Default { get; } = new QueryString();

		QueryString() : base(x => QueryHelpers.ParseQuery(x.ToAbsoluteUri(x.Uri).Query)) {}
	}
}
