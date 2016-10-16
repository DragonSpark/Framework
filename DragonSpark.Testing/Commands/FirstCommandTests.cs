using DragonSpark.Commands;
using DragonSpark.Extensions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class FirstCommandTests
	{
		[Fact]
		public void Verify()
		{
			var commands = new []{ 3, 4, 5, 8, 10 }.Select( i => new Command( i ) ).Fixed();
			var first = new FirstCommand<int>( commands );
			first.Execute( 8 );
			Assert.Equal( 8, commands.Where( command => command.Executed ).Only().Number );
		}

		class Command : Command<int>
		{
			public Command( int number )
			{
				Number = number;
			}

			public int Number { get; }

			public override bool IsSatisfiedBy( int parameter ) => Number == parameter;
		}
	}
}