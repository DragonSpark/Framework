using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Presentation.Environment.Browser.Window;

sealed class CreateWindowFocusElement : IStopAware<NewWindowFocusElementInput, WindowFocusElement>
{
	readonly LoadModule<WindowFocusElement> _load;
	readonly NewWindowFocusElement          _new;

	public CreateWindowFocusElement(LoadModule<WindowFocusElement> load) : this(load, NewWindowFocusElement.Default) {}

	public CreateWindowFocusElement(LoadModule<WindowFocusElement> load, NewWindowFocusElement @new)
	{
		_load = load;
		_new  = @new;
	}

	public async ValueTask<WindowFocusElement> Get(Stop<NewWindowFocusElementInput> parameter)
	{
		var (subject, stop) = parameter;
		var module       = await _load.Off(stop);
		var reference    = await _new.Off(new(new(module, subject), stop));
		return new(reference);
	}
}