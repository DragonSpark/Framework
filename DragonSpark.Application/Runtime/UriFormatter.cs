using DragonSpark.Text;
using System;

namespace DragonSpark.Application.Runtime;

public sealed class UriFormatter : Formatter<Uri>
{
	public static UriFormatter Default { get; } = new();

	UriFormatter() : base(x => x.ToString()) {}
}