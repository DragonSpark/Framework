using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Tasks
{
	public class SuppliedTaskSource<T> : SuppliedTaskSource
	{
		public SuppliedTaskSource( ICommand<T> command, T parameter ) : base( command.Fixed( parameter ).ToRunDelegate() ) {}
		public SuppliedTaskSource( ICommand<T> command, Func<T> parameter ) : base( command.Fixed( parameter() ).ToRunDelegate() ) {}
	}

	public class SuppliedTaskSource : SuppliedSource<Action, Task>
	{
		readonly static Func<Action, Task> Run = Task.Run;
		
		public SuppliedTaskSource( Action action ) : base( Run, action ) {}
	}
}