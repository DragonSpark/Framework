using DragonSpark.Runtime.Specifications;

namespace DragonSpark.Runtime.Values
{
	public class AmbientKey<T> : AmbientKey
	{
		public AmbientKey( ISpecification specification ) : base( typeof(T), specification )
		{}
	}
}