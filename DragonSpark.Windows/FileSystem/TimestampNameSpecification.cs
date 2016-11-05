using DragonSpark.Specifications;
using System;
using System.Globalization;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class TimestampNameSpecification : SpecificationBase<string>
	{
		public static TimestampNameSpecification Default { get; } = new TimestampNameSpecification();
		TimestampNameSpecification() {}

		public override bool IsSatisfiedBy( string parameter )
		{
			DateTimeOffset item;
			var result = DateTimeOffset.TryParseExact( parameter, Defaults.ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out item );
			return result;
		}
	}
}