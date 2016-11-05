using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Aspects.Specifications
{
	public class ApplySpecificationAttributeTests
	{
		[Fact]
		public void VerifySubject()
		{
			var sut = new Subject();
			Assert.False( sut.IsSatisfiedBy( 123 ) );
			Assert.False( sut.Called );
			Assert.True( sut.IsSatisfiedBy( 6776 ) );
			Assert.False( sut.Called );
			Assert.False( sut.IsSatisfiedBy( 123 ) );
			Assert.False( sut.Called );
		}

		[Fact]
		public void VerifyCommand()
		{
			var sut = new Command();
			sut.Execute( 123 );
			Assert.Empty( sut.Parameters );

			const int valid = 6776;
			sut.Execute( valid );
			Assert.Equal( valid, Assert.Single( sut.Parameters ) );
		}

		[Fact]
		public void VerifySpecification()
		{
			var sut = new SpecificationCommand();
			Assert.False( sut.IsSatisfiedBy( 1234 ) );
			Assert.True( sut.IsSatisfiedBy( 6776 ) );
		}

		[Fact]
		public void AutoValidationOnlyOnce()
		{
			var sut = new PocoCommand();

			Assert.Empty( sut.Parameters );
			sut.Execute( 123 );
			Assert.Equal( 123, Assert.Single( sut.Parameters ) );

			for ( int i = 0; i < 10; i++ )
			{
				sut.Execute( 123 );
			}
			Assert.Single( sut.Parameters );
		}

		[Theory, AutoData]
		void VerifyAutoValidation( CoreCommand sut )
		{
			var applied = sut;
			Assert.False( applied.CanExecute( 123 ) );
			Assert.True( applied.CanExecute( 6776 ) );
			Assert.Equal( 2, applied.CanExecuteCalled );

			Assert.Empty( applied.Parameters );
			applied.Execute( 123 );
			Assert.Empty( applied.Parameters );
			Assert.Equal( 3, applied.CanExecuteCalled );

			Assert.True( applied.CanExecute( 6776 ) );
			Assert.Equal( 4, applied.CanExecuteCalled );

			Assert.Empty( applied.Parameters );
			applied.Execute( 6776 );
			Assert.Single( applied.Parameters, 6776 );
			Assert.Equal( 4, applied.CanExecuteCalled );
		}

		[UsedImplicitly, ApplyAutoValidation]
		class CoreCommand : ICommand
		{
			public event EventHandler CanExecuteChanged = delegate {};

			public int CanExecuteCalled { get; private set; }

			public bool CanExecute( object parameter )
			{
				CanExecuteCalled++;
				return parameter is int && (int)parameter == 6776;
			}

			public void Execute( object parameter ) => Parameters.Add( parameter );

			public ICollection<object> Parameters { get; } = new Collection<object>();
		}

		[ApplyAutoValidation, ApplySpecification( typeof(OnlyOnceSpecification<int>) )]
		class PocoCommand : CommandBase<int>
		{
			public override void Execute( int parameter ) => Parameters.Add( parameter );

			public ICollection<int> Parameters { get; } = new Collection<int>();
		}

		[ApplyAutoValidation]
		class Command : CommandBase<int>
		{
			public override bool IsSatisfiedBy( int parameter ) => parameter == 6776;

			public override void Execute( int parameter ) => Parameters.Add( parameter );

			public ICollection<int> Parameters { get; } = new Collection<int>();
		}

		[ApplySpecification( typeof(Specification) )]
		sealed class SpecificationCommand : CommandBase<int>
		{
			public override void Execute( int parameter ) {}
		}

		[ApplySpecification( typeof(Specification) )]
		class Subject : ISpecification<int>
		{
			public bool Called { get; private set; }

			public bool IsSatisfiedBy( int parameter )
			{
				Called = true;
				return false;
			}
		}

		sealed class Specification : SpecificationBase<int>
		{
			[UsedImplicitly]
			public static Specification Default { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( int parameter ) => parameter == 6776;
		}
	}
}