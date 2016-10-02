using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public sealed class CollectionMarkupPropertyFactory : MarkupPropertyFactoryBase
	{
		public static IParameterizedSource<IServiceProvider, IMarkupProperty> Default { get; } = new CollectionMarkupPropertyFactory().Apply( CollectionSpecification.Default );
		CollectionMarkupPropertyFactory() : this( PropertyReferenceFactory.Default.ToSourceDelegate() ) {}

		readonly Func<IServiceProvider, PropertyReference> propertyFactory;

		public CollectionMarkupPropertyFactory( Func<IServiceProvider, PropertyReference> propertyFactory )
		{
			this.propertyFactory = propertyFactory;
		}

		public override IMarkupProperty Get( IServiceProvider parameter )
		{
			var reference = propertyFactory( parameter );
			var result = reference.IsAssigned() ? new CollectionMarkupProperty( (IList)parameter.Get<IProvideValueTarget>().TargetObject, reference ) : null;
			return result;
		}
	}
}