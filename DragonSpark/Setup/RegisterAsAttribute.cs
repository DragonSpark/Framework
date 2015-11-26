using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	public sealed class RegisterAsAttribute : RegistrationBaseAttribute
	{
		public RegisterAsAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAsAttribute( string name ) : this( null, name )
		{}

		public RegisterAsAttribute( Type @as, string name )
		{
			As = @as;
			Name = name;
		}

		public Type As { get; }

		public string Name { get; }

		protected override void PerformRegistration( IServiceRegistry registry, Type subject )
		{
			var from = As ?? subject.Extend().GetAllInterfaces().First( type => subject.Name.Contains( type.Name.Substring( 1 ) ) );
			registry.Register( from, subject, Name );
		}
	}

	public class FactoryBuiltObjectFactory : ActivateFactoryBase<ObjectFactoryContext, object>
	{
		public static FactoryBuiltObjectFactory Instance { get; } = new FactoryBuiltObjectFactory();

		public FactoryBuiltObjectFactory() : base( item => item.FactoryType )
		{}

		protected override object CreateItem( ObjectFactoryContext parameter )
		{
			var item = base.CreateItem( parameter );
			var result = item.AsTo<IFactory, object>( factory => factory.Create() ) ?? item.AsTo<IFactoryWithParameter, object>( factory => DetermineResultFromContext( factory, parameter ) );
			return result;
		}

		object DetermineResultFromContext( IFactoryWithParameter factory, ObjectFactoryContext context )
		{
			var parameterType = FactoryReflectionSupport.Instance.GetParameterType( factory.GetType() );
			var parameter = parameterType.IsInstanceOfType( context.Context ) ? context.Context : Activate( context, parameterType );
			var result = parameter.Transform( factory.Create );
			return result;
		}

		object Activate( ObjectFactoryContext context, TypeExtension parameterType )
		{
			var type = context.Context.AsTo<Type, Type>( t => parameterType.IsAssignableFrom( t ) ? t : null ) ?? parameterType;
			var result = Activate<object>( type );
			return result;
		}
	}

	public class ObjectFactoryContext
	{
		public ObjectFactoryContext( Type factoryType, object context = null )
		{
			FactoryType = factoryType;
			Context = context;
		}

		public Type FactoryType { get;}
		public object Context { get; }
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
			FactoryReflectionSupport.Instance.GetResultType( subject ).With( type =>
			{
				registry.RegisterFactory( type, () =>
				{
					var result = FactoryBuiltObjectFactory.Instance.Create( new ObjectFactoryContext( subject ) );
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