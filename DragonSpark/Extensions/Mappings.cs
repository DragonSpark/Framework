using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Extensions
{
	public static class Mappings
	{
		public static Action<IMappingExpression> OnlyProvidedValues()
		{
			return x => x.ForAllMembers( options => options.Condition( condition => !condition.IsSourceValueNull ) );
		}

		public static IMappingExpression IgnoreUnassignable( this IMappingExpression expression, Type sourceType, Type destinationType )
		{
			Mapper.FindTypeMapFor( sourceType, destinationType )
				.GetPropertyMaps()
				.Where( map => !map.DestinationPropertyType.Adapt().IsAssignableFrom( map.SourceMember.To<PropertyInfo>().PropertyType ) )
				.Each( map =>
				{
					expression.ForMember( map.SourceMember.Name, opt => opt.Ignore() );
				} );
			return expression;
		}
		
		public static TResult MapInto<TResult>( this object source, TResult existing = null, Action<IMappingExpression> configure = null ) where TResult : class 
		{
			var context = new ObjectMappingContext<TResult>( source, existing, configure );
			var factory = Activator.Current.Activate<ObjectMappingFactory<TResult>>();
			var result = factory.Create( context );
			return result;
		}
	}
}