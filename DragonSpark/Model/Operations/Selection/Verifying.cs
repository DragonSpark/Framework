using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

[UsedImplicitly]
public class Verifying<TIn, TOut> : Selecting<TIn, TOut>
{
	protected Verifying(ISelect<TIn, ValueTask<TOut?>> @select) : base(@select.Then().Select(x => x.Verify())) {}
}