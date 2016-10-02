using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Sources.Parameterized
{
	public abstract class TypeLocatorBase : FactoryCache<Type, Type>
	{
		readonly ImmutableArray<TypeAdapter> adapters;
		readonly Func<TypeInfo, bool> isAssignable;
		readonly Func<Type[], Type> selector;

		protected TypeLocatorBase( params Type[] types ) : this( types.Select( type => type.Adapt() ).ToImmutableArray() ) {}

		TypeLocatorBase( ImmutableArray<TypeAdapter> adapters )
		{
			this.adapters = adapters;
			isAssignable = IsAssignable;
			selector = From;
		}

		protected override Type Create( Type parameter )
		{
			var result = parameter.Append( parameter.Adapt().GetAllInterfaces() )
								  .AsTypeInfos()
								  .Where( isAssignable )
								  .Select( info => info.GenericTypeArguments )
								  .Select( selector )
								  .FirstOrDefault();
			return result;
		}

		bool IsAssignable( TypeInfo type ) => type.IsGenericType && adapters.IsAssignableFrom( type.GetGenericTypeDefinition() );

		protected abstract Type From( IEnumerable<Type> genericTypeArguments );
	}
}