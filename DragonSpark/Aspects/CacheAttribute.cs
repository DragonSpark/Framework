using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Reflection;

namespace DragonSpark.Aspects
{
	public static class InterceptionArgsExtensions
	{
		public static object GetReturnValue( this MethodInterceptionArgs @this ) => @this.With( x => x.Proceed() ).ReturnValue;
	}
	
	[PSerializable, ProvideAspectRole( StandardRoles.Caching ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading ), LinesOfCodeAvoided( 6 ), AttributeUsage( AttributeTargets.Method | AttributeTargets.Property )]
	public sealed class Freeze : MethodInterceptionAspect
	{
		class Stored : ConnectedValue<object>
		{
			public Stored( Invocation instance, Func<object> factory ) : base( instance.Item1, Reference<Stored>.Key( instance ), factory ) {}
		}

		class Invocation : Tuple<object, MemberInfo, EqualityList>
		{
			public Invocation( object item1, MemberInfo item2, EqualityList item3 ) : base( item1, item2, item3 )
			{ }
		}

		class InvocationReference : Reference<Invocation>
		{
			public InvocationReference( Invocation invocation ) : base( invocation, invocation.Item1 )
			{ }
		}

		class MethodInvocationFactory : InvocationFactory<MethodInterceptionArgs>
		{
			public static MethodInvocationFactory Instance { get; } = new MethodInvocationFactory();

			MethodInvocationFactory() : base( args => new Invocation( args.Instance ?? args.Method.DeclaringType, args.Method, new EqualityList( args.Arguments ) ), args => args.GetReturnValue )
			{ }
		}

		abstract class InvocationFactory<T> : FactoryBase<T, object> where T : AdviceArgs
		{
			readonly Func<T, Invocation> invocation;
			readonly Func<T, Func<object>> factory;

			protected InvocationFactory( Func<T, Invocation> invocation, Func<T, Func<object>> factory )
			{
				this.invocation = invocation;
				this.factory = factory;
			}

			protected override object CreateItem( T parameter )
			{
				var item = invocation( parameter );
				var reference = new InvocationReference( item ).Item;
				var result = new Stored( reference, factory( parameter ) ).Item;
				return result;
			}
		}


		public override void OnInvoke( MethodInterceptionArgs args )
		{
			if ( !args.Method.IsSpecialName || args.Method.Name.Contains( "get_" ) )
			{
				args.ReturnValue = MethodInvocationFactory.Instance.Create( args ) ?? args.ReturnValue;
			}
			else
			{
				base.OnInvoke( args );
			}
		}
	}
}