using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Patterns.Contracts;
using System;
using Activator = System.Activator;

namespace DragonSpark.TypeSystem
{
	public abstract class HostingAttribute : Attribute, IValue
	{
		readonly IValue inner;

		protected HostingAttribute( [Required]Delegate factory ) : this( (IValue)Activator.CreateInstance( typeof(AttributeValueAdapter<>).MakeGenericType( factory.Adapt().GetResultType() ), factory ) )
		{}

		protected HostingAttribute( IValue inner )
		{
			this.inner = inner;
		}

		public object Item => inner.Item;
	}

	public class AttributeValueAdapter<T> : Value<T>
	{
		readonly Lazy<T> factory;

		public AttributeValueAdapter( Func<T> factory ) : this( new Lazy<T>( factory ) ) {}

		public AttributeValueAdapter( Lazy<T> factory )
		{
			this.factory = factory;
		}

		public override T Item => factory.Value;
	}
}