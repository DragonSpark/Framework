using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Setup.Registration
{
	public sealed class RegisterAttribute : RegistrationBaseAttribute
	{
		readonly Type @as;
		readonly string name;

		public RegisterAttribute() : this( null, null )
		{}

		public RegisterAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAttribute( string name ) : this( null, name )
		{}

		RegisterAttribute( Type @as, string name )
		{
			this.@as = @as;
			this.name = name;
		}

		protected override void PerformRegistration( IServiceRegistry registry, Type subject )
		{
			var from = @as ?? subject.Extend().GetAllInterfaces().With( types => types.FirstOrDefault( type => subject.Name.Contains( type.Name.Substring( 1 ) ) ) ?? types.FirstOrDefault() ) ?? subject;
			registry.Register( from, subject, @name );
		}
	}

	/*public class ParameterFactory : FactoryBase<object, object>
	{
		protected override object CreateItem( object parameter )
		{
			var parameterType = FactoryReflectionSupport.Instance.GetParameterType( factory.GetType() );
			var activated = parameterType.IsInstanceOfType( parameter.Context ) ? parameter.Context : Activate( parameter, parameterType );
		}

		object Activate( ObjectFactoryParameter parameter, TypeExtension parameterType )
		{
			var type = parameter.Context.AsTo<Type, Type>( t => parameterType.IsAssignableFrom( t ) ? t : null ) ?? parameterType;
			var result = Activator.Activate<object>( type );
			return result;
		}
	}*/
}