using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using System;
using System.Linq;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Aspects
{
	public class OfFactoryType : OfTypeAttribute
	{
		public OfFactoryType() : base( typeof(IFactory), typeof(IFactoryWithParameter) ) {}
	}

	public class OfTypeAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
	{
		readonly Type[] types;

		public OfTypeAttribute( params Type[] types )
		{
			this.types = types;
		}

		protected override string GetErrorMessage()
		{
			var names = string.Join( " or ", types.Select( type => type.FullName ) );
			return /*ContractLocalizedTextProvider.Current.GetMessage( nameof(OfTypeAttribute) )*/ $"The specified type is not of type (or cannot be cast to) {names}";
		}

		public Exception ValidateValue( Type value, string locationName, LocationKind locationKind )
			=> types.Any( type => type.Adapt().IsAssignableFrom( value ) ) ? null : CreateException( value, locationName, locationKind, LocationValidationContext.SuccessPostcondition );

		Exception CreateException( object value, string locationName, LocationKind locationKind, LocationValidationContext context )
		{
			var factory = context == LocationValidationContext.SuccessPostcondition ? (Func<object, string, LocationKind, Exception>)CreatePostconditionFailedException : CreateArgumentException;
			var result = factory( value, locationName, locationKind );
			return result;
		}
	}
}