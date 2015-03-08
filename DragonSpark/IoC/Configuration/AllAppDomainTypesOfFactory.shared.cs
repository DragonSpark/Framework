using System;
using System.Collections.ObjectModel;
using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class AllAppDomainTypesOfFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var targetType = ElementType ?? type.GetCollectionElementType() ?? type;

			var result = new AppDomainTypeResolver( container ).Resolve( targetType, ExcludeTypes.Union( ExcludeBuildType ? targetType.ToEnumerable() : Enumerable.Empty<Type>() ).ToArray() );
			return result;
		}

		public bool ExcludeBuildType { get; set; }

		public Collection<Type> ExcludeTypes
		{
			get { return excludeTypes; }
		}	readonly Collection<Type> excludeTypes = new Collection<Type>();

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ElementType { get; set; }
	}
}