using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.ComponentModel
{
	public class CollectionAttribute : ActivateAttributeBase
	{
		public CollectionAttribute( Type elementType = null, string name = null ) : base( t => Create( elementType, name ) ) {}

		static ActivatedValueProvider Create( Type type, string name ) => new ActivatedValueProvider( p =>
		{
			var elementType = type ?? p.PropertyType.Adapt().GetInnerType();
			var result = new ActivateParameter( elementType.With( Transformer.Instance.Create ), name );
			return result;
		} );

		public class Transformer : TransformerBase<Type>
		{
			public static Transformer Instance { get; } = new Transformer();

			protected override Type CreateItem( Type parameter ) => typeof(Collection<>).MakeGenericType( parameter );
		}
	}
}