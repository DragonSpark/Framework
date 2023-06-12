using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Environment.Browser;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class StateAwareDataRequest : IDataRequest
{
	readonly IDataRequest _previous;
	readonly IOperation   _remove;

	public StateAwareDataRequest(IDataRequest previous, IClientVariable<string> state) : this(previous, state.Remove) {}

	public StateAwareDataRequest(IDataRequest previous, IOperation remove)
	{
		_previous = previous;
		_remove   = remove;
	}

	public async ValueTask<object> Get(DataManagerRequest parameter)
	{
		try
		{
			return await _previous.Get(parameter);
		}
		catch
		{
			await _remove.Await();
			throw;
		}
	}
}