using DragonSpark.Commands;
using System;

namespace DragonSpark.Application.Setup
{
	public sealed class RegisterInstanceCommand : CommandBase<object>
	{
		public static RegisterInstanceCommand Default { get; } = new RegisterInstanceCommand();
		RegisterInstanceCommand() : this( Instances.Default.Get ) {}

		readonly Func<IServiceRepository> repository;

		public RegisterInstanceCommand( Func<IServiceRepository> repository )
		{
			this.repository = repository;
		}

		public override void Execute( object parameter ) => repository().Add( parameter );
	}
}