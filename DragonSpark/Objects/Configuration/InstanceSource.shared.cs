using System;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Configuration
{
	public class InstanceSource : InstanceSourceBase
	{
		public InstanceSource()
		{
			SourceType = typeof(object);
		}

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type SourceType { get; set; }

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ResultType { get; set; }

		protected override IFactory Create()
		{
			var type = typeof(Factory<,>).MakeGenericType( SourceType, ResultType );
			var result = Activator.CreateInstance( type ).To<IFactory>();
			return result;
		}
	}
}