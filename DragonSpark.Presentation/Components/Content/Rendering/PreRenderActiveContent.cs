using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class PreRenderActiveContent<T> : Validated<ValueTask<T?>>, IActiveContent<T>
	{
		public PreRenderActiveContent(ICondition condition, IActiveContent<T> memory, IActiveContent<T> previous)
			: base(condition, memory, previous) {}
	}
}