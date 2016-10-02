using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources
{
	public class AmbientStackCommand<T> : StackCommand<T>
	{
		public AmbientStackCommand() : this( AmbientStack<T>.Default ) {}

		public AmbientStackCommand( AmbientStack<T> stack ) : base( stack.Get() ) {}
	}
}