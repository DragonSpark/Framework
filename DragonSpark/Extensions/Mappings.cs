using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Extensions
{
	public static class Mappings
	{
		/*delegate object Method();
		static readonly MethodInfo MethodInfo = typeof(Mappings).GetTypeInfo().DeclaredMethods.First( m => m.Name == nameof(OnlyProvidedValues) && m.IsGenericMethod && m.IsPublic && m.IsStatic );*/

		/*public static object OnlyProvidedValues( Type source, Type destination = null )
		{
			var resolver = (Method)MethodInfo.MakeGenericMethod( source, destination ?? source ).CreateDelegate( typeof(Method) );
			var result = resolver();
			return result;
		}*/

		public static Action<IMappingExpression> OnlyProvidedValues()
		{
			return x => x.ForAllMembers( options => options.Condition( condition => !condition.IsSourceValueNull ) );
		}

		public static IMappingExpression IgnoreUnassignable( this IMappingExpression expression, Type sourceType, Type destinationType )
		{
			Mapper.FindTypeMapFor( sourceType, destinationType )
				.GetPropertyMaps()
				.Where( map => !map.DestinationPropertyType.IsAssignableFrom( map.SourceMember.To<PropertyInfo>().PropertyType ) )
				.Apply( map =>
				{
					
					expression.ForMember( map.SourceMember.Name, opt => opt.Ignore() );
				} );
			return expression;
		}

		
		/*public static object MapInto( this object source, object existing, object configure )
		{
			var type = source.GetType();
			var result = typeof(ObjectExtensions).InvokeGeneric( nameof(MapInto), new[] { type, existing?.GetType() ?? type }, source, existing, configure );
			return result;
		}*/

		public static TResult MapInto<TResult>( this object source, TResult existing = null, Action<IMappingExpression> configure = null ) where TResult : class 
		{
			var sourceType = source.GetType();
			var destinationType = existing?.GetType() ?? ( typeof(TResult) == typeof(object) ? sourceType : typeof(TResult) );
				
			Mapper.FindTypeMapFor( sourceType, destinationType ).Null( () =>
			{
				var expression = Mapper.CreateMap( sourceType, destinationType ).IgnoreUnassignable( sourceType, destinationType );
				expression.TypeMap.DestinationCtor = x => existing ?? Activator.CreateInstance<object>( x.DestinationType );
				configure.With( x => x( expression ) );
			} );
			var result = Mapper.DynamicMap( source, sourceType, destinationType );
			return (TResult)result;
		}
	}
}