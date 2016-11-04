using DragonSpark.Expressions;
using DragonSpark.Sources.Scopes;
using DragonSpark.TypeSystem;
using DragonSpark.TypeSystem.Generics;
using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class AmbientStack<T> : Scope<IStack<T>>, IStackSource<T>
	{
		public static AmbientStack<T> Default { get; } = new AmbientStack<T>();

		public AmbientStack() : this( Stacks<T>.Default ) {}
		public AmbientStack( IParameterizedSource<object, IStack<T>> source ) : base( source.ToDelegate() ) {}

		public T GetCurrentItem() => Get().Peek();
	}

	public static class AmbientStack
	{
		readonly static IGenericMethodContext<Invoke> Method = typeof(AmbientStack).Adapt().GenericFactoryMethods[nameof(GetCurrentItem)];

		public static object GetCurrentItem( Type type ) => Method.Make( type ).Invoke<object>();

		public static T GetCurrentItem<T>() => AmbientStack<T>.Default.GetCurrentItem();

		public static StackAssignment<T> Assignment<T>( this IStackSource<T> @this, T item )  => new StackAssignment<T>( @this, item );
	}
}