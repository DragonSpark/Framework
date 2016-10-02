using System;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;

namespace DragonSpark.Aspects
{
	public sealed class AssignedAttribute : LocationContractAttribute, ILocationValidationAspect<object>
	{
		protected override string GetErrorMessage() => "The parameter '{0}' must be assigned (non-default value).";

		public Exception ValidateValue( object value, string locationName, LocationKind locationKind ) => 
			SpecialValues.DefaultOrEmpty( value.GetType() ).Equals( value ) ? CreateArgumentException( value, locationName, locationKind ) : null;
	}
}