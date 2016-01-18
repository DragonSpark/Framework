using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;
using System;
using PostSharp.Patterns.Model;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework
{
	public class OutputValue : AssociatedValue<Type, string[]>
	{
		public OutputValue( Type instance ) : base( instance )
		{}
	}

	public class InitializeOutputCommand : Command<Type>
	{
		readonly ITestOutputHelper helper;

		public InitializeOutputCommand( ITestOutputHelper helper )
		{
			this.helper = helper;
		}

		protected override void OnExecute( Type parameter )
		{
			var item = new OutputValue( parameter ).Item;
			item.With( lines => lines.Each( helper.WriteLine ) );
		}
	}

	[Serializable, LinesOfCodeAvoided( 8 )]
	public class AssignExecutionContextAspect : MethodInterceptionAspect
	{
		public sealed override void OnInvoke( MethodInterceptionArgs args )
		{
			var parameter = MethodContext.Get( args.Method );
			using ( new AssignExecutionContextCommand().ExecuteWith( parameter ) )
			{
				args.Proceed();
			}
		}
	}

	public class AssignExecutionContextCommand : AssignValueCommand<Tuple<string>>
	{
		public AssignExecutionContextCommand() : this( CurrentExecution.Instance ) {}

		public AssignExecutionContextCommand( IWritableValue<Tuple<string>> value ) : base( value ) {}
	}

	[Disposable]
	public abstract class Tests
	{
		protected Tests( ITestOutputHelper output ) : this( output, new InitializeOutputCommand( output ).Run ) {}

		protected Tests( ITestOutputHelper output, Action<Type> initialize )
		{
			Output = output;
			initialize( GetType() );
		}

		[Reference]
		protected ITestOutputHelper Output { get; }

		protected virtual void Dispose( bool disposing ) {}
	}
}