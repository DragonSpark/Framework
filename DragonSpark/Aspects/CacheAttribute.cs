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
		readonly static object Instance = new object();

		class Stored : ConnectedValue<WeakReference>
		{
			static string DetermineKey( MethodInterceptionArgs args )
			{
				var result = string.Join( "_",  new object[] { args.Instance?.GetType(), args.Method }.Concat( args.Arguments ).Select( o => o?.GetHashCode() ?? -1 ) );
				return result;
			}

			public Stored( MethodInterceptionArgs args ) : base( args.Instance ?? Instance, DetermineKey( args ), () => args.With( x => x.Proceed() ).ReturnValue.With( item => new WeakReference( item ) ) )
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