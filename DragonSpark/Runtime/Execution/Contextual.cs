using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Execution
{
	public class Contextual<T> : Result<T>
	{
		readonly static bool Attach = IsAssignableFrom<IDisposable>.Default.Get(A.Metadata<T>());

		Contextual(ISelect<object, T> select, bool attach)
			: base((attach ? select.Then().Configure(Implementations.Resources).Get() : select)
			       .Stores()
			       .Reference()
			       .In(ExecutionContext.Default)) {}

		public Contextual(Func<T> source) : this(Start.A.Selection.Of.Any.By.Calling(source), Attach) {}
	}
}