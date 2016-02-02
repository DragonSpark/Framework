using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using System;
using DragonSpark.Runtime;

namespace DragonSpark.ComponentModel
{
	public class CollectionAttribute : ActivateAttributeBase
	{
		public CollectionAttribute( Type elementType = null, string name = null ) : base( t => Create( elementType, name ) ) {}

		static ActivatedValueProvider Create( Type type, string name ) => new ActivatedValueProvider( p =>
		{
			var elementType = type ?? p.PropertyType.Adapt().GetEnumerableType();
			var result = new ActivateParameter( elementType.With( Transformer.Instance.Create ), name );
			return result;
		} );

		class Transformer : TransformerBase<Type>
		{
			public static Transformer Instance { get; } = new Transformer();

			protected override Type CreateItem( Type parameter ) => typeof(Collection<>).MakeGenericType( parameter );
		}
	}
}