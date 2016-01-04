using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using System;
using System.Reflection;
using PostSharp.Patterns.Contracts;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework
{
	public class OutputValue : AssociatedValue<Type, ITestOutputHelper>
	{
		public OutputValue( Type instance ) : base( instance )
		{}
	}

	public class AssociatedSetup : AssociatedValue<object, SetupAutoData>
	{
		public AssociatedSetup( object instance ) : base( instance )
		{}
	}

	[LinesOfCodeAvoided( 1 ), Serializable]
	public class AssignExecutionAttribute : InstanceLevelAspect
	{
		[OnMethodInvokeAdvice, MulticastPointcut( Attributes = MulticastAttributes.Instance, Targets = MulticastTargets.Method )]
		public void OnInvoke( MethodInterceptionArgs args )
		{
			using ( var command = new AssignExecutionContextCommand() )
			{
				command.Execute( args.Method );
				using ( var setup = new SetupExecution( args.Method ) )
				{
					setup.Execute( args.Proceed );
				}
			}
		}
	}

	public class SetupExecution : Command<Action>, IDisposable
	{
		readonly SetupAutoData data;

		public SetupExecution( MethodBase method ) : this( new AssociatedSetup( method ).Item )
		{}

		public SetupExecution( [Required]SetupAutoData data )
		{
			this.data = data;
		}

		protected override void OnExecute( Action parameter )
		{
			data.Items.Each( aware => aware.Before( data ) );
			parameter();
		}

		public void Dispose()
		{
			data.Items.Each( aware => aware.After( data ) );
		}
	}

	public class AssignExecutionContextCommand : Command<MethodBase>, IDisposable
	{
		readonly IWritableValue<Tuple<string>> context;

		public AssignExecutionContextCommand() : this( CurrentExecution.Instance )
		{}

		public AssignExecutionContextCommand( IWritableValue<Tuple<string>> context )
		{
			this.context = context;
		}

		protected override void OnExecute( MethodBase parameter )
		{
			context.Assign( MethodContext.Get( parameter ) );
		}

		public void Dispose()
		{
			context.Assign( null );
		}
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