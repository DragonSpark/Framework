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
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			Activation.Execution.Initialize( CurrentExecution.Instance );
		}
	}

	public class OutputValue : AssociatedValue<Type, ITestOutputHelper>
	{
		public OutputValue( Type instance ) : base( instance )
		{}
	}

	public class AssociatedFixture : AssociatedValue<MethodInfo, IFixture>
	{
		public AssociatedFixture( MethodInfo instance ) : base( instance )
		{}
	}

	[LinesOfCodeAvoided( 1 ), Serializable]
	public class AssignExecutionAttribute : InstanceLevelAspect
	{
		[OnMethodInvokeAdvice, MulticastPointcut( Attributes = MulticastAttributes.Instance, Targets = MulticastTargets.Method )]
		public void OnInvoke( MethodInterceptionArgs args )
		{
			CurrentExecution.Instance.Assign( args.Method );
			args.Proceed();
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