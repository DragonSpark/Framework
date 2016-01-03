using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Aspects
{
	public static class MethodInterceptionArgsExtensions
	{
		public static object GetReturnValue( this MethodInterceptionArgs @this ) => @this.With( x => x.Proceed() ).ReturnValue;
	}

	[PSerializable, ProvideAspectRole( StandardRoles.Caching ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading ), LinesOfCodeAvoided( 6 )]
	public sealed class Cache : MethodInterceptionAspect
	{
		class Stored : ConnectedValue<WeakReference>
		{
			readonly static object Placeholder = new object();

			public Stored( Invocation instance, Func<object> factory ) : base( instance.Item1, Reference<Stored>.Key( instance ), () => new WeakReference( factory() ?? Placeholder ) )
			{}

			public override WeakReference Item => base.Item.With( reference => reference.IsAlive ? reference : Clear() );

			public object GetTarget() => Item.Target == Placeholder ? null : Item.Target;

			WeakReference Clear()
			{
				Property.TryDisconnect();
				return base.Item;
			}
		}

		class InvocationCount : ConnectedValue<int>
		{
			public InvocationCount( [Required]object instance ) : base( instance, typeof(InvocationCount), () => -1 )
			{}
		}

		class InvocationFrame : IDisposable
		{
			readonly InvocationCount count;

			public InvocationFrame( object instance ) : this( new InvocationCount( instance ) )
			{}

			InvocationFrame( [Required]InvocationCount count )
			{
				this.count = count;
				Set( true );
			}

			void Set( bool increase ) => count.Assign( count.Item + ( increase ? 1 : -1 ) );

			public object GetValue( IEnumerable<Func<object>> factories ) => factories.ElementAtOrDefault( count.Item )?.Invoke();

			public void Dispose() => Set( false );
		}

		class Invocation : Tuple<object, MethodBase, EqualityList>
		{
			public Invocation( MethodInterceptionArgs args ) : base( args.Instance ?? args.Method.DeclaringType, args.Method, new EqualityList( args.Arguments ) )
			{}
		}

		class InvocationReference : Reference<Invocation>
		{
			public InvocationReference( Invocation invocation ) : base( invocation, invocation.Item1 )
			{}
		}

		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var invocation = new Invocation( args );
			var reference = new InvocationReference( invocation ).Item;
			using ( var context = new InvocationFrame( reference ) )
			{
				var returnValue = context.GetValue( new Func<object>[] { new Stored( reference, args.GetReturnValue ).GetTarget, args.GetReturnValue } );
				args.ReturnValue = returnValue ?? args.ReturnValue;
			}
		}
	}
}