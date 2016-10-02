using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Sources.Parameterized;
using System.Windows.Input;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class CommandAttributeBase : HostingAttributeBase
	{
		protected CommandAttributeBase( ICommand command ) : this( command.Adapt<AutoData>() ) {}
		protected CommandAttributeBase( ICommand<AutoData> command ) : base( command.Wrap() ) {}
	}
}