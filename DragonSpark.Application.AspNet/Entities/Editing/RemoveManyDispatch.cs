using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

sealed class RemoveManyDispatch<TIn, TOut> : IStopAware<TIn> where TOut : class
{
	readonly IEditMany<TIn, TOut> _edit;

	public RemoveManyDispatch(IEditMany<TIn, TOut> edit) => _edit = edit;

	public async ValueTask Get(Stop<TIn> parameter)
	{
		using var edit = await _edit.Off(parameter);
		foreach (var remove in edit.Subject)
		{
			edit.Remove(remove);
		}

		await edit.Off();
	}
}