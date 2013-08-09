using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class DependencyValue : InjectionParameterValue
	{
		public string BuildName { get; set; }

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type DependencyType { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new ResolvedParameter( DependencyType, BuildName );
			return result;
		}
	}
}