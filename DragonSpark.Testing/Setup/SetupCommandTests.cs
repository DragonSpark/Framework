using DragonSpark.Setup;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	public class SetupCommandTests
	{
		[Theory, AutoData]
		public void Execute( Command sut )
		{
			sut.Execute( new Context( new object() ) );
			Assert.True( sut.Executed, "Didn't call" );
		}

		[Theory, AutoData]
		public void Update( Command sut )
		{
			var called = false;
			sut.CanExecuteChanged += ( sender, args ) => called = true;
			sut.Update();
			Assert.True( called, "Didn't call" );
		}

		[Theory, AutoData]
		public void CallWithNonContext( Command sut )
		{
			var context = new object();
			Assert.Throws<InvalidOperationException>( () => sut.Execute( context ) );
		}
	}

	public class Command : SetupCommand<Context>
	{
		public bool Executed { get; private set; }
		
		protected override void Execute( Context context )
		{
			Executed = true;
		}
	}

	public class Context : SetupContext
	{
		public Context( object arguments ) : base( arguments )
		{}
	}
}