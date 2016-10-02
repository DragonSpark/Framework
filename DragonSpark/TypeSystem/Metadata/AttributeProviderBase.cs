using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;

namespace DragonSpark.TypeSystem.Metadata
{
	public abstract class AttributeProviderBase : IAttributeProvider
	{
		readonly ISpecification<Type> defined;
		readonly IParameterizedSource<Type, IEnumerable<Attribute>> factory;

		protected AttributeProviderBase()
		{
			defined = new DelegatedSpecification<Type>( Contains ).ToCachedSpecification();
			factory = new Cache<Type, IEnumerable<Attribute>>( GetAttributes );
		}

		public abstract bool Contains( Type attributeType );

		public abstract IEnumerable<Attribute> GetAttributes( Type attributeType );

		IEnumerable<Attribute> IAttributeProvider.GetAttributes( Type attributeType ) => defined.IsSatisfiedBy( attributeType ) ? factory.Get( attributeType ) : Items<Attribute>.Default;
	}
}