using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.ComponentModel
{
	public class ExtensionAttribute : DefaultValueBase
	{
		public ExtensionAttribute( string name = null ) : base( () => new ActivatedValueProvider( new ActivateAttribute.ParameterFactory<IUnityContainer>( name ).Create, new Factory().Create ) ) {}

		public class Factory : ActivateAttribute.Factory<IUnityContainerExtensionConfigurator>
		{
			readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, IUnityContainer> factory;
			public Factory() : this( new ActivateAttribute.Factory<IUnityContainer>().Create ) { }

			protected Factory( [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, IUnityContainer> factory )
			{
				this.factory = factory;
			}

			protected override IUnityContainerExtensionConfigurator CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter ).Extension( parameter.Item2.Metadata.PropertyType );
		}
	}

	public class LocateAttribute : DefaultValueBase
	{
		public LocateAttribute() : this( (string)null ) { }

		public LocateAttribute( string name ) : this( null, name ) { }

		public LocateAttribute( Type locatedType, string name = null ) : base( () => new ActivatedValueProvider( new ActivateAttribute.ParameterFactory( locatedType, name ).Create, new Factory().Create ) ) { }
		
		public class Factory : ActivateAttribute.Factory<object>
		{
			readonly IServiceLocator locator;

			public Factory() : this( Services.Location.Item )
			{}

			public Factory( [Required]IServiceLocator locator )
			{
				this.locator = locator;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter )
			{
				var instance = locator.GetInstance( parameter.Item1.Type, parameter.Item1.Name );
				return instance;
			}
		}
	}

	public class ActivateAttribute : DefaultValueBase
	{
		public ActivateAttribute() : this( (string)null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType, string name = null ) : this( () => new ActivatedValueProvider( new ParameterFactory( activatedType, name ).Create, new Factory<object>().Create ) )
		{}

		protected ActivateAttribute( Func<IDefaultValueProvider> provider ) : base( provider )
		{}

		public class ParameterFactory<T> : ParameterFactory
		{
			public ParameterFactory( string name ) : base( typeof( T ), name ) { }
		}

		public class ParameterFactory : FactoryBase<PropertyInfo, ActivateParameter>
		{
			readonly Func<PropertyInfo, Type> type;
			readonly string name;

			public ParameterFactory( Type activatedType, string name ) : this( p => activatedType ?? p.PropertyType, name )
			{}

			public ParameterFactory( [Required]Func<PropertyInfo, Type> type, string name )
			{
				this.type = type;
				this.name = name;
			}

			protected override ActivateParameter CreateItem( PropertyInfo parameter ) => new ActivateParameter( type( parameter ), name );
		}

		public class Factory<T> : FactoryBase<Tuple<ActivateParameter, DefaultValueParameter>, T> where T : class
		{
			readonly Func<ActivateParameter, T> factory;

			public Factory() : this( ActivateFactory<T>.Instance.Create ) { }

			public Factory( [Required]Func<ActivateParameter, T> factory )
			{
				this.factory = factory;
			}

			protected override T CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter.Item1 );
		}
	}

	public class ActivatedValueProvider : IDefaultValueProvider
	{
		readonly Func<PropertyInfo, ActivateParameter> createParameter;
		readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, object> create;

		public ActivatedValueProvider( [Required]Func<PropertyInfo, ActivateParameter> createParameter, [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, object> create )
		{
			this.createParameter = createParameter;
			this.create = create;
		}

		public object GetValue( DefaultValueParameter parameter ) => create( Tuple.Create( createParameter( parameter.Metadata ), parameter ) );
	}
}