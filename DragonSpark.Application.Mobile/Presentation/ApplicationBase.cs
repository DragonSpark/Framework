using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Presentation;


// ReSharper disable once MissingAnnotation
public class ApplicationBase : Microsoft.UI.Xaml.Application, IApplication
{
	public void Execute(Run.Application parameter)
	{
		var (builder, host) = parameter;
		MainWindow          = builder.Window;
		Host                = host;
	}

	public Window MainWindow { get; private set; } = null!;

	public IHost Host { get; private set; } = null!;
}
