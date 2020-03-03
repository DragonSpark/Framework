using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// ATTRIBUTION: https://www.nuget.org/packages/Radzen.Blazor/
	/// </summary>
	public sealed class UniqueIdentifier : ISelect<Guid, string>
	{
		public static UniqueIdentifier Default { get; } = new UniqueIdentifier();

		UniqueIdentifier() {}

		public string Get(Guid parameter) => Convert.ToBase64String(parameter.ToByteArray())
		                                            .Replace("/", "-")
		                                            .Replace("+", "-")
		                                            .Substring(0, 10);
	}
}
