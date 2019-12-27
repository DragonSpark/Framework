using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Execution
{
	public class Contextual<T> : FixedSelection<object, T>
	{
		readonly static bool Attach = Is.AssignableFrom<IDisposable>().Get(A.Metadata<T>());

		public Contextual(Func<T> source) : this(Start.A.Selection.Of.Any.By.Calling(source), Attach) {}

		Contextual(ISelect<object, T> select, bool attach)
			: base((attach
				        ? select.Then().Configure(Implementations.Resources).Get()
				        : select)
			       .Stores()
			       .Reference(),
			       ExecutionContext.Default) {}
	}
}