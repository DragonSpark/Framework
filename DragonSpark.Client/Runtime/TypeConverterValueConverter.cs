using System;

namespace DragonSpark.Runtime
{
	public partial class TypeConverterValueConverter
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType")]
		static IDisposable ResolveConverter( Type targetType )
		{
			return new Disposable();
		}
	}
}
