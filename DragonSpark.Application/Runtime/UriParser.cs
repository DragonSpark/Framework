using DragonSpark.Text;
using System;

namespace DragonSpark.Application.Runtime;

public sealed class UriParser : Parser<Uri>
{
	public static UriParser Default { get; } = new();

	UriParser() : base(x => new Uri(x)) {}
}