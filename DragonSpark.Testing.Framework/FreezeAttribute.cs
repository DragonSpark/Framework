using System.Threading;
using DragonSpark.Activation;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;

namespace DragonSpark.Testing.Framework
{
	/*public interface ITypeCustomizer
	{
		Type From { get; }

		Type To { get; }

		void Customize( object instance );
	}*/

	/*public abstract class TypeCustomizer<TFrom, TTo> : ITypeCustomizer where TTo : class
	{
		Type ITypeCustomizer.From
		{
			get { return typeof(TFrom); }
		}

		Type ITypeCustomizer.To
		{
			get { return typeof(TTo); }
		}

		void ITypeCustomizer.Customize( object instance )
		{
			instance.As<TTo>( Customize );
		}

		protected abstract void Customize( TTo instance );
	}*/

	/*public class CustomizeFreezeAttribute : FreezeAttribute
	{
		readonly Type customizerType;

		public CustomizeFreezeAttribute( Type type, Type customizerType ) : this( type, type, customizerType )
		{}

		public CustomizeFreezeAttribute( Type @from, Type to, Type customizerType ) : base( @from, to )
		{
			this.customizerType = customizerType;
		}

		public override object GetInstance( IServiceLocator locator, Type type )
		{
			customizerType.With( x =>
			{
				var customizer = locator.GetInstance( customizerType ).To<ICustomization>();
				var fixture = locator.GetInstance<IFixture>();
				customizer.Customize( fixture );
			} );
			
			var result = base.GetInstance( locator, type );
			return result;
		}
	}*/

	public class FreezeAttribute : RegistrationAttribute
	{
		readonly ISpecimenBuilder builder = new MethodInvoker( new ModestConstructorQuery() );

		public FreezeAttribute( Type type ) : base( type )
		{}

		public FreezeAttribute( Type @from, Type to ) : base( @from, to )
		{}

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			var context = new SpecimenContext( fixture );
			var factory = new DelegatedBuilder( () => builder.Create( MappedTo, context ) );
			;
			var item = new CompositeSpecimenBuilder( new[] { RegistrationType, MappedTo }
				.Distinct()
				.Select( x => SpecimenBuilderNodeFactory.CreateTypedNode( x, factory ) ) );
			fixture.Customizations.Insert( 0, item );
		}

		class DelegatedBuilder : ISpecimenBuilder
		{
			readonly Lazy<object> resolver;

			public DelegatedBuilder( Func<object> resolver )
			{
				this.resolver = new Lazy<object>( resolver );
			}

			public object Create( object request, ISpecimenContext context )
			{
				var result = resolver.Value;
				return result;
			}
		}
	}
}