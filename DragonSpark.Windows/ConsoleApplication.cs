using System.Windows.Input;
using DragonSpark.Application;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows
{
	public class ConsoleApplication : ApplicationBase<string[]>
	{
		public ConsoleApplication() : this( Items<ICommand>.Default ) {}
		public ConsoleApplication( params ICommand[] commands ) : base( commands ) {}
	}
}