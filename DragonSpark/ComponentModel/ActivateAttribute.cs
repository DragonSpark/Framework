using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ExtensionAttribute : ActivateAttribute
	{
		public ExtensionAttribute(string name = null ) : base( () => new ExtensionProvider( name ) )
		{}
	}

	public class ExtensionProvider : ActivatedValueProvider
	{
		public ExtensionProvider( string name = null ) : base( null, name )
		{}

		protected override object Activate( Parameter parameter )
		{
			var result = base.Activate( parameter ).AsTo<IUnityContainer, IUnityContainerExtensionConfigurator>( container => container.Extension( parameter.Metadata.PropertyType ) );
			return result;
		}

		protected override Type DetermineType( PropertyInfo propertyInfo )
		{
			return typeof(IUnityContainer);
		}
	}

	public class ActivateAttribute : DefaultValueBase
	{
		public ActivateAttribute() : this( (string)null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType, string name = null ) : this( () => new ActivatedValueProvider( activatedType, name ) )
		{}

		public ActivateAttribute( Func<IDefaultValueProvider> provider ) : base( provider )
		{}
	}

	public class ActivatedValueProvider : IDefaultValueProvider
	{
		public ActivatedValueProvider( Type activatedType, string name ) : this( Activation.Activator.Current, activatedType, name )
		{}

		public ActivatedValueProvider( [Required]IActivator activator, Type activatedType, string name )
		{
			Activator = activator;
			ActivatedType = activatedType;
			Name = name;
		}

		protected IActivator Activator { get; }
		protected Type ActivatedType { get; }
		protected string Name { get; }

		public object GetValue( DefaultValueParameter parameter )
		{
			var type = ActivatedType ?? DetermineType( parameter.Metadata );
			var adapted = new Parameter( type, parameter.Instance, parameter.Metadata );
			var result = Activate( adapted );
			return result;

			/*var result = new[] { DetermineType( parameter.Metadata ), ActivatedType }
				.NotNull()
				.Select( t => new Parameter( t, parameter.Instance, parameter.Metadata ) )
				.Select( Activate )
				.NotNull()
				.FirstOrDefault();
			return result;*/
		}

		protected virtual object Activate( Parameter parameter )
		{
			var result = Activator.Activate<object>( parameter.ActivatedType, Name )
				/*?? 
				parameter.ActivatedType.Adapt().DetermineImplementor().With( info => Activator.Activate<object>( info.AsType() ) )
				?? 
				SingletonLocator.Instance.Locate( parameter.ActivatedType )*/;
			return result;
		}

		protected virtual Type DetermineType( PropertyInfo propertyInfo )
		{
			return propertyInfo.PropertyType;
		}

		public class Parameter : DefaultValueParameter
		{
			public Parameter( Type activatedType, object instance, PropertyInfo metadata ) : base( instance, metadata )
			{
				ActivatedType = activatedType;
			}

			public Type ActivatedType { get; }
		}
	}
}