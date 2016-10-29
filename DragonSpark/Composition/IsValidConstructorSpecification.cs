using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	sealed class IsValidConstructorSpecification : SpecificationBase<ConstructorInfo>
	{
		readonly Func<Type, bool> validate;

		public IsValidConstructorSpecification( Func<Type, bool> validate )
		{
			this.validate = validate;
		}

		public override bool IsSatisfiedBy( ConstructorInfo parameter )
		{
			var types = parameter.GetParameterTypes();
			var result = !types.Any() || types.All( validate );
			return result;
		}
	}
}