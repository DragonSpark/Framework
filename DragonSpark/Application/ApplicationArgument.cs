using DragonSpark.Model.Results;

namespace DragonSpark.Application
{
	public class ApplicationArgument<T> : Instance<T>
	{
		public ApplicationArgument(T instance) : base(instance) {}
	}
}