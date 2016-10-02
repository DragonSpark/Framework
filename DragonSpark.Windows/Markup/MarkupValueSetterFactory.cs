using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Windows.Markup
{
	public sealed class MarkupValueSetterFactory : CompositeFactory<IServiceProvider, IMarkupProperty>, IMarkupPropertyFactory
	{
		public static MarkupValueSetterFactory Default { get; } = new MarkupValueSetterFactory();
		MarkupValueSetterFactory() : base( 
			DependencyPropertyMarkupPropertyFactory.Default, 
			CollectionMarkupPropertyFactory.Default, 
			PropertyInfoMarkupPropertyFactory.Default, 
			FieldInfoMarkupPropertyFactory.Default ) {}
	}
}