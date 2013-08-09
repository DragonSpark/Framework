using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "ExcludeTypes" )]
	public class AllAppDomainTypesOfParameter : InjectionParameterValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ElementType { get; set; }

		public bool ExcludeBuildType { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new IoC.AllAppDomainTypesOfParameter( ElementType, ExcludeTypes.Union( ExcludeBuildType ? targetType.ToEnumerable() : Enumerable.Empty<Type>() ).ToArray() );
			return result;
		}

		public Collection<Type> ExcludeTypes
		{
			get { return excludeTypes; }
		}	readonly Collection<Type> excludeTypes =  new Collection<Type>();
	}
}