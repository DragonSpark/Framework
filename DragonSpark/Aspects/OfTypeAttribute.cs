using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Aspects
{
	public class OfTypeAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
	{
		readonly ImmutableArray<TypeAdapter> types;

		public OfTypeAttribute( params Type[] types ) : this( types.AsAdapters() ) {}

		OfTypeAttribute( ImmutableArray<TypeAdapter> types ) : this( types, $"The specified type is not of type (or cannot be cast to) {string.Join( " or ", types.Select( type => type.ReferenceType.FullName ) )}" ) {}

		OfTypeAttribute( ImmutableArray<TypeAdapter> types, string errorMessage )
		{
			this.types = types;
			ErrorMessage = errorMessage;
		}

		public Exception ValidateValue( Type value, string locationName, LocationKind locationKind ) => value != null && !types.IsAssignableFrom( value ) ? CreateArgumentException( value, locationName, locationKind ) : null;
	}
}