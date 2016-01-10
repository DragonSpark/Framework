using System.ComponentModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	[BuildUp]
	public class SetupObjectBuilderCommand : Command<object>
	{
		[Extension]
		public ObjectBuilderExtension Extension { [return: NotNull]get; set; }

		[DefaultValue( true )]
		public bool Enabled { get; set; }

		protected override void OnExecute( object parameter ) => Extension.Enable( Enabled );
	}
}