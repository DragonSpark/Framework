using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;
using System;
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

	public class AssociatedAutoData : AssociatedValue<object, AutoData>
	{
		public AssociatedAutoData( object instance ) : base( instance )
		{}
	}

	[LinesOfCodeAvoided( 8 ), Serializable]
	public class AssignExecutionAttribute : MethodInterceptionAspect
	{
		public sealed override void OnInvoke( MethodInterceptionArgs args )
		{
			using ( var command = new AssignExecutionCommand() )
			{
				command.Execute( MethodContext.Get( args.Method ) );

				args.Proceed();
			}
		}
	}

	public class AssignExecutionCommand : AssignValueCommand<Tuple<string>>
	{
		public AssignExecutionCommand() : this( CurrentExecution.Instance )
		{}

		public AssignExecutionCommand( IWritableValue<Tuple<string>> value ) : base( value )
		{}
	}

	// [AssignExecution( AttributeInheritance = MulticastInheritance.Multicast, AttributeTargetMemberAttributes = MulticastAttributes.Instance )]
	public abstract class Tests : IDisposable
	{
		protected Tests( ITestOutputHelper output ) : this( output, new InitializeOutputCommand( output ).Run )
		{}

		protected Tests( ITestOutputHelper output, Action<Type> command )
		{
			Output = output;
			command( GetType() );
		}

		protected ITestOutputHelper Output { get; }

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing )
		{
			disposing.IsTrue( OnDispose );
		}

		protected virtual void OnDispose()
		{}
	}
}