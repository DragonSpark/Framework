using DragonSpark.Text;
using System;

namespace DragonSpark.Presentation.Components;

/// <summary>
/// ATTRIBUTION: https://www.nuget.org/packages/Radzen.Blazor/
/// </summary>
public sealed class UniqueIdentifier : IFormatter<Guid>
{
	public static UniqueIdentifier Default { get; } = new();

	UniqueIdentifier() {}

	public string Get(Guid parameter) => Convert.ToBase64String(parameter.ToByteArray())
	                                            .Replace('/', '-')
	                                            .Replace('+', '-')
	                                            .Substring(0, 10);
}