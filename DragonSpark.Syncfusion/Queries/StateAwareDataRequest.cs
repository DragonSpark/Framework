using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Environment.Browser;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class StateAwareDataRequest : IDataRequest
{
	readonly IDataRequest _previous;
	readonly IOperation   _remove;
	readonly Switch       _active;

	public StateAwareDataRequest(IDataRequest previous, IClientVariable<string> state, Switch active)
		: this(previous, state.Remove, active) {}

	public StateAwareDataRequest(IDataRequest previous, IOperation remove, Switch active)
	{
		_previous = previous;
		_remove   = remove;
		_active   = active;
	}

	public async ValueTask<object> Get(DataManagerRequest parameter)
	{
		try
		{
			_active.Up();
			return await _previous.Off(parameter);
		}
		catch
		{
			_active.Down();
			await _remove.Off();
			throw;
		}
	}
}
