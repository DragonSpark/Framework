using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using System;

namespace DragonSpark.Aspects
{
	public class OfTypeAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
	{
		readonly Type type;

		public OfTypeAttribute( Type type )
		{
			this.type = type;
		}

		protected override string GetErrorMessage()
		{
			return /*ContractLocalizedTextProvider.Current.GetMessage( nameof(OfTypeAttribute) )*/ $"The specified type is not of type (or cannot be cast to) {type.FullName}";
		}

		public Exception ValidateValue( Type value, string locationName, LocationKind locationKind )
		{
			var result = !type.Adapt().IsAssignableFrom( value ) ? CreateException( value, locationName, locationKind, LocationValidationContext.SuccessPostcondition ) : null;
			return result;
		}

		Exception CreateException( object value, string locationName, LocationKind locationKind, LocationValidationContext context )
		{
			var factory = context == LocationValidationContext.SuccessPostcondition ? (Func<object, string, LocationKind, Exception>)CreatePostconditionFailedException : CreateArgumentException;
			var result = factory( value, locationName, locationKind );
			return result;
		}
	}
}