using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
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

	public class AssociatedFixture : AssociatedValue<MethodBase, IFixture>
	{
		public AssociatedFixture( MethodBase instance ) : base( instance )
		{}
	}

	[LinesOfCodeAvoided( 1 ), Serializable]
	public class AssignExecutionAttribute : InstanceLevelAspect
	{
		[OnMethodInvokeAdvice, MulticastPointcut( Attributes = MulticastAttributes.Instance, Targets = MulticastTargets.Method )]
		public void OnInvoke( MethodInterceptionArgs args )
		{
			using ( new ExecutionContext( args.Method ) )
			{
				args.Proceed();
			}
		}
	}

	public class ExecutionContext : IDisposable
	{
		readonly IWritableValue<Tuple<string>> context;

		public ExecutionContext( MethodBase info ) : this( CurrentExecution.Instance, info )
		{}

		public ExecutionContext( IWritableValue<Tuple<string>> context, MethodBase info )
		{
			this.context = context;
			context.Assign( MethodContext.Get( info ) );
		}

		public void Dispose()
		{
			context.Assign( null );
		}
	}

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