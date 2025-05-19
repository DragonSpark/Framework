using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Modifying<TIn, T> : StopAdaptor<TIn, T>
{
	public Modifying(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Off) {}

	public Modifying(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	public Modifying(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Off) {}

	protected Modifying(IEdit<TIn, T> select, Await<Edit<T>> configure)
		: base(new ModifyingDispatch<TIn, T>(select, configure)) {}
}