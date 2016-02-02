using System;
using System.Windows.Input;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Testing.TestObjects.Modules;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class SetupCommandTests
	{
		[Theory, AutoData]
		public void Execute( Command sut )
		{
			sut.Execute( new object() );
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
		public void CallWithNonContext( Command<SetupCommandTests> sut )
		{
			var context = new object();
			Assert.Throws<InvalidOperationException>( () => sut.To<ICommand>().Execute( context ) );
		}
	}

	public class Command : Command<object> {}

	public class Command<T> : SetupCommandBase<T>
	{
		public bool Executed { get; private set; }
		
		protected override void OnExecute( T parameter ) => Executed = true;
	}
}