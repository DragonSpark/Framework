using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public sealed class SingletonDelegates : SingletonDelegates<Func<object>>
	{
		/*public static ISource<SingletonDelegates> Current { get; } = new Scope<SingletonDelegates>( Factory.GlobalCache( () => new SingletonDelegates() ) );*/
		public static SingletonDelegates Default { get; } = new SingletonDelegates();
		SingletonDelegates() : this( SingletonProperties.Default ) {}
		public SingletonDelegates( IParameterizedSource<Type, PropertyInfo> source ) : base( source.ToSourceDelegate(), SingletonPropertyDelegates.Default.Get ) {}
	}
}