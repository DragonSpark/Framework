using System;
using System.Linq;
using System.Reflection;
using AutoMapper.Internal;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	public class SurrogateFactory : FactoryBase<SurrogateFactory.Parameter, object>
	{
		public static SurrogateFactory Instance { get; } = new SurrogateFactory();

		protected override object CreateItem( Parameter parameter )
		{
			var constructor = DetermineConstructor( parameter.ActivatedType, parameter.Surrogate );
			var result = constructor.With( info => info.Invoke( GetArguments( info, parameter.Surrogate ) ) );
			return result;
		}

		static object[] GetArguments( MethodBase info, object parameter )
		{
			var result = info.GetParameters().Select( parameterInfo => LocateMember( parameterInfo, parameter ).With( memberInfo => memberInfo.GetMemberValue( parameter ) ) ).ToArray();
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

		[Cache]
		static MemberInfo LocateMember( ParameterInfo parameter, object source )
		{
			var type = source.GetType();
			var typeInfo = type.GetTypeInfo();
			var member = typeInfo.GetDeclaredProperty( parameter.Name.Capitalized() ) ?? (MemberInfo)typeInfo.GetDeclaredField( parameter.Name );
			var result = member.With( info => parameter.ParameterType.Adapt().IsAssignableFrom( info.GetMemberType() ) ? info : null ) 
						 ?? type.GetRuntimeProperties().Cast<MemberInfo>().Concat( type.GetRuntimeFields() ).FirstOrDefault( info => parameter.ParameterType.Adapt().IsAssignableFrom( info.GetMemberType() ) );
			return result;
		}

		public class Parameter
		{
			public Parameter( [Required]object surrogate, [Required]Type permanentType )
			{
				Surrogate = surrogate;
				ActivatedType = permanentType;
			}

			public object Surrogate { get; }
			public Type ActivatedType { get; }
		}
	}
}