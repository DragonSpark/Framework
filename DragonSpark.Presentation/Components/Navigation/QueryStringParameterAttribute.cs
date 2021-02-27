using System;

namespace DragonSpark.Presentation.Components.Navigation
{
	/// <summary>
	/// ATTRIBUTION: https://www.meziantou.net/bind-parameters-from-the-query-string-in-blazor.htm
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class QueryStringParameterAttribute : Attribute
	{
		public QueryStringParameterAttribute(string? name = null) => Name = name;

		public string? Name { get; }
	}
}