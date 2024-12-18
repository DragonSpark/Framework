using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Presentation;

public interface IApplication : ICommand<Run.Application>
{
	Window MainWindow { get; }

	IHost Host { get; }
}