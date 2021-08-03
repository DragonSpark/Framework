using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using System;

namespace DragonSpark.Composition.Compose
{
	public class LocateComponent<TIn, TOut> : Select<TIn, TOut>
	{
		public LocateComponent(Func<TIn, IComponentType> select) : this(select.Start()) {}

		public LocateComponent(Selector<TIn, IComponentType> select)
			: base(select.Select(x => x.Get(A.Type<TOut>()))
			             .Select(Start.A.Selection.Of.System.Type.By.Self.Then()
			                          .Activate<TOut>()
			                          .Ensure.Input.IsAssigned.Otherwise.Throw(LocateGuardMessage.Default)
			                          .Ensure.Output.IsAssigned.Otherwise.Throw(LocateComponentMessage<TOut>.Default)
			                    )) {}
	}
}