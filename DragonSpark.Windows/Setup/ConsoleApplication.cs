using DragonSpark.Application;
using DragonSpark.TypeSystem;
using System.Windows.Input;

namespace DragonSpark.Windows.Setup
{
	public class ConsoleApplication : ApplicationBase<string[]>
	{
		public ConsoleApplication() : this( Items<ICommand>.Default ) {}
		public ConsoleApplication( params ICommand[] commands ) : base( commands ) {}
	}
}