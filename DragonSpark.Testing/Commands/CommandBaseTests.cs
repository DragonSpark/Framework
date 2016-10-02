using DragonSpark.Commands;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class CommandBaseTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Execute( Command sut )
		{
			sut.Execute( new object() );
			Assert.True( sut.Executed, "Didn't call" );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Update( Command sut )
		{
			var called = false;
			sut.CanExecuteChanged += ( sender, args ) => called = true;
			sut.Update();
			Assert.True( called, "Didn't call" );
		}
	}

	public class Command : Command<object> {}

	public class Command<T> : CommandBase<T>
	{
		public bool Executed { get; private set; }
		
		public override void Execute( T parameter ) => Executed = true;
	}
}