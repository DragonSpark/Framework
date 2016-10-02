using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public class SpecifiedSingletonHostSpecification : SingletonSpecification
	{
		readonly Type host;
		public SpecifiedSingletonHostSpecification( Type host = null, params string[] candidates ) : base( candidates )
		{
			this.host = host;
		}

		public override bool IsSatisfiedBy( PropertyInfo parameter ) => ( host == null || parameter.DeclaringType == host ) && base.IsSatisfiedBy( parameter );
	}
}