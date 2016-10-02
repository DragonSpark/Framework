using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public sealed class Delegates : Cache<ICache<MethodInfo, Delegate>>
	{
		public static Delegates Default { get; } = new Delegates();
		Delegates() : base( o => new Cache( o ) ) {}

		sealed class Cache : FactoryCache<MethodInfo, Delegate>
		{
			readonly object instance;

			public Cache( object instance )
			{
				this.instance = instance;
			}

			protected override Delegate Create( MethodInfo parameter )
			{
				var info = parameter.AccountForClosedDefinition( instance.GetType() );
				var delegateType = DelegateType.Default.Get( info );
				var result = info.CreateDelegate( delegateType, parameter.IsStatic ? null : instance );
				return result;
			}
		}
	}
}