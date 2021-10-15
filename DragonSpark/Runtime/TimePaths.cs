using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime;

public sealed class TimePaths : ISelect<DateTimeOffset, string>
{
	public static TimePaths Default { get; } = new TimePaths();

	TimePaths() : this("yyyy-MM-dd-HH-mm-ss") {}

	readonly string _format;

	public TimePaths(string format) => _format = format;

	public string Get(DateTimeOffset parameter) => parameter.ToString(_format);
}