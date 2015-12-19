using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Linq;

namespace DragonSpark.Aspects
{
	[PSerializable]
	[LinesOfCodeAvoided( 6 )]
	public class Cache : MethodInterceptionAspect
	{
		class Stored : ConnectedValue<WeakReference>
		{
			static string DetermineKey( MethodInterceptionArgs args ) => string.Join( "_", new[] { DetermineHost( args ), args.Method }.Concat( args.Arguments ).Select( o => o?.GetHashCode() ?? -1 ) );

			static object DetermineHost( MethodInterceptionArgs args ) => args.Instance ?? args.Method.DeclaringType;

			public Stored( MethodInterceptionArgs args ) : base( DetermineHost( args ), DetermineKey( args ), () => args.With( x => x.Proceed() ).ReturnValue.With( item => new WeakReference( item ) ) )
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
			args.ReturnValue = new Stored( args ).Item?.Target;
		}
	}
}