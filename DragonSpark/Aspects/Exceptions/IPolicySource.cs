using DragonSpark.Sources;
using Polly;

namespace DragonSpark.Aspects.Exceptions
{
	public interface IPolicySource : ISource<Policy> {}
}