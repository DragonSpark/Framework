using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public sealed class PropertyInfoMarkupPropertyFactory : MarkupPropertyFactory<PropertyInfo>
	{
		public static IParameterizedSource<IServiceProvider, IMarkupProperty> Default { get; } = new PropertyInfoMarkupPropertyFactory().Apply( Specification<object, PropertyInfo>.Default );
		PropertyInfoMarkupPropertyFactory() {}

		protected override IMarkupProperty Create( object targetObject, PropertyInfo targetProperty ) => new ClrPropertyMarkupProperty( targetObject, targetProperty );
	}
}