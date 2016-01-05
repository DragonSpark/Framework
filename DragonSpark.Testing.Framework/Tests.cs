using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework
{
	public class OutputValue : AssociatedValue<Type, ITestOutputHelper>
	{
		public OutputValue( Type instance ) : base( instance )
		{}
	}

	public class AssociatedAutoData : AssociatedValue<object, AutoData>
	{
		public AssociatedAutoData( object instance ) : base( instance )
		{}
	}

	[LinesOfCodeAvoided( 8 ), Serializable]
	public class AssignExecutionAttribute : MethodInterceptionAspect
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			using ( var command = new AssignExecutionContextCommand() )
			{
				command.Execute( MethodContext.Get( args.Method ) );
				using ( var setup = new SetupExecution( args.Method ) )
				{
					setup.Execute( args.Proceed );
				}
			}
		}
	}

	public class SetupExecution : DisposingCommand<Action>
	{
		readonly AutoData data;

		public SetupExecution( MethodBase method ) : this( new AssociatedAutoData( method ).Item )
		{}

		public SetupExecution( [Required]AutoData data )
		{
			this.data = data;
		}

		protected override void OnExecute( Action parameter )
		{
			data.Items.Each( aware => aware.Before( data ) );
			parameter();
		}

		protected override void OnDispose() => data.Items.Each( aware => aware.After( data ) );
	}

	public class AssignExecutionContextCommand : ValueContextCommand<Tuple<string>>
	{
		public AssignExecutionContextCommand() : this( CurrentExecution.Instance )
		{}

		public AssignExecutionContextCommand( IWritableValue<Tuple<string>> value ) : base( value )
		{}
	}

	// [AssignExecution( AttributeInheritance = MulticastInheritance.Multicast, AttributeTargetMemberAttributes = MulticastAttributes.Instance )]
	public abstract class Tests : IDisposable
	{
		protected Tests( ITestOutputHelper output )
		{
			new OutputValue( GetType() ).Assign( output );
		}

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