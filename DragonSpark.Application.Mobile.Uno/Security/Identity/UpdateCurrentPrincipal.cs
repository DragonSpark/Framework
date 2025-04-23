using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

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
			await _assign.Off();
		}
		else
		{
			_clear.Execute();
		}
	}
}