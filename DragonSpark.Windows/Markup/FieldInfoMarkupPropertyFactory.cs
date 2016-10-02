using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public sealed class FieldInfoMarkupPropertyFactory : MarkupPropertyFactory<FieldInfo>
	{
		public static IParameterizedSource<IServiceProvider, IMarkupProperty> Default { get; } = new FieldInfoMarkupPropertyFactory().Apply( Specification<object, FieldInfo>.Default );
		FieldInfoMarkupPropertyFactory() {}

		protected override IMarkupProperty Create( object targetObject, FieldInfo targetProperty ) => new ClrFieldMarkupProperty( targetObject, targetProperty );
	}
}