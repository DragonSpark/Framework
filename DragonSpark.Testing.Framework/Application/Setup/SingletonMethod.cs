using DragonSpark.Activation.Location;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class SingletonMethod : SuppliedSource<Type, object>, IMethod
	{
		public SingletonMethod( Type parameter ) : this( parameter, SourceAccountedAlteration.Defaults.Get( parameter ) ) {}

		[UsedImplicitly]
		public SingletonMethod( Type parameter, Func<object, object> account ) : base( SingletonLocator.Default.Apply( account ).Get, parameter ) {}

		public IEnumerable<ParameterInfo> Parameters { get; } = Items<ParameterInfo>.Default;

		object IMethod.Invoke( IEnumerable<object> parameters )
		{
			var invoke = Get();
			return invoke;
		}
	}
}