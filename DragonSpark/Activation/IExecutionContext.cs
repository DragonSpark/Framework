using DragonSpark.Runtime.Values;

namespace DragonSpark.Activation
{
	public interface IExecutionContext : IWritableValue<object>
	{}

	public static class Execution
	{
		static Execution()
		{
			Initialize( DefaultExecutionContext.Instance );
		}

		public static void Initialize( IExecutionContext current )
		{
			Context = current;
		}	static IExecutionContext Context { get; set; }

		public static object Current => Context.Item;

		public static void Assign( object current )
		{
			Context.Assign( current );
		}
	}

	class DefaultExecutionContext : FixedValue<object>, IExecutionContext
	{
		public static DefaultExecutionContext Instance { get; } = new DefaultExecutionContext();

		readonly object defaultContext = new object();

		public override object Item => base.Item ?? defaultContext;
	}
}