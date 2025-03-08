using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Window;

sealed class CreateWindowFocusElement : ISelecting<NewWindowFocusElementInput, WindowFocusElement>
{
	readonly LoadModule<WindowFocusElement> _load;
	readonly NewWindowFocusElement          _new;

	public CreateWindowFocusElement(LoadModule<WindowFocusElement> load) : this(load, NewWindowFocusElement.Default) {}

	public CreateWindowFocusElement(LoadModule<WindowFocusElement> load, NewWindowFocusElement @new)
	{
		_load = load;
		_new  = @new;
	}

	public async ValueTask<WindowFocusElement> Get(NewWindowFocusElementInput parameter)
	{
		var module    = await _load.Off();
		var reference = await _new.Off(new(module, parameter));
		return new(reference);
	}
}