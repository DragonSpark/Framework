using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Linq;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup
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

	public class FactoryBuiltObjectFactory : ActivateFactory<ObjectFactoryParameter, object>
	{
		public FactoryBuiltObjectFactory( IActivator activator ) : base( activator )
		{}

		protected override object Activate( Type qualified, ObjectFactoryParameter parameter )
		{
			var item = base.Activate( qualified, parameter );
			var result = item.AsTo<IFactory, object>( factory => factory.Create() ) ?? item.AsTo<IFactoryWithParameter, object>( factory => factory.Create( parameter.Context ) );
			return result;
		}

		/*object DetermineResultFromContext( IFactoryWithParameter factory, ObjectFactoryParameter parameter )
		{
			var result = activated.Transform( factory.Create );
			return result;
		}*/
		
		/*protected override ObjectFactoryParameter QualifyParameter( object parameter )
		{
			return base.QualifyParameter( parameter ) ?? parameter.AsTo<Type, ObjectFactoryParameter>( type => type );
		}*/
	}

	public class ObjectFactoryParameter : ActivateParameter
	{
		public ObjectFactoryParameter( Type factoryType ) : this( factoryType, null )
		{}

		public ObjectFactoryParameter( Type factoryType, object context ) : base( factoryType )
		{
			Context = context;
		}

		public object Context { get; }

		public static implicit operator ObjectFactoryParameter( Type type )
		{
			var result = new ObjectFactoryParameter( type );
			return result;
		}
	}

	class FactoryReflectionSupport
	{
		public static FactoryReflectionSupport Instance { get; } = new FactoryReflectionSupport();
		static TypeExtension[] Types { get; } = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => type.Extend() ).ToArray();

		public TypeExtension GetResultType( TypeExtension factoryType )
		{
			var result = Get( factoryType, types => types.Last(), Types );
			return result;
		}

		Type Get( TypeExtension factoryType, Func<Type[],Type> selector, params TypeExtension[] typesToCheck )
		{
			var result = factoryType.GetAllInterfaces().AsTypeInfos().Where( type => type.IsGenericType && typesToCheck.Any( extension => extension.IsAssignableFrom( type.GetGenericTypeDefinition() ) ) ).Select( type => selector( type.GenericTypeArguments ) ).FirstOrDefault();
			return result;
		}

		public TypeExtension GetParameterType( TypeExtension factoryType )
		{
			var result = Get( factoryType, types => types.First(), Types.Last() );
			return result;
		}
	}

	public class RegisterFactoryForResultAttribute : RegistrationBaseAttribute
	{
		protected override void PerformRegistration( IServiceRegistry registry, Type subject )
		{
			var typeExtension = FactoryReflectionSupport.Instance.GetResultType( subject );
			typeExtension.With( type =>
			{
				registry.RegisterFactory( type, () =>
				{
					var factory = Activator.Current.Activate<FactoryBuiltObjectFactory>();
					var result = factory.Create( new ObjectFactoryParameter( subject ) );
					return result;
				} );
			} );
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : Attribute, IConventionRegistration
	{
		protected abstract void PerformRegistration( IServiceRegistry registry, Type subject );

		public void Register( IServiceRegistry registry, Type subject )
		{
			PerformRegistration( registry, subject );
		}
	}

	public interface IConventionRegistration
	{
		void Register( IServiceRegistry registry, Type subject );
	}
}