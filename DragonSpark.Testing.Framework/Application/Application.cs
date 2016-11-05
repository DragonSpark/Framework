using DragonSpark.Application;
using System.Windows.Input;

namespace DragonSpark.Testing.Framework.Application
{
	public class Application : ApplicationBase<AutoData>, IApplication
	{
		public Application( params ICommand[] commands ) : base( commands ) {}
	}
}