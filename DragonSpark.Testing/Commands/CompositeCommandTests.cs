using DragonSpark.Commands;
using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class CompositeCommandTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void Execute( Command command )
		{
			var sut = new CompositeCommand( command.ToItem() );
			Assert.False( command.Executed );
			sut.Execute( new object() );
			Assert.True( command.Executed );
		}

		[Fact]
		public void Coverage()
		{
			var sut = new CompositeCommand();
			Assert.NotNull( sut );
		}

		class Command : CommandBase<object>
		{
			public bool Executed { get; private set; }
			
			public override void Execute( object parameter )
			{
				Executed = true;
			}
		}
	}
}