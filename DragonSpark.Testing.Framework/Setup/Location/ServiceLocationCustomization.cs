using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	/*public class ObjectBuilderCustomization : CustomizationBase
	{
		[Activate]
		public SetupAutoData Context { get; set; }

		protected override void Customize( IFixture fixture )
		{
			var specification = new BuilderSpecification( Context.Method.GetParameters().Select( info => info.ParameterType ).ToArray() );
			fixture.Behaviors.Add( new ObjectBuilderBehavior( specification ) );
		}

		class BuilderSpecification : SpecificationBase<Type>
		{
			readonly Type[] typesToBuild;

			public BuilderSpecification( Type[] typesToBuild )
			{
				this.typesToBuild = typesToBuild;
			}

			protected override bool IsSatisfiedByParameter( Type parameter ) => base.IsSatisfiedByParameter( parameter ) && !typeof(Mock).Adapt().IsAssignableFrom( parameter ) && typesToBuild.Contains( parameter );
		}

		class ObjectBuilderBehavior : ISpecimenBuilderTransformation
		{
			readonly ISpecification<Type> specification;
			
			public ObjectBuilderBehavior( ISpecification<Type> specification )
			{
				this.specification = specification;
			}

			public ISpecimenBuilder Transform( ISpecimenBuilder builder ) => new Builder( builder, specification );

			class Builder : ISpecimenBuilderNode
			{
				readonly ISpecimenBuilder inner;
				readonly ISpecification<Type> specification;
				
				public Builder( ISpecimenBuilder inner, ISpecification<Type> specification )
				{
					this.inner = inner;
					this.specification = specification;
				}

				public ISpecimenBuilderNode Compose( IEnumerable<ISpecimenBuilder> builders )
				{
					var builder = builders.Fixed().With( b => b.Only() ?? new CompositeSpecimenBuilder( b ) );
					var result = new Builder( builder, specification );
					return result;
				}

				public object Create( object request, ISpecimenContext context )
				{
					var item = inner.Create( request, context );
					var result = request.AsTo<Type, object>( type => specification.IsSatisfiedBy( type ) ? item.BuildUp() : null ) ?? item;
					return result;
				}

				

				public IEnumerator<ISpecimenBuilder> GetEnumerator()
				{
					yield return inner;
				}

				IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			}
		}
		
	}*/

	public abstract class CustomizationBase : ICustomization
	{
		[Aspects.BuildUp]
		void ICustomization.Customize( IFixture fixture ) => Customize( fixture );

		protected abstract void Customize( IFixture fixture );
	}

	public class ServiceLocationCustomization : CustomizationBase
	{
		[Activate]
		public IServiceLocator Locator { get; set; }

		[Activate]
		public IServiceLocationAuthority Authority { get; set; }

		protected override void Customize( IFixture fixture ) => Locator.With( locator =>
		{
			new ServiceLocationRelay( locator, new AuthorizedLocationSpecification( locator, Authority ) ).With( fixture.ResidueCollectors.Add );
		} );
	}
	
	class FixtureRegistry : IServiceRegistry
	{
		readonly IFixture fixture;

		public FixtureRegistry( IFixture fixture )
		{
			this.fixture = fixture;
		}

		public void Register( Type @from, Type mappedTo, string name = null ) => fixture.Customizations.Add( new TypeRelay( @from, mappedTo ) );

		public void Register( Type type, object instance ) => this.InvokeGenericAction( nameof(RegisterInstance), new[] { type }, instance );

		void RegisterInstance<T>( T instance ) => fixture.Freeze( instance );

		public void RegisterFactory( Type type, Func<object> factory ) => this.InvokeGenericAction( nameof(RegisterFactory), type.ToItem(), factory );

		void RegisterFactory<T>( Func<object> factory ) => fixture.Customize<T>( c => c.FromFactory( () => (T)factory() ).OmitAutoProperties() );
	}

	public class RegisterFactoryCommand<T> : Command<T> where T : IFactory
	{
		public RegisterFactoryCommand( [Required]IServiceRegistry registry )
		{
			Registry = registry;
		}

		public IServiceRegistry Registry { get; }

		protected override void OnExecute( T parameter )
		{
			var type = FactoryReflectionSupport.GetResultType( typeof(T) );
			Registry.RegisterFactory( type, parameter.Create );
		}
	}

	public class RegisterFactoryGenericCommand<T> : RegisterFactoryCommand<IFactory<T>>
	{
		public RegisterFactoryGenericCommand( IServiceRegistry registry ) : base( registry )
		{}

		protected override void OnExecute( IFactory<T> parameter )
		{
			base.OnExecute( parameter );
			Registry.Register( typeof(Func<T>), parameter.ToDelegate() );
		}
	}

	public static class ServiceRegistryExtensions
	{
		public static void RegisterFactory( this IServiceRegistry @this, Type type, IFactory factory )
		{
			var commands = new[] { Activator.Current.Construct<ICommand>( typeof(RegisterFactoryGenericCommand<>).MakeGenericType( type ), @this ), new RegisterFactoryCommand<IFactory>( @this ) };
			commands.FirstOrDefault( command => command.CanExecute( factory ) ).With( x =>
			{
				x.Execute( factory );
			} );
		}
	}
}