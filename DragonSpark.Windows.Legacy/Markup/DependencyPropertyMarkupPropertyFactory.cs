using DragonSpark.Sources.Parameterized;
using System;
using System.Windows;

namespace DragonSpark.Windows.Legacy.Markup
{
	public sealed class DependencyPropertyMarkupPropertyFactory : MarkupPropertyFactoryBase<DependencyObject, DependencyProperty>
	{
		public static IParameterizedSource<IServiceProvider, IMarkupProperty> Default { get; } = new DependencyPropertyMarkupPropertyFactory().Apply( Specification<DependencyObject, DependencyProperty>.Default );
		DependencyPropertyMarkupPropertyFactory() {}

		protected override IMarkupProperty Create( DependencyObject targetObject, DependencyProperty targetProperty ) => new DependencyPropertyMarkupProperty( targetObject, targetProperty );
	}
}