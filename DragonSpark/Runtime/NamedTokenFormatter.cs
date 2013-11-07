using System;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	public sealed class NamedTokenFormatter : IFormatProvider, ICustomFormatter
	{
		public static NamedTokenFormatter Instance
		{
			get { return InstanceField; }
		}	static readonly NamedTokenFormatter InstanceField = new NamedTokenFormatter();

		object IFormatProvider.GetFormat( Type formatType )
		{
			return formatType == typeof(ICustomFormatter) ? this : null;
		}

		string ICustomFormatter.Format( string format, object arg, IFormatProvider formatProvider )
		{
			return Format( arg, format );
		}

		static string Format( object arg, string format )
		{
			var result = ValueResolver.Resolve( arg, format ).Transform( x => x.ToString() );
			return result;
		}
	}
}
