using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;

namespace DragonSpark.Runtime.Values
{
	public interface IAmbientKey
	{
		bool Handles( IAmbientRequest request );
	}

	public class AmbientKey : IAmbientKey
	{
		public Type TargetType { get; set; }
		readonly ISpecification specification;

		public AmbientKey( Type targetType, ISpecification specification )
		{
			TargetType = targetType;
			this.specification = specification;
		}

		public bool Handles( IAmbientRequest request )
		{
			var result = request.RequestedType.Extend().IsAssignableFrom( TargetType ) && specification.IsSatisfiedBy( request.Context );
			return result;
		}
	}
}