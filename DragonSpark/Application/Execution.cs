using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public static class Execution
	{
		public static IAssignableSource<ISource> Context { get; } = new SuppliedSource<ISource>( ExecutionContext.Default );

		public static object Current() => Support.Get();

		static class Support
		{
			readonly public static Func<object> Get = Context.Delegate();
		}
	}
}