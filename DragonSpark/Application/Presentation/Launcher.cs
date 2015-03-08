using System.Windows;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation
{
	public class Launcher<TShell> : Launcher where TShell : DependencyObject
	{
		protected override DependencyObject CreateShell()
		{
			var result = Container.Resolve<TShell>();
			return result;
		}
	}
}