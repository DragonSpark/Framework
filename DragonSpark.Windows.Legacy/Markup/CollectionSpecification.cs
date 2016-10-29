using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System;
using System.Collections;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class CollectionSpecification : SpecificationBase<IServiceProvider>
	{
		public static CollectionSpecification Default { get; } = new CollectionSpecification();

		public override bool IsSatisfiedBy( IServiceProvider parameter ) => 
			parameter.Get<IProvideValueTarget>().TargetObject.With( o => o is IList && o.Adapt().GetEnumerableType() != null );
	}
}