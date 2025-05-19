using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

sealed class ModifyDispatch<TIn, T> : IStopAware<TIn>
{
	readonly IEdit<TIn, T>  _select;
	readonly Await<Edit<T>> _configure;

	public ModifyDispatch(IEdit<TIn, T> select, Await<Edit<T>> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask Get(Stop<TIn> parameter)
	{
		using var edit = await _select.On(parameter);
		await _configure(edit);
		await edit.Off();
	}
}