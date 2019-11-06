using System;

namespace DragonSpark.Text.Formatting
{
	public class DateTimeOffsetFormatter : IFormatter<DateTimeOffset>
	{
		readonly string _format;

		public DateTimeOffsetFormatter(string format) => _format = format;

		public string Get(DateTimeOffset parameter) => parameter.ToString(_format);
	}
}