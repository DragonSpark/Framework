using System.Collections.Generic;
using System.Linq;
using DragonSpark;
using DragonSpark.Runtime;

namespace DragonSpark.Objects.Synchronization
{
	public static class SimilarPropertyHelper
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NonPublic", Justification = "It is what it is." )]
		public static IEnumerable<string> ResolveSimilarProperties( SynchronizationKey key, bool includeNonPublicProperties )
		{
			var flags = includeNonPublicProperties ? DragonSparkBindingOptions.AllProperties : DragonSparkBindingOptions.PublicProperties;
			var result =
				from 
					property in key.First.GetProperties( flags )
				where property.CanRead 
				let info = key.Second.GetProperty( property.Name, flags )
				where
					info != null && info.Name != "Item" && info.CanRead && info.CanWrite && ConversionHelper.IsConvertible( property.PropertyType, info.PropertyType ) 
				select info.Name;
			return result;
		}
	}
}