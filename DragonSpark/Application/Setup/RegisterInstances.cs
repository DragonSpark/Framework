using DragonSpark.Commands;
using DragonSpark.Runtime;
using System;
using System.Windows.Markup;

namespace DragonSpark.Application.Setup
{
	[ContentProperty( nameof(Instances) )]
	public class RegisterInstances : CommandBase<object>
	{
		readonly static Action<object> Register = RegisterInstanceCommand.Default.Execute;

		readonly Action<object> register;

		public RegisterInstances() : this( Register ) {}

		public RegisterInstances( Action<object> register )
		{
			this.register = register;
		}

		public override void Execute( object parameter )
		{
			foreach ( var instance in Instances )
			{
				register( instance );
			}
		}

		public DeclarativeCollection<object> Instances { get; } = new DeclarativeCollection<object>();
	}
}