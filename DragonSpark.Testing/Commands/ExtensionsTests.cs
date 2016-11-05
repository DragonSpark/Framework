using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class ExtensionsTests
	{
		[Fact]
		public void ToSpecification()
		{
			var sut = new Command<object>();
			ICommand command = sut;
			Assert.NotSame( sut.ToSpecification(), command.ToSpecification() );
		}

		[Fact]
		public void WithParameter()
		{
			new Command<object>().ToDelegate().WithParameter( new object() ).Execute();
			new Command<object>().ToDelegate().WithParameter( new object().Self ).Execute();
		}

		[Fact]
		public void Apply()
		{
			var command = new Command();
			var sut = command.Accept( Coercer.Default );
			Assert.Null( Tags.Default.Get( command ) );
			sut.Execute( GetType() );
			Assert.Same( GetType().Assembly, Tags.Default.Get( command ) );
		}

		[Fact]
		public void AsConfigurable()
		{
			var command = new Command();
			var configurable = command.AsConfigurable();
			Assert.Null( Tags.Default.Get( command ) );
			configurable.Execute( GetType().Assembly );
			Assert.Same( GetType().Assembly, Tags.Default.Get( command ) );

			Tags.Default.Remove( command );

			configurable.Configuration.Assign( () => x => {} );
			Assert.Null( Tags.Default.Get( command ) );
			configurable.Execute( GetType().Assembly );
			Assert.Null( Tags.Default.Get( command ) );
		}

		[Fact]
		public void ToExecuteDelegate()
		{
			Assert.NotNull( Command.Current.ToExecuteDelegate() );
			Assert.NotNull( Command.Action.ToExecuteDelegate() );
		}

		[Fact]
		public void Execute()
		{
			var command = new Command();
			command.AsCompiled().ExecuteItem( GetType().Assembly );
			Assert.Same( GetType().Assembly, Tags.Default.Get( command ) );
		}

		sealed class Coercer : CoercerBase<Type, Assembly>
		{
			public static Coercer Default { get; } = new Coercer();
			Coercer() {}

			protected override Assembly Coerce( Type parameter ) => parameter.Assembly;
		}

		sealed class Tags : Cache<object>
		{
			public static Tags Default { get; } = new Tags();
			Tags() {}
		}

		sealed class Command : CommandBase<Assembly>
		{
			public static IScope<Command> Current { get; } = new SingletonScope<Command>( () => new Command() );
			public static IScope<Action<Assembly>> Action { get; } = new SingletonScope<Action<Assembly>>( () => new Command().Execute );

			public override void Execute( Assembly parameter ) => Tags.Default.Set( this, parameter );
		}
	}
}