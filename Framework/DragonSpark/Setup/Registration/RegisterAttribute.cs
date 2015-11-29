using System;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Setup.Registration
{
	public sealed class RegisterAttribute : RegistrationBaseAttribute
	{
		public RegisterAttribute() : this( null, null )
		{}

		public RegisterAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAttribute( string name ) : this( null, name )
		{}

		public RegisterAttribute( Type @as, string name )
		{
			As = @as;
			Name = name;
		}

		public Type As { get; }

		public string Name { get; }

		protected override void PerformRegistration( IServiceRegistry registry, Type subject )
		{
			var from = As ?? subject.Extend().GetAllInterfaces().FirstOrDefault( type => subject.Name.Contains( type.Name.Substring( 1 ) ) ) ?? subject;
			registry.Register( from, subject, Name );
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