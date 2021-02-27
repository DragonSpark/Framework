using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Navigation
{
	public interface IQueryString : ISelect<NavigationManager, Dictionary<string, StringValues>> {}

	sealed class QueryString : ReferenceValueStore<NavigationManager, Dictionary<string, StringValues>>, IQueryString
	{
		public static QueryString Default { get; } = new QueryString();

		QueryString() : base(x => QueryHelpers.ParseQuery(x.ToAbsoluteUri(x.Uri).Query)) {}
	}
}