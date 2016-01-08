using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.ComponentModel
{
	public class CollectionAttribute : ActivateAttribute
	{
		public CollectionAttribute( Type elementType = null, string name = null ) : base( () => new ActivatedValueProvider(
			p => new ActivateParameter( elementType.With( Transformer.Instance.Create ) ?? p.PropertyType.Adapt().GetInnerType().With( Transformer.Instance.Create ), name ),
			new Factory<object>().Create
		) ) {}

		public class Transformer : TransformerBase<Type>
		{
			public static Transformer Instance { get; } = new Transformer();

			protected override Type CreateItem( Type parameter ) => typeof(Collection<>).MakeGenericType( parameter );
		}
	}
}