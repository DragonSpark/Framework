using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	public interface IAspectHub
	{
		void Register( IAspect aspect );
	}
}