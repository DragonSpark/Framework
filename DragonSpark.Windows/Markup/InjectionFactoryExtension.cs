using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Activation.Build;
using DragonSpark.Activation.IoC;
using DragonSpark.Setup.Commands;

namespace DragonSpark.Windows.Markup
{
	[MarkupExtensionReturnType( typeof(InjectionMember) )]
	public class InjectionFactoryExtension : FactoryExtension<InjectionMemberContext, InjectionMember>
	{
		public InjectionFactoryExtension()
		{}

		public InjectionFactoryExtension( Type type ) : base( type )
		{}

		public InjectionFactoryExtension( Type type, object parameter ) : base( type, parameter )
		{}

		public InjectionFactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName, parameter )
		{}

		protected override IFactory<InjectionMemberContext, InjectionMember> DetermineFactory( IServiceProvider serviceProvider )
		{
			return new InjectionFactoryFactory( Type, Parameter );
		}

		protected override InjectionMemberContext DetermineParameter( IServiceProvider serviceProvider )
		{
			var targetType = serviceProvider.Get<DeferredContext>().RegistrationType;
			var result = new InjectionMemberContext( Services.Location.Locate<IUnityContainer>(), targetType );
			return result;
		}

		static Type Resolve( IServiceProvider serviceProvider )
		{
			var xamlType = serviceProvider.Get<IXamlSchemaContextProvider>().SchemaContext.GetXamlType( typeof(UnityRegistrationCommand) );
			var member = xamlType.GetMember( "RegistrationType" );
			var value = serviceProvider.Get<IAmbientProvider>().GetFirstAmbientValue( new [] { xamlType }, member );
			var result = (Type)value.Value;
			return result;
		}

		protected override IServiceProvider Prepare( IServiceProvider serviceProvider, IProvideValueTarget target, IMarkupTargetValueSetterBuilder builder )
		{
			var result = new DeferredContext( serviceProvider, target.TargetObject, target.TargetProperty, builder.GetPropertyType( target ), Resolve( serviceProvider ) );
			return result;
		}

		public class DeferredContext : Markup.DeferredContext
		{
			public DeferredContext( IServiceProvider inner, object targetObject, object targetProperty, Type propertyType, Type registrationType ) : base( inner, targetObject, targetProperty, propertyType )
			{
				RegistrationType = registrationType;
			}

			public Type RegistrationType { get; }
		}
	}
}