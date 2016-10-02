using DragonSpark.Aspects.Relay;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using PostSharp.Patterns.Model;
using System;
using Xunit;

namespace DragonSpark.Testing.Aspects.Relay
{
	public class ApplyRelayAttributeTests
	{
		[Fact]
		public void Verify()
		{
			var sut = new Command();
			Assert.False( sut.CanExecute( 123 ) );
			Assert.Equal( 0, sut.CanExecuteCalled );
			Assert.Equal( 1, sut.CanExecuteGenericCalled );

			Assert.True( sut.CanExecute( 6776 ) );
			Assert.Equal( 0, sut.CanExecuteCalled );
			Assert.Equal( 2, sut.CanExecuteGenericCalled );

			sut.Execute( (object)6776 );

			Assert.Equal( 0, sut.ExecuteCalled );
			Assert.Equal( 1, sut.ExecuteGenericCalled );
			Assert.Equal( 6776, sut.LastResult );
		}

		[Fact]
		public void VerifySpecification()
		{
			var sut = new Specification();
			var generalized = sut.QueryInterface<ISpecification<object>>();
			Assert.False( generalized.IsSatisfiedBy( 123 ) );
			Assert.Equal( 1, sut.IsSatisfiedByCalled );
			Assert.True( generalized.IsSatisfiedBy( 6776 ) );
			Assert.Equal( 2, sut.IsSatisfiedByCalled );
			Assert.Equal( 0, sut.GeneralizedIsSatisfiedByCalled );
		}

		[Fact]
		public void VerifySource()
		{
			var sut = new ParameterizedSource();
			var generalized = sut.QueryInterface<IParameterizedSource<object, object>>();
			Assert.False( (bool)generalized.Get( "Not Valid" ) );
			Assert.Equal( 1, sut.GetCalled );
			Assert.True( (bool)generalized.Get( ParameterizedSource.HelloWorld ) );
			Assert.Equal( 2, sut.GetCalled );
			Assert.Equal( 0, sut.GeneralizedGetCalled );
		}

		abstract class GeneralizedParameterizedSourceBase<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IParameterizedSource<object, object>
		{
			public int GeneralizedGetCalled { get; private set; }

			public object Get( object parameter )
			{
				GeneralizedGetCalled++;
				return null;
			}
		}

		[ApplyRelay]
		class ParameterizedSource : GeneralizedParameterizedSourceBase<string, bool>
		{
			public const string HelloWorld = "Hello World!";

			public int GetCalled { get; private set; }

			public override bool Get( string parameter )
			{
				GetCalled++;
				return parameter == HelloWorld;
			}
		}

		abstract class GeneralizedSpecificationBase<T> : SpecificationBase<T>, ISpecification<object>
		{
			public int GeneralizedIsSatisfiedByCalled { get; private set; }

			bool ISpecification<object>.IsSatisfiedBy( object parameter )
			{
				GeneralizedIsSatisfiedByCalled++;
				return false;
			}
		}

		[/*EnsureGeneralizedImplementations,*/ ApplyRelay]
		sealed class Specification : GeneralizedSpecificationBase<int>
		{
			public int IsSatisfiedByCalled { get; private set; }

			public override bool IsSatisfiedBy( int parameter )
			{
				IsSatisfiedByCalled++;
				return parameter == 6776;
			}
		}

		[ApplyRelay]
		sealed class Command : DragonSpark.Commands.ICommand<int>
		{
			public event EventHandler CanExecuteChanged = delegate {};

			public int CanExecuteCalled { get; private set; }
			public int CanExecuteGenericCalled { get; private set; }

			public int ExecuteCalled { get; private set; }
			public int ExecuteGenericCalled { get; private set; }

			public int LastResult { get; private set; }

			public bool CanExecute( object parameter )
			{
				CanExecuteCalled++;
				return false;
			}

			public void Execute( object parameter ) => ExecuteCalled++;

			public bool IsSatisfiedBy( int parameter )
			{
				CanExecuteGenericCalled++;
				return parameter == 6776;
			}

			public void Execute( int parameter )
			{
				ExecuteGenericCalled++;
				LastResult = parameter;
			}

			void DragonSpark.Commands.ICommand<int>.Update() {}
		}
	}
}