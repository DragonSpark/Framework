using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Navigation;

sealed class QueryString : Select<NavigationManager, Dictionary<string, StringValues>>
{
	public static QueryString Default { get; } = new QueryString();

	QueryString() : base(x => QueryHelpers.ParseQuery(x.ToAbsoluteUri(x.Uri).Query)) {}
}