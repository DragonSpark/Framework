using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;

namespace DragonSpark.Commands
{
	public class SuppliedEnumerableComand<T> : SuppliedCommand<IEnumerable<T>>
	{
		public SuppliedEnumerableComand( ICommand<IEnumerable<T>> command, params T[] parameter ) : base( command, parameter ) {}
	}

	public class SuppliedCommand<T> : RunCommandBase
	{
		readonly ICommand<T> command;
		readonly Func<T> parameter;

		public SuppliedCommand( ICommand<T> command, T parameter ) : this( command, Factory.For( parameter ) ) {}

		public SuppliedCommand( ICommand<T> command, Func<T> parameter ) : base( command.AsDisposable() )
		{
			this.command = new SpecificationCommand<T>( Common<T>.Assigned, command.ToDelegate() );
			this.parameter = parameter;
		}

		public override void Execute() => command.Execute( parameter() );
	}
}