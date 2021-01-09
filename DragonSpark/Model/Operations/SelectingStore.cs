using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class SelectingStore<TIn, TOut> : Selecting<TIn, TOut> where TIn : class
	{
		public SelectingStore(ISelect<TIn, ValueTask<TOut>> select)
			: base(select.Then()
			             .Allocate()
			             .Stores()
			             .New()
			             .Select(x => x.ToOperation())) {}
	}
}