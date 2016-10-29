using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Collections.ObjectModel;

namespace DragonSpark.ComponentModel
{
	public sealed class CollectionAttribute : ServicesValueBase
	{
		readonly static Func<Type, Type> Transform = CollectionAlteration.Default.Get;

		public CollectionAttribute( Type elementType = null ) : base( t => Create( elementType ) ) {}

		static ServicesValueProvider Create( Type type = null ) => new ServicesValueProvider( p =>
		{
			var elementType = type ?? p.PropertyType.Adapt().GetEnumerableType();
			var result = elementType.With( Transform );
			return result;
		}, Constructor.Default.Get );

		sealed class CollectionAlteration : AlterationBase<Type>
		{
			public static CollectionAlteration Default { get; } = new CollectionAlteration();
			CollectionAlteration() {}

			public override Type Get( Type parameter ) => typeof(Collection<>).MakeGenericType( parameter );
		}
	}
}