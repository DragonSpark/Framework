using DragonSpark.Aspects;
using DragonSpark.Aspects.Validation;
using DragonSpark.Sources.Parameterized;
using Moq;
using PostSharp.Aspects;
using System;
using System.Reflection;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Aspects.Validation
{
	public class AutoValidationControllerTests
	{
		[Fact]
		public void CommandWorkflow()
		{
			var command = new Command();
			Assert.Equal( 0, command.CanExecuteCalled );
			var notNumber = command.CanExecute( new object() );
			Assert.False( notNumber );
			Assert.Equal( 1, command.CanExecuteCalled );

			var cannot = command.CanExecute( 123 );
			Assert.False( cannot );
			Assert.Equal( 2, command.CanExecuteCalled );

			const int valid = 1212;
			var can = command.CanExecute( valid );
			Assert.True( can );
			Assert.Equal( 3, command.CanExecuteCalled );
			
			Assert.Equal( 0, command.ExecuteCalled );

			command.Execute( 123 );

			Assert.Equal( 4, command.CanExecuteCalled );
			Assert.Equal( 0, command.ExecuteCalled );
			
			command.Execute( valid );
			Assert.Equal( 5, command.CanExecuteCalled );
			Assert.Equal( 1, command.ExecuteCalled );
			Assert.Equal( valid, command.LastResult.GetValueOrDefault() );
		}

		[Fact]
		public void Handler()
		{
			var method = (MethodInfo)MethodBase.GetCurrentMethod();
			var adapter = new Adapter( method );
			var controller = new AutoValidationController( adapter );
			var first = Guid.NewGuid();
			var one = new Aspect( method, first );
			controller.Register( one );
			Assert.Same( one, controller.Execute( first, new Mock<IInvocation>().Object ) );
		}

		[Fact]
		public void Linked()
		{
			var method = (MethodInfo)MethodBase.GetCurrentMethod();
			var adapter = new Adapter( method );
			var controller = new AutoValidationController( adapter );
			var first = Guid.NewGuid();
			var second = Guid.NewGuid();
			var one = new Aspect( method, first );
			controller.Register( one );

			var two = new Aspect( method, second );
			controller.Register( two );

			Assert.True( controller.Handles( first ) );
			Assert.True( controller.Handles( second ) );
			Assert.False( controller.Handles( Guid.NewGuid() ) );

			var invocation = new Mock<IInvocation>().Object;
			Assert.Same( one, controller.Execute( first, invocation ) );
			Assert.Same( two, controller.Execute( second, invocation ) );
		}

		sealed class Adapter : IParameterValidationAdapter
		{
			readonly MethodInfo method;

			public Adapter( MethodInfo method )
			{
				this.method = method;
			}

			public bool IsSatisfiedBy( MethodInfo parameter ) => parameter == method;

			public bool IsSatisfiedBy( object parameter ) => false;
		}

		sealed class Aspect : IAspect, IMethodAware, IParameterAwareHandler
		{
			readonly Guid id;

			public Aspect( MethodInfo method, Guid id )
			{
				this.id = id;
				Method = method;
			}

			public MethodInfo Method { get; }

			public bool Handles( object parameter ) => id.Equals( parameter );

			public bool Handle( object parameter, out object handled )
			{
				var result = Handles( parameter );
				handled = result ? this : null;
				return result;
			}
		}

		[Fact]
		public void GenericCommandWorkflow()
		{
			var command = new GenericCommand();
			var controller = AutoValidationControllerFactory.Default.Get( command );

			Assert.Equal( 0, command.CanExecuteCalled );
			var notNumber = command.CanExecute( new object() );
			Assert.False( notNumber );
			Assert.Equal( 1, command.CanExecuteCalled );
			Assert.Equal( 0, command.CanExecuteGenericCalled );

			var number = command.CanExecute( 123 );
			Assert.False( number );
			Assert.Equal( 2, command.CanExecuteCalled );
			Assert.Equal( 1, command.CanExecuteGenericCalled );

			var valid = command.CanExecute( 6776 );
			Assert.True( valid );
			Assert.Equal( 3, command.CanExecuteCalled );
			Assert.Equal( 2, command.CanExecuteGenericCalled );

			var again = command.CanExecute( 6776 );
			Assert.True( again );
			Assert.Equal( 4, command.CanExecuteCalled );
			Assert.Equal( 3, command.CanExecuteGenericCalled );

			Assert.Equal( 0, command.ExecuteCalled );
			Assert.Equal( 0, command.ExecuteGenericCalled );

			command.Execute( new object() );
			
			Assert.Equal( 5, command.CanExecuteCalled );
			Assert.Equal( 3, command.CanExecuteGenericCalled );
			Assert.Equal( 0, command.ExecuteCalled );
			Assert.Equal( 0, command.ExecuteGenericCalled );

			command.Execute( 123 );
			
			Assert.Equal( 6, command.CanExecuteCalled );
			Assert.Equal( 4, command.CanExecuteGenericCalled );
			Assert.Equal( 0, command.ExecuteCalled );
			Assert.Equal( 0, command.ExecuteGenericCalled );

			controller.MarkValid( 6776, true );
			command.Execute( 6776 );

			Assert.Equal( 6, command.CanExecuteCalled );
			Assert.Equal( 4, command.CanExecuteGenericCalled );
			Assert.Equal( 1, command.ExecuteCalled );
			Assert.Equal( 1, command.ExecuteGenericCalled );

			var result = command.LastResult;
			Assert.Equal( 6776, result.Number );

			var parameter = new GenericCommand.Parameter( 1234 );
			Assert.False( command.IsSatisfiedBy( parameter ) );
			
			Assert.Equal( 6, command.CanExecuteCalled );
			Assert.Equal( 5, command.CanExecuteGenericCalled );
			Assert.Equal( 1, command.ExecuteCalled );
			Assert.Equal( 1, command.ExecuteGenericCalled );

			var validGeneric = new GenericCommand.Parameter( 6776 );
			Assert.True( command.IsSatisfiedBy( validGeneric ) );

			Assert.Equal( 6, command.CanExecuteCalled );
			Assert.Equal( 6, command.CanExecuteGenericCalled );
			Assert.Equal( 1, command.ExecuteCalled );
			Assert.Equal( 1, command.ExecuteGenericCalled );

			command.Execute( validGeneric );

			Assert.Equal( 6, command.CanExecuteCalled );
			Assert.Equal( 6, command.CanExecuteGenericCalled );
			Assert.Equal( 1, command.ExecuteCalled );
			Assert.Equal( 2, command.ExecuteGenericCalled );

			Assert.Same( validGeneric, command.LastResult );
		}

		[ApplyAutoValidation]
		class Command : ICommand
		{
			public event EventHandler CanExecuteChanged = delegate {};

			public int CanExecuteCalled { get; private set; }

			public int ExecuteCalled { get; private set; }

			public int? LastResult { get; private set; }

			public bool CanExecute( object parameter )
			{
				CanExecuteCalled++;
				return parameter is int && (int)parameter == 1212;
			}

			public void Execute( object parameter )
			{
				ExecuteCalled++;
				LastResult = (int)parameter;
			}
		}

		[ApplyAutoValidation]
		class GenericCommand : DragonSpark.Commands.ICommand<GenericCommand.Parameter>
		{
			public event EventHandler CanExecuteChanged = delegate {};

			public int CanExecuteCalled { get; private set; }
			public int CanExecuteGenericCalled { get; private set; }

			public int ExecuteCalled { get; private set; }
			public int ExecuteGenericCalled { get; private set; }

			public Parameter LastResult { get; private set; }

			public bool CanExecute( object parameter )
			{
				CanExecuteCalled++;
				return parameter is int && IsSatisfiedBy( new Parameter( (int)parameter ) );
			}

			public void Execute( object parameter )
			{
				ExecuteCalled++;
				Execute( new Parameter( (int)parameter ) );
			}

			public bool IsSatisfiedBy( Parameter parameter )
			{
				CanExecuteGenericCalled++;
				return parameter.Number == 6776;
			}

			public void Execute( Parameter parameter )
			{
				ExecuteGenericCalled++;
				LastResult = parameter;
			}

			void DragonSpark.Commands.ICommand<Parameter>.Update() {}

			public class Parameter
			{
				public Parameter( int number )
				{
					Number = number;
				}

				public int Number { get; }
			}
		}
	}
}