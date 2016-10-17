using System;
using System.Globalization;
using DragonSpark.Specifications;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class TimestampPathSpecification : SpecificationBase<string>
	{
		public static TimestampPathSpecification Default { get; } = new TimestampPathSpecification();
		TimestampPathSpecification() {}

		public override bool IsSatisfiedBy( string parameter )
		{
			DateTimeOffset item;
			var result = DateTimeOffset.TryParseExact( parameter, Defaults.ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out item );
			return result;
		}
	}
}