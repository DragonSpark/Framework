using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class UpdateCurrentPrincipal : IOperation<bool>
{
	readonly AssignCurrentPrincipal _assign;
	readonly ICommand               _clear;

	public UpdateCurrentPrincipal(AssignCurrentPrincipal assign, PrincipalStores clear)
	{
		_assign = assign;
		_clear  = clear;
	}

	public async ValueTask Get(bool parameter)
	{
		if (parameter)
		{
			await _assign.Await();
		}
		else
		{
			_clear.Execute();
		}
	}
}