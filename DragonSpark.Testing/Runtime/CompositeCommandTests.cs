using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class CompositeCommandTests
	{
		[Theory, AutoData]
		void Execute( Command command )
		{
			
			var sut = new CompositeCommand( command.ToItem() );
			Assert.False( command.Executed );
			sut.Execute( new object() );
			Assert.True( command.Executed );
		}

		class Command : Command<object>
		{
			public bool Executed { get; private set; }
			
			protected override void OnExecute( object parameter )
			{
				Executed = true;
			}
		}
	}
}