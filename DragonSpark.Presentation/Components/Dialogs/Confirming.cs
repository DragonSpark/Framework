using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Radzen;

namespace DragonSpark.Presentation.Components.Dialogs;

public class Confirming<T> : Confirming<None, T> where T : ConfirmComponentBase
{
	protected Confirming(DialogService service, string title, T result) : base(service, title, result) {}

	protected Confirming(IOperation display, IDialogResultAware result) : base(display, result) {}
}

public class Confirming<T, TConfirm> : IConfirming<T> where TConfirm : ConfirmComponentBase
{
	readonly IOperation         _display;
	readonly IDialogResultAware _result;

	protected Confirming(DialogService service, string title, TConfirm result)
		: this(new OpenDialog(service, title, RenderFragments.Default.Get(result)), result) {}

	protected Confirming(IOperation display, IDialogResultAware result)
	{
		_display = display;
		_result  = result;
	}

	public async ValueTask<DialogResult> Get(T parameter)
	{
		await _display.Get().Go();
		return _result.Get();
	}
}