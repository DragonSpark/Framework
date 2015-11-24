using DragonSpark.Extensions;
using System;

namespace DragonSpark.Runtime
{
	public interface IAmbientKey
	{
		bool Handles( IAmbientRequest request );
	}

	public class AmbientKey<T> : AmbientKey
	{
		public AmbientKey( ISpecification specification ) : base( typeof(T), specification )
		{}
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