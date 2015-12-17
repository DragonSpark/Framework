using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class ReflectionSurrogateFactory<T> : FactoryBase<MemberInfo, T> where T : class
	{
		public static ReflectionSurrogateFactory<T> Instance { get; } = new ReflectionSurrogateFactory<T>();

		readonly IFactory<SurrogateFactory.Parameter, object> factory;
		readonly IFactory<object, Type> locator;

		public ReflectionSurrogateFactory() : this( SurrogateFactory.Instance )
		{}

		public ReflectionSurrogateFactory( IFactory<SurrogateFactory.Parameter, object> factory ) : this( factory, SurrogateTypeLocator.Instance )
		{}

		public ReflectionSurrogateFactory( [Required]IFactory<SurrogateFactory.Parameter, object> factory, [Required]IFactory<object, Type> locator )
		{
			this.factory = factory;
			this.locator = locator;
		}

		protected override T CreateItem( MemberInfo parameter )
		{
			var result = (T)parameter.GetCustomAttributes()
				.Select( attribute => new { attribute, type = locator.Create( attribute ) } )
				.Where( item => item.type.With( typeof(T).Adapt().IsAssignableFrom ) )
				.WithFirst( item => new SurrogateFactory.Parameter( item.attribute, item.type ) )
				.With( factory.Create );
			return result;
		}
	}
}