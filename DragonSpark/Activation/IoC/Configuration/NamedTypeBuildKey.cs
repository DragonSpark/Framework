using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.IoC.Configuration
{
	public class NamedTypeBuildKey : ISingleton<Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey>, IFactory
	{
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

		public object Create( Type resultType, object parameter = null )
		{
			var result = parameter.AsTo<IUnityContainer, object>( x => this.Create( x, resultType ) );
			return result;
		}
	}
}