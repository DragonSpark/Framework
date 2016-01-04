using PostSharp.Aspects;
using System;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class SimpleAspectTests
	{
		[Fact]
		public void PossibleBug()
		{
			Assert.Throws<NullReferenceException>( () => new MyClass().Method() );
		}

		class MyClass
		{
			[MethodInterceptor]
			public void Method()
			{}
		}

		[Serializable]
		public class MethodInterceptor : MethodInterceptionAspect
		{
			class InvocationFactory
			{
				readonly Action<MethodInterceptionArgs> invocation;

				public InvocationFactory() : this( args => args.Method.ToString() )
				{}

				InvocationFactory( Action<MethodInterceptionArgs> invocation )
				{
					this.invocation = invocation;
				}

				public void Method( MethodInterceptionArgs parameter )
				{
					invocation( parameter );
				}
			}

			public override void OnInvoke( MethodInterceptionArgs args )
			{
				new InvocationFactory().Method( args );
				base.OnInvoke( args );
			}
		}
	}
}