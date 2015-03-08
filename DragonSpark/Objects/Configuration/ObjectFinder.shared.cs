using System;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Configuration
{
	public class ObjectFinder : ObjectFinderBase
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ObjectFinderType { get; set; }

		protected override ILocator Create()
		{
			var result = Activator.CreateInstance( ObjectFinderType ).To<ILocator>();
			return result;
		}
	}
}