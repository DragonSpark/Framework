using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using DragonSpark.Activation;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.ComponentModel
{
	/*public class ExtensionAttribute : DefaultValueBase
	{
		public ExtensionAttribute( string name = null ) : base( t => new ActivatedValueProvider( new ActivatedValueProvider.Converter<IUnityContainer>( name ).Create, Creator.Instance.Create ) ) {}

		public class Creator : ActivatedValueProvider.Creator<IUnityContainerExtensionConfigurator>
		{
			public new static Creator Instance { get; } = new Creator();

			readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, IUnityContainer> factory;

			public Creator() : this( new ActivatedValueProvider.Creator<IUnityContainer>().Create ) { }

			protected Creator( [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, IUnityContainer> factory )
			{
				this.factory = factory;
			}

			protected override IUnityContainerExtensionConfigurator CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter ).Extension( parameter.Item2.Metadata.PropertyType );
		}
	}*/

	public class LocateAttribute : DefaultValueBase
	{
		public LocateAttribute() : this( null ) { }

		public LocateAttribute( string name ) : this( null, name ) { }

		public LocateAttribute( Type locatedType, string name = null ) : base( t => new ActivatedValueProvider( new ActivatedValueProvider.Converter( locatedType, name ).Create, Factory.Instance.Create ) ) { }
		
		public class Factory : ActivatedValueProvider.Creator<object>
		{
			public static Factory Instance { get; } = new Factory();

			readonly ServiceLocatorProvider locator;

			Factory() : this( Services.GetLocator )
			{}

			Factory( [Required]ServiceLocatorProvider locator )
			{
				this.locator = locator;
			}

			protected override object CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter )
			{
				var serviceLocator = locator();
				var instance = serviceLocator.GetInstance( parameter.Item1.Type, parameter.Item1.Name );
				var result = instance ?? base.CreateItem( parameter );
				return result;
			}
		}
	}

	public class ActivateAttribute : ActivateAttributeBase
	{
		public ActivateAttribute() : this( null ) { }

		public ActivateAttribute( string name ) : this( null, name ) { }

		public ActivateAttribute( Type locatedType, string name = null ) : base( t => new ActivatedValueProvider( new ActivatedValueProvider.Converter( locatedType, name ).Create, ActivatedValueProvider.Creator.Instance.Create ) ) { }
	}

	public abstract class ActivateAttributeBase : DefaultValueBase
	{
		protected ActivateAttributeBase( ActivatedValueProvider.Converter converter, ActivatedValueProvider.Creator creator ) : base( t => new ActivatedValueProvider( converter, creator ) ) {}

		protected ActivateAttributeBase( Func<object, IDefaultValueProvider> provider ) : base( provider ) {}
	}

	public class ActivatedValueProvider : IDefaultValueProvider
	{
		readonly Func<PropertyInfo, ActivateParameter> convert;
		readonly Func<Tuple<ActivateParameter, DefaultValueParameter>, object> create;

		public ActivatedValueProvider( Converter converter ) : this( converter, Creator.Instance ) {}

		public ActivatedValueProvider( Converter converter, Creator creator ) : this( converter.Create, creator.Create ) {}

		public ActivatedValueProvider( [Required]Func<PropertyInfo, ActivateParameter> convert ) : this( convert, Creator.Instance.Create ) {}

		public ActivatedValueProvider( [Required]Func<PropertyInfo, ActivateParameter> convert, [Required]Func<Tuple<ActivateParameter, DefaultValueParameter>, object> create )
		{
			this.convert = convert;
			this.create = create;
		}

		public object GetValue( DefaultValueParameter parameter )
		{
			var activateParameter = convert( parameter.Metadata );
			var context = Tuple.Create( activateParameter, parameter );
			return create( context );
		}

		public class Converter<T> : Converter
		{
			public Converter( string name ) : base( typeof(T), name ) { }
		}

		public class Converter : FactoryBase<PropertyInfo, ActivateParameter>
		{
			readonly Func<PropertyInfo, Type> type;
			readonly string name;

			public Converter( Type activatedType, string name ) : this( p => activatedType ?? p.PropertyType, name ) { }

			public Converter( [Required]Func<PropertyInfo, Type> type, string name )
			{
				this.type = type;
				this.name = name;
			}

			protected override ActivateParameter CreateItem( PropertyInfo parameter ) => new ActivateParameter( type( parameter ), name );
		}

		public class Creator : Creator<object>
		{
			public static Creator Instance { get; } = new Creator();
		}

		public class Creator<T> : FactoryBase<Tuple<ActivateParameter, DefaultValueParameter>, T> where T : class
		{
			// public static Creator<T> Instance { get; } = new Creator<T>();

			readonly Func<ActivateParameter, T> factory;

			public Creator() : this( ActivateFactory<T>.Instance.Create ) { }

			public Creator( [Required]Func<ActivateParameter, T> factory )
			{
				this.factory = factory;
			}

			protected override T CreateItem( Tuple<ActivateParameter, DefaultValueParameter> parameter ) => factory( parameter.Item1 );
		}
	}
}