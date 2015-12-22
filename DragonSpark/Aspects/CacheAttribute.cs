using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Linq;

namespace DragonSpark.Aspects
{
	[PSerializable]
	[LinesOfCodeAvoided( 6 )]
	[ProvideAspectRole( StandardRoles.Caching )]
	[AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading )]
	public sealed class Cache : MethodInterceptionAspect
	{
		/*public object On { get; set; }*/

		class Stored : ConnectedValue<WeakReference>
		{
			static string DetermineKey( MethodInterceptionArgs args ) => CacheKeyFactory.Instance.Create( new [] { DetermineHost( args ), args.Method }.Concat( args.Arguments )  );

			static object DetermineHost( MethodInterceptionArgs args ) => args.Instance ?? args.Method.DeclaringType;

			static WeakReference FromArgs( MethodInterceptionArgs args ) => args.With( x => x.Proceed() ).ReturnValue.With( item => new WeakReference( item ) );

			public Stored( MethodInterceptionArgs args ) : base( DetermineHost( args ), DetermineKey( args ), () => FromArgs( args ) )
			{}

			public override WeakReference Item => base.Item.With( x => x.IsAlive ? x : Clear() );

			WeakReference Clear()
			{
				Property.TryDisconnect();
				return base.Item;
			}
		}

		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var stored = new Stored( args );
			stored.Item?.Target.With( o =>
			{
				args.ReturnValue = o;
			} );
		}
	}
}