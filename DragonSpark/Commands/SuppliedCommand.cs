using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Commands
{
	public class SuppliedCommand<T> : RunCommandBase
	{
		readonly ICommand<T> command;
		readonly Func<T> parameter;

		public SuppliedCommand( ICommand<T> command, T parameter ) : this( command, Factory.For( parameter ) ) {}

		public SuppliedCommand( ICommand<T> command, Func<T> parameter )
		{
			this.command = new SpecificationCommand<T>( Common<T>.Assigned, command.ToDelegate() );
			this.parameter = parameter;
		}

		public override void Execute() => command.Execute( parameter() );

		protected override void OnDispose() => command.TryDispose();
	}
}