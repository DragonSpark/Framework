using DragonSpark.Aspects.Coercion;
using DragonSpark.Coercion;
using System;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Aspects.Coercion
{
	public class ApplyCoercerAttributeTests
	{
		[Fact]
		public void Verify()
		{
			var sut = new Command();
			Assert.True( sut.CanExecute( 6776 ) );
			var invalid = 123;
			Assert.False( sut.CanExecute( invalid ) );
			var now = DateTime.Now;
			Assert.False( sut.CanExecute( now ) );
			sut.Execute( now );
			Assert.Equal( now, sut.LastResult );
			sut.Execute( invalid );
			Assert.Equal( invalid, sut.LastResult );
			sut.Execute( 6776 );
			Assert.Equal( Coercer.ValidMatch, sut.LastResult );
		}

		class Coercer : CoercerBase<int, string>
		{
			public const string ValidMatch = "Valid Match";

			protected override string Apply( int parameter )
			{
				switch ( parameter )
				{
					case 6776:
						return ValidMatch;
				}
				return null;
			}
		}

		[ApplyCoercer( typeof(Coercer) )]
		class Command : ICommand
		{
			public event EventHandler CanExecuteChanged = delegate {};

			public object LastResult { get; private set; }

			public bool CanExecute( object parameter ) => parameter is string;

			public void Execute( object parameter ) => LastResult = parameter;
		}
	}
}