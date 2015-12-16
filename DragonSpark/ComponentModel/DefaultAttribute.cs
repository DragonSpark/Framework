using AutoMapper.Internal;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Class )]
	public class SurrogateForAttribute : Attribute
	{
		public SurrogateForAttribute( Type type )
		{
			Type = type;
		}

		public Type Type { get; }
	}

	public interface ISurrogate
	{
		Type For { get; }
	}

	[AttributeUsage( AttributeTargets.Property )]
	public abstract class DefaultValueBase : SurrogateAttribute
	{
		protected DefaultValueBase( [OfType( typeof(IDefaultValueProvider) )]Type @for ) : base( @for )
		{}
	}

	public abstract class SurrogateAttribute : Attribute, ISurrogate
	{
		protected SurrogateAttribute( [Required]Type @for )
		{
			For = @for;
		}

		public Type For { get; }
	}

	public static class AttributeExtensions
	{
		public static T From<T>( this MemberInfo @this, object source ) where T : class
		{
			var parameter = @this.GetCustomAttributes().Select( SurrogateTypeLocator.Instance.Create ).WithFirst( typeof(T).Adapt().IsAssignableFrom, type => new SurrogateFactory<T>.Parameter( source, type ) );
			var result = new SurrogateFactory<T>().Create( parameter );
			return result;
		}
	}

	public class SurrogateTypeLocator : FactoryBase<object, Type>
	{
		public static SurrogateTypeLocator Instance { get; } = new SurrogateTypeLocator();

		protected override Type CreateItem( object parameter )
		{
			var result =
				parameter.AsTo<ISurrogate, Type>( surrogate => surrogate.For )
				??
				parameter.FromMetadata<SurrogateForAttribute, Type>( attribute => attribute.Type )
				??
				parameter.GetType().With( type => type.Name.Replace( nameof(Attribute), string.Empty ).With( name => type.Assembly().DefinedTypes.AsTypes().Where( info => info.Name == name ).Only() ) );
			return result;
		}
	}

	public class SurrogateFactory<T> : FactoryBase<SurrogateFactory<T>.Parameter, T> where T : class
	{
		protected override T CreateItem( Parameter parameter )
		{
			var constructor = DetermineConstructor( parameter.ActivatedType, parameter.Surrogate );
			var result = constructor.With( info => (T)info.Invoke( GetArguments( info, parameter.Surrogate ) ) );
			return result;
		}

		static object[] GetArguments( MethodBase info, object parameter )
		{
			var result = info.GetParameters().Select( parameterInfo => LocateMember( parameterInfo, parameterInfo ).With( memberInfo => memberInfo.GetMemberValue( parameter ) ) ).ToArray();
			return result;
		}

		static ConstructorInfo DetermineConstructor( Type activatedType, object source )
		{
			var result = activatedType.GetTypeInfo().DeclaredConstructors
				.OrderByDescending( info => info.GetParameters().Length )
				.FirstOrDefault( info => Locate( info, source ) );
			return result;
		}

		static bool Locate( MethodBase info, object source )
		{
			return info.GetParameters().All( parameterInfo => LocateMember( parameterInfo, source ) != null );
		}

		static MemberInfo LocateMember( ParameterInfo parameter, object source )
		{
			var type = source.GetType();
			var member = type.GetRuntimeProperty( parameter.Name.Capitalized() ) ?? (MemberInfo)type.GetRuntimeField( parameter.Name );
			var result = member.With( info => parameter.ParameterType.Adapt().IsAssignableFrom( info.GetMemberType() ) ? info : null ) 
				?? type.GetRuntimeProperties().Cast<MemberInfo>().Concat( type.GetRuntimeFields() ).FirstOrDefault( info => parameter.ParameterType.Adapt().IsAssignableFrom( info.GetMemberType() ) );
			return result;
		}

		public class Parameter
		{
			public Parameter( [Required]object surrogate, [Required]Type permanentType )
			{
				ActivatedType = permanentType;
				Surrogate = surrogate;
			}

			public Type ActivatedType { get; }

			public object Surrogate { get; }
		}
	}

	public class DefaultAttribute : DefaultValueBase
	{
		readonly object value;

		public DefaultAttribute( object value ) : base( typeof(DefaultValueProvider) )
		{
			this.value = value;
		}
	}

	public interface IDefaultValueProvider
	{
		object GetValue( DefaultValueParameter parameter );
	}

	class DefaultValueProvider : IDefaultValueProvider
	{
		readonly object value;

		public DefaultValueProvider( object value )
		{
			this.value = value;
		}

		public virtual object GetValue( DefaultValueParameter parameter )
		{
			var result = value.ConvertTo( parameter.Metadata.PropertyType );
			return result;
		}
	}
}