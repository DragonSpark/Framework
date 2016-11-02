using DragonSpark.Sources;

namespace DragonSpark.Application
{
	public class Execution : SuppliedSource<ISource>
	{
		public static Execution Default { get; } = new Execution();
		Execution() : base( ExecutionContext.Default ) {}

		/*public static IAssignableSource<ISource> Context { get; } = new SuppliedSource<ISource>( ExecutionContext.Default );

		public static object Current() => Support.Get();

		static class Support
		{
			readonly public static Func<object> Get = Context.GetValue;
		}*/
	}
}