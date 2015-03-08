using System;
using DragonSpark.Configuration;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class NamedTypeBuildKey : IInstanceSource<Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey>, IFactory
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type BuildType { get; set; }

		public string BuildName { get; set; }

		public Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey Instance
		{
			get
			{
				var result = new Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey( BuildType, BuildName );
				return result;
			}
		}

		public object Create( Type resultType, object source = null )
		{
			var result = source.AsTo<IUnityContainer, object>( x => this.Create( x, resultType ) );
			return result;
		}
	}
}