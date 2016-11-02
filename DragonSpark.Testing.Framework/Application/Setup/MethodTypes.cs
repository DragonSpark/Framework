using DragonSpark.Application;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Testing.Framework.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	sealed class MethodTypes : CacheWithImplementedFactoryBase<ImmutableArray<Type>>, ITypeSource
	{
		readonly static Func<object, ImmutableArray<Func<MethodBase, ImmutableArray<Type>>>> Locator = HostedValueLocator<Func<MethodBase, ImmutableArray<Type>>>.Default.Get;

		public static MethodTypes Default { get; } = new MethodTypes();
		MethodTypes() : this( CurrentMethod.Default.Get ) {}

		readonly Func<MethodBase> methodSource;
		readonly Func<object, ImmutableArray<Func<MethodBase, ImmutableArray<Type>>>> locator;
		readonly Func<object, ImmutableArray<Type>> selector;

		public MethodTypes( Func<MethodBase> methodSource ) : this( methodSource, Locator ) {}

		public MethodTypes( Func<MethodBase> methodSource, Func<object, ImmutableArray<Func<MethodBase, ImmutableArray<Type>>>> locator )
		{
			this.methodSource = methodSource;
			this.locator = locator;
			selector = Get;
		}

		protected override ImmutableArray<Type> Create( object parameter )
		{
			var methodBase = methodSource();
			var immutableArray = locator( parameter );
			var enumerable = immutableArray.Introduce( methodBase ).Concat();
			var result = enumerable.ToImmutableArray();
			return result;
		}

		public ImmutableArray<Type> Get() => this.ToImmutableArray();

		object ISource.Get() => Get();

		public IEnumerator<Type> GetEnumerator()
		{
			var method = methodSource();
			var result = new object[] { method, method.DeclaringType, method.DeclaringType.Assembly }.Select( selector ).Concat().GetEnumerator();
			return result;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}